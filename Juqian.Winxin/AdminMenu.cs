using Orchard;
using Orchard.Localization;
using Orchard.Security;
using Orchard.UI.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Juqian.Winxin
{
    public class AdminMenu : INavigationProvider
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IOrchardServices _orchardServices;

        public AdminMenu(IAuthorizationService authorizationService, IWorkContextAccessor workContextAccessor
            , IOrchardServices orchardServices)
        {
            _authorizationService = authorizationService;
            _workContextAccessor = workContextAccessor;
            _orchardServices = orchardServices;
        }

        public Localizer T { get; set; }
        public void GetNavigation(NavigationBuilder builder)
        {
            builder//.AddImageSet("weixin")
                .Add(T("微信设置"), "1.1", BuildMenu);
        }
        private void BuildMenu(NavigationItemBuilder menu)
        {
            menu.Add(T("账号设置"), "1.1",
                     item => item.Action("WeiXin", "Admin", new { Area = "Settings" }).Permission(Permissions.ManageWeiXin));
            menu.Add(T("菜单设置"), "1.2",
                     item => item.Action("SetMenu", "WeiXinAdmin", new { Area = "Juqian.Winxin" }).Permission(Permissions.ManageWeiXin));
        }
        public string MenuName { get { return "admin"; } }
    }
}