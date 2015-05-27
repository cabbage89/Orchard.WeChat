using Orchard;
using Orchard.ContentManagement;
using Orchard.Core.Navigation.Services;
using Orchard.Core.Navigation.ViewModels;
using Orchard.Localization;
using Orchard.UI.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orchard.UI.Notify;
using Juqian.Winxin.Services;
using Orchard.Core.Contents.Controllers;

namespace Juqian.Winxin.Controllers
{
    [ValidateInput(false), Admin]
    public class WeiXinAdminController : Controller, IUpdateModel
    {
        private readonly IMenuService _menuService;
        private readonly IWeiXinSdk _weiXinSdk;
        private readonly IWinXinService _winXinService;
        public WeiXinAdminController(IMenuService menuService
            , IOrchardServices services
            , IWinXinService winXinService
            , IWeiXinSdk weiXinSdk)
        {
            _menuService = menuService;
            Services = services;
            _weiXinSdk = weiXinSdk;
            _winXinService = winXinService;

            T = NullLocalizer.Instance;
        }
        public IOrchardServices Services { get; set; }
        public Localizer T { get; set; }
        public ActionResult SetMenu()
        {
            var model = new MenuWidgetViewModel
            {
                Menus = _menuService.GetMenus(),
            };
            return View(model);
        }
        [HttpPost, ActionName("SetMenu")]
        [FormValueRequired("submit.setmenu")]
        public ActionResult SetMenu_POST(MenuWidgetViewModel model)
        {
            var menu = _menuService.GetMenu(model.CurrentMenuId);
            var json = _winXinService.OrchardMenuToJson(menu);

            if (_weiXinSdk.MenuCreate(json))
                Services.Notifier.Information(T("微信菜单设置成功，由于微信客户端缓存，需要24小时微信客户端才会展现出来."));
            else
                Services.Notifier.Error(T("微信菜单设置失败."));
            return RedirectToAction("SetMenu");
        }

        [HttpPost, ActionName("SetMenu")]
        [FormValueRequired("submit.deletemenu")]
        public ActionResult DeleteMenu_POST() 
        {
            if (_weiXinSdk.MenuDelete())
                Services.Notifier.Information(T("菜单删除成功."));
            else
                Services.Notifier.Error(T("菜单删除失败."));
            return RedirectToAction("SetMenu");
        }

        [HttpPost, ActionName("SetMenu")]
        [FormValueRequired("submit.getmenu")]
        public ActionResult GetMenu_POST() 
        {
            string json = _weiXinSdk.MenuGet();
            if (json == null)
                Services.Notifier.Error(T("菜单获取失败."));
            else
            {
                _winXinService.JsonToOrchardMenu(string.Format("微信菜单[{0}]", DateTime.Now.ToLocalTime().ToString()), json);
                Services.Notifier.Information(T("菜单获取成功."));
            }
            return RedirectToAction("SetMenu");
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties)
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        public void AddModelError(string key, LocalizedString errorMessage)
        {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}