using Juqian.Winxin.Controllers;
using Juqian.Winxin.Services;
using Orchard.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Juqian.Winxin.Filters
{
    public class WinXinReqAuth : FilterProvider, IActionFilter
    {
        private readonly IWinXinService _winXinService;

        public WinXinReqAuth(IWinXinService winXinService)
        {
            _winXinService = winXinService;
        }
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if ((filterContext.Controller as WinXinController) == null)
                return;

            var signature = filterContext.RequestContext.HttpContext.Request["signature"];
            var timestamp = filterContext.RequestContext.HttpContext.Request["timestamp"];
            var nonce = filterContext.RequestContext.HttpContext.Request["nonce"];

            if (!_winXinService.AuthReq(signature, timestamp, nonce))
                throw new Exception("非微信服务器请求");
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}