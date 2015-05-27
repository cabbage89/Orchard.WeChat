using Juqian.Winxin.Models;
using Orchard;
using Orchard.Localization;
using Orchard.UI.Admin.Notification;
using Orchard.UI.Notify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using System.Web.Mvc;

namespace Juqian.Winxin.Services
{
    public class MissingSettingsBanner : INotificationProvider
    {
        private readonly IOrchardServices _orchardServices;

        public MissingSettingsBanner(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public IEnumerable<NotifyEntry> GetNotifications()
        {
            var workContext = _orchardServices.WorkContext;
            var wxSettings = workContext.CurrentSite.As<WinXinSettingsPart>();

            if (string.IsNullOrWhiteSpace(wxSettings.AppId) ||
                string.IsNullOrWhiteSpace(wxSettings.Secret) ||
                string.IsNullOrWhiteSpace(wxSettings.ValidToken) ||
                string.IsNullOrWhiteSpace(wxSettings.UserName))
            {
                var urlHelper = new UrlHelper(workContext.HttpContext.Request.RequestContext);
                var url = urlHelper.Action("WeiXin", "Admin", new { Area = "Settings" });
                yield return new NotifyEntry { Message = T("The <a href=\"{0}\">微信设置</a> 配置不完整.", url), Type = NotifyType.Warning };
            }
        }
    }
}