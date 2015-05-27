using Orchard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Juqian.Winxin.Models;
using System.Net;
using System.Text;
using System.IO;
using Orchard.UI.Navigation;
using Orchard.Core.Navigation.Services;
using Orchard.Core.Navigation.Models;
using Orchard.ContentManagement.Aspects;
using Orchard.MediaLibrary.Models;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace Juqian.Winxin.Services
{
    public interface IWinXinService : IDependency
    {
        /// <summary>
        /// 验证微信服务器请求
        /// </summary>
        bool AuthReq(string signature, string timestamp, string nonce);
        string GetSHA1(string str);
        string GetWebString(string url);
        string PostWebString(string url, string postData);
        string OrchardMenuToJson(IContent content);

        void JsonToOrchardMenu(string menuName, string json);

        double GetDistance(double lat1, double lng1, double lat2, double lng2);

        int ConvertWXDateTimeInt(System.DateTime time);

        string GetMediaAbsoluteUrl(MediaPart part);
    }
    public class WinXinService : IWinXinService
    {
        private readonly IOrchardServices _orchardServices;
        private readonly INavigationManager _navigationManager;
        private readonly IMenuService _menuService;
        private readonly IContentManager _contentManager;
        public WinXinService(IOrchardServices orchardServices
            , IMenuService menuService
            , IContentManager contentManager
            , INavigationManager navigationManager)
        {
            _orchardServices = orchardServices;
            _contentManager = contentManager;
            _navigationManager = navigationManager;
            _menuService = menuService;
        }
        public bool AuthReq(string signature, string timestamp, string nonce)
        {
            var wxSettingsPart = _orchardServices.WorkContext.CurrentSite.As<WinXinSettingsPart>();
            string[] ArrTmp = { wxSettingsPart.ValidToken, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = GetSHA1(tmpStr);
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string GetSHA1(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentNullException(str);

            string rethash = null;

            System.Security.Cryptography.SHA1 hash = System.Security.Cryptography.SHA1.Create();
            System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
            byte[] combined = encoder.GetBytes(str);
            var dataHashed = hash.ComputeHash(combined);
            rethash = BitConverter.ToString(dataHashed).Replace("-", "");

            return rethash;
        }
        public string GetWebString(string url)
        {
            var client = new WebClient();
            client.Encoding = Encoding.UTF8;
            return client.DownloadString(url);
        }
        public string PostWebString(string url, string postData)
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            //此处必须将Unicode字符全部替换,否则微信服务器报错.
            return client.UploadString(url, Regex.Unescape(postData));
        }
        public string OrchardMenuToJson(IContent content)
        {
            var menuItems = _navigationManager.BuildMenu(content);

            var menuModel = new WeiXinMenuModel();
            //var list1 = new List<string>();
            foreach (var menuItem in menuItems)
            {
                var item = new WeiXinMenuItemModel(menuItem.Text.Text);
                var clickPart = menuItem.Content.As<WeiXinClickMenuItemPart>();
                
                //var list2 = new List<string>();

                if (clickPart != null)
                {
                    //事件菜单
                    item.type = "click";
                    item.key = clickPart.Key;
                    //list1.Add("{\"name\":\"" + menuItem.Text.Text + "\",\"type\":\"click\",\"key\":\"" + clickPart.Key + "\"}");
                }
                else if (menuItem.Items.Count() > 0)
                {
                    item.type = "";
                    foreach (var subItem in menuItem.Items)
                    {
                        var subItem1 = new WeiXinMenuItemModel(subItem.Text.Text);
                        var clickPart1 = subItem.Content.As<WeiXinClickMenuItemPart>();
                        if (clickPart1 != null)
                        {
                            //事件菜单
                            subItem1.type = "click";
                            subItem1.key = clickPart1.Key;
                            //list2.Add("{\"name\":\"" + subItem.Text.Text + "\",\"type\":\"click\",\"key\":\"" + clickPart1.Key + "\"}");
                        }
                        else
                        {
                            subItem1.type = "view";
                            subItem1.url = subItem.Url;
                            //list2.Add("{\"name\":\"" + subItem.Text.Text + "\",\"type\":\"view\",\"url\":\"" + subItem.Url + "\"}");
                        }
                        item.sub_button.Add(subItem1);
                    }
                    //if (list2.Count > 0)
                    //    list1.Add("{\"name\":\"" + menuItem.Text.Text + "\",\"sub_button\":[" + string.Join(",", list2) + "]}");
                    //else
                    //    list1.Add("{\"name\":\"" + menuItem.Text.Text + "\"}");
                }
                else
                {
                    item.type = "view";
                    item.url = menuItem.Url;
                    //list1.Add("{\"name\":\"" + menuItem.Text.Text + "\",\"type\":\"view\",\"url\":\"" + menuItem.Url + "\"}");
                }
                menuModel.button.Add(item);
            }

            //return "{\"button\":[" + string.Join(",", list1) + "]}";

            var menuJson = System.Web.Helpers.Json.Encode(menuModel);
            return menuJson;
        }
        public void JsonToOrchardMenu(string menuName, string json)
        {
            var respMenu = System.Web.Helpers.Json.Decode<WeiXinRespMenuModel>(json);
            var model = respMenu.menu;
            var menu = _menuService.Create(menuName);
            var i = 1;
            var user = _orchardServices.WorkContext.CurrentUser;
            foreach (var item in model.button)
            {
                int j = 1;
                ContentItem menuItem = null, menuItem1;
                if (item.sub_button.Count() > 0)
                {
                    //创建链接型菜单
                    menuItem = _contentManager.Create("MenuItem");
                    var menuPart = menuItem.As<MenuPart>();
                    menuPart.MenuText = item.name;
                    menuPart.Menu = menu;
                    menuPart.MenuPosition = i.ToString();
                    menuItem.As<ICommonPart>().Owner = user;
                    foreach (var subItem in item.sub_button)
                    {
                        if (subItem.type == "view")
                        {
                            //创建链接型菜单
                            menuItem1 = _contentManager.Create("MenuItem");
                            var menuPart1 = menuItem1.As<MenuPart>();
                            menuPart1.MenuText = subItem.name;
                            menuPart1.Menu = menu;
                            menuItem1.As<MenuItemPart>().Url = subItem.url;
                            menuPart1.MenuPosition = string.Format("{0}.{1}", i, j);
                            menuItem1.As<ICommonPart>().Owner = user;
                        }
                        else if (subItem.type == "click")
                        {
                            menuItem1 = _contentManager.Create("WXClickMenuItem");
                            var menuPart1 = menuItem1.As<MenuPart>();
                            menuPart1.MenuText = subItem.name;
                            menuPart1.Menu = menu;
                            menuItem1.As<WeiXinClickMenuItemPart>().Key = subItem.key;
                            menuPart1.MenuPosition = string.Format("{0}.{1}", i, j);
                            menuItem1.As<ICommonPart>().Owner = user;
                        }
                    }
                }
                else if (item.type == "view")
                {
                    //创建链接型菜单
                    menuItem = _contentManager.Create("MenuItem");
                    var menuPart = menuItem.As<MenuPart>();
                    menuPart.MenuText = item.name;
                    menuPart.Menu = menu;
                    menuItem.As<MenuItemPart>().Url = item.url;
                    menuPart.MenuPosition = i.ToString();
                    menuItem.As<ICommonPart>().Owner = user;
                }
                else if (item.type == "click")
                {
                    menuItem = _contentManager.Create("WXClickMenuItem");
                    var menuPart = menuItem.As<MenuPart>();
                    menuPart.MenuText = item.name;
                    menuPart.Menu = menu;
                    menuItem.As<WeiXinClickMenuItemPart>().Key = item.key;
                    menuPart.MenuPosition = i.ToString();
                    menuItem.As<ICommonPart>().Owner = user;
                }
                i++;
            }
        }

        double Rad(double d)
        {
            return d * Math.PI / 180.0;
        }
        public double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double d_EarthRadius = 6378.137;
            double radLat1 = Rad(lat1);
            double radLat2 = Rad(lat2);
            double radLat = Rad(lat1) - Rad(lat2);
            double radLng = Rad(lng1) - Rad(lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(radLat / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(radLng / 2), 2)));
            s = s * d_EarthRadius;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }

        public int ConvertWXDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        public string GetMediaAbsoluteUrl(MediaPart part)
        {
            return _orchardServices.WorkContext.CurrentSite.BaseUrl + part.MediaUrl;
        }
    }
}