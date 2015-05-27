using Juqian.Winxin.Models;
using Juqian.Winxin.Services;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Workflows.Models;
using Orchard.Workflows.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;

namespace Juqian.Winxin.Activities
{
    public class WeiXinDispatchActivity : Task
    {
        private readonly IWinXinService _winXinService;
        public WeiXinDispatchActivity(IWinXinService winXinService)
        {
            _winXinService = winXinService;
        }

        public ILogger Logger { get; set; }
        public Localizer T { get; set; }

        public override string Name
        {
            get { return "微信接口转发"; }
        }

        public override LocalizedString Category
        {
            get { return T("微信"); }
        }

        public override LocalizedString Description
        {
            get { return T("将当前微信请求转发至其它微信接口处理响应."); }
        }

        public override string Form
        {
            get { return "ActivityWeiXinDispatch"; }
        }
        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            yield return T("Error");
            yield return T("Success");
        }
        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            var part = workflowContext.Content.As<WXMsgPart>();
            var apiUrl = activityContext.GetState<string>("api_url");
            var apiToken = activityContext.GetState<string>("api_token");
            var timestamp = HttpContext.Current.Request.QueryString["timestamp"];
            var nonce = HttpContext.Current.Request.QueryString["nonce"];

            string[] arr = { apiToken, timestamp, nonce };
            Array.Sort(arr);     //字典排序
            string tmpStr = string.Join("", arr);
            var signature = _winXinService.GetSHA1(tmpStr);
            signature = signature.ToLower();

            var client = new System.Net.WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            var url = string.Format("{0}?timestamp={1}&nonce={2}&signature={3}"
                    , apiUrl, timestamp, nonce, signature);
            string postData = part.XML;
            //using (var stream = HttpContext.Current.Request.InputStream)
            //{
            //    var reader = new StreamReader(stream);
            //    postData = reader.ReadToEnd();
            //}

            string result = null;
            try
            {
                result = client.UploadString(url, postData);
            }
            catch (System.Net.WebException ex)
            {
                string msg = null;
                using (var stream = ex.Response.GetResponseStream())
                {
                    var reader = new StreamReader(stream);
                    msg = reader.ReadToEnd();
                }
                Logger.Warning(ex, ex.Message);
            }
            catch (Exception ex)
            {
                var innerEx = ex;
                while (innerEx.InnerException != null)
                    innerEx = innerEx.InnerException;
                Logger.Warning(ex, innerEx.Message);
            }

            if (result == null)
            {
                yield return T("Error");
            }
            else
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(result);
                HttpContext.Current.Response.End();
                yield return T("Success");
            }
        }
    }
}