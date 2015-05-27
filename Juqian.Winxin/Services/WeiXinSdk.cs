using Juqian.Winxin.Models;
using Orchard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.Localization;
using System.Threading;
using Orchard.UI.Notify;
using Orchard.MediaLibrary.Models;
using System.Net;
using System.Web.Helpers;

namespace Juqian.Winxin.Services
{
    public class WeiXinSdk : IWeiXinSdk
    {
        readonly static string URL_MENUGET = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}";
        readonly static string URL_MENUDELETE = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}";
        readonly static string URL_MENUCREATE = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";
        readonly static string URL_ACCESS_TOKEN = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
        readonly static string URL_UPLOADFILE = "http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}";

        readonly static int SleepTime = 3000;
        readonly static int TryCount = 3;

        public readonly IWinXinService _winXinService;
        private readonly IOrchardServices _orchardServices;
        public Localizer T { get; set; }

        WinXinSettingsPart WinXinSettingsPart;
        public WeiXinSdk(IWinXinService winXinService, IOrchardServices orchardServices)
        {
            _winXinService = winXinService;
            _orchardServices = orchardServices;
            T = NullLocalizer.Instance;

            WinXinSettingsPart = _orchardServices.WorkContext.CurrentSite.As<WinXinSettingsPart>();
        }

        #region 公共方法

        string GetApiUrl(string url, params string[] args)
        {
            if (!string.IsNullOrWhiteSpace(WinXinSettingsPart.AccessToken))
            {
                var list = args.ToList();
                list.Insert(0, WinXinSettingsPart.AccessToken);
                return string.Format(url, list.ToArray());
            }
            else
            {
                RefreshToken();
                return GetApiUrl(url, args);
            }
        }

        void RefreshToken(int tryCount = 0)
        {
            var json = _winXinService.GetWebString(string.Format(URL_ACCESS_TOKEN, WinXinSettingsPart.AppId, WinXinSettingsPart.Secret));
            dynamic result = System.Web.Helpers.Json.Decode(json);
            try
            {
                if (result.errcode != null && result.errcode != 0)
                {
                    throw new WXResultException(result.errcode, result.errmsg);
                }
            }
            catch (WXResultException ex)
            {
                if (ex.Code == -1 && tryCount < TryCount)//系统繁忙
                {
                    Thread.Sleep(SleepTime);
                    RefreshToken(tryCount++);
                }
                else
                    throw ex;
            }
            WinXinSettingsPart.AccessToken = result.access_token;
            WinXinSettingsPart.ExpiresIn = result.expires_in;
        }

        string ApiGetString(string url, out dynamic jsonObj, int tryCount = 0)
        {
            jsonObj = null;
            try
            {
                var json = _winXinService.GetWebString(GetApiUrl(url));
                var result = Json.Decode(json);
                if (result.errcode != null && result.errcode != 0)
                {
                    throw new WXResultException(result.errcode, result.errmsg);
                }
                return json;
            }
            catch (WXResultException ex)
            {
                if (ex.Code == -1 && tryCount < TryCount)//系统繁忙
                {
                    Thread.Sleep(SleepTime);
                    return ApiGetString(url, out jsonObj, tryCount++);
                }
                else if (ex.Code == 42001)
                {
                    RefreshToken();
                    return ApiGetString(url, out jsonObj);
                }
                _orchardServices.Notifier.Error(T("错误信息:[{0}]{1}.", ex.Code, ex.Message));
            }
            catch (Exception ex)
            {
                _orchardServices.Notifier.Error(T("错误信息:{0}.", ex.Message));
            }
            return null;
        }

        dynamic ApiPostString(string url, string postData, int tryCount = 0)
        {
            try
            {
                var json = _winXinService.PostWebString(GetApiUrl(url), postData);
                var result = Json.Decode(json);
                if (result.errcode != null && result.errcode != 0)
                {
                    throw new WXResultException(result.errcode, result.errmsg);
                }
                return result;
            }
            catch (WXResultException ex)
            {
                if (ex.Code == -1 && tryCount < TryCount)//系统繁忙
                {
                    Thread.Sleep(SleepTime);
                    return ApiPostString(url, postData, tryCount++);
                }
                else if (ex.Code == 42001)
                {
                    RefreshToken();
                    return ApiPostString(url, postData);
                }
                _orchardServices.Notifier.Error(T("错误信息:[{0}]{1}.", ex.Code, ex.Message));
            }
            catch (Exception ex)
            {
                _orchardServices.Notifier.Error(T("错误信息:{0}.", ex.Message));
            }
            return null;
        }

        #endregion
        public string MenuGet()
        {
            dynamic jsonObj = null;
            return ApiGetString(URL_MENUGET, out jsonObj);
        }

        public bool MenuDelete()
        {
            dynamic jsonObj = null;
            var result = ApiGetString(URL_MENUDELETE, out jsonObj);
            return result != null;
        }

        public bool MenuCreate(string json)
        {
            var result = ApiPostString(URL_MENUCREATE, json);
            return result != null;
        }

        public MediaModel UploadFile(MediaPart part, string type)
        {
            var url = GetApiUrl(URL_UPLOADFILE, type);
            WebClient myWebClient = new WebClient();
            myWebClient.Credentials = CredentialCache.DefaultCredentials;
            var filepath = HttpContext.Current.Server.MapPath(part.MediaUrl);
            try
            {
                byte[] responseArray = myWebClient.UploadFile(url, "POST", filepath);
                var result = System.Text.Encoding.Default.GetString(responseArray, 0, responseArray.Length);
                var model = System.Web.Helpers.Json.Decode<MediaModel>(result);
                return model;
            }
            catch { }
            return null;
        }
    }
}