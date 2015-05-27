using Juqian.Winxin.Filters;
using Juqian.Winxin.Models;
using Juqian.Winxin.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Security;
using Orchard.Settings;
using Orchard.Themes;
using Orchard.Workflows.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Juqian.Winxin.Controllers
{
    //请注意,此控制器是返回给微信服务器的XML信息包,因此不得应用[Themed]特性,应纯输出!.
    public class WinXinController : Controller
    {
        public Localizer T { get; set; }

        private readonly IOrchardServices _orchardServices;
        private readonly IWorkflowManager _workflowManager;
        private readonly IWinXinService _winXinService;
        private readonly IContentManager _contentManager;
        private readonly ISiteService _siteService;
        private readonly IMembershipService _membershipService;
        private readonly IWeiXinResp _weiXinResp;

        public string Owner { get; set; }
        dynamic Shape { get; set; }
        public WinXinController(IWorkflowManager workflowManager
            , IOrchardServices orchardServices
            , IWinXinService winXinService
            , IContentManager contentManager
            , ISiteService siteService
            , IMembershipService membershipService
            , IShapeFactory shapeFactory
            , IWeiXinResp weiXinResp
            )
        {
            _workflowManager = workflowManager;
            _membershipService = membershipService;
            _contentManager = contentManager;
            _orchardServices = orchardServices;
            _siteService = siteService;
            _winXinService = winXinService;
            Shape = shapeFactory;
            _weiXinResp = weiXinResp;
        }

        public ActionResult Index(string signature, string timestamp, string nonce, string echostr)
        {
            if (_winXinService.AuthReq(signature, timestamp, nonce))
                return Content(echostr);
            else
                throw new Exception("非微信服务器请求");
        }

        [HttpPost]
        public ActionResult Index(WXReqModel model)
        {
            var workContext = _orchardServices.WorkContext;
            var wxSettings = workContext.CurrentSite.As<WinXinSettingsPart>();

            if (model.ToUserName != wxSettings.UserName)
                return Content(string.Format(@"<xml>
<ToUserName><![CDATA[{0}]]></ToUserName>
<FromUserName><![CDATA[{1}]]></FromUserName>
<CreateTime>{2}</CreateTime>
<MsgType><![CDATA[text]]></MsgType>
<Content><![CDATA[{3}]]></Content>
</xml>
", model.FromUserName, model.ToUserName, _winXinService.ConvertWXDateTimeInt(DateTime.Now), T("微信平台对接错误,本平台不为此服务号或订阅号提供服务!")));

            string name = null;
            switch (model.MsgType)
            {
                case "text":
                    name = "微信文本消息";
                    break;
                case "image":
                    name = "微信图片消息";
                    break;
                case "voice":
                    name = "微信语音消息";
                    break;
                case "video":
                    name = "微信视频消息";
                    break;
                case "location":
                    name = "微信地理位置消息";
                    break;
                case "link":
                    name = "微信链接消息";
                    break;
                case "event":
                    name = "微信事件消息";
                    if (model.Event == "LOCATION")
                        name = "微信上报地理位置事件";
                    break;
                default:
                    throw new OrchardException(T(string.Format("未知的消息类型:{0}", model.MsgType)));
            }
            if (String.IsNullOrEmpty(Owner))
            {
                Owner = _siteService.GetSiteSettings().SuperUser;
            }
            var owner = _membershipService.GetUser(Owner);
            var msg = _contentManager.New("WeiXinMsg");
            msg.As<ICommonPart>().Owner = owner;
            var part = msg.As<WXMsgPart>();

            part.ToUserName = model.ToUserName;
            part.FromUserName = model.FromUserName;
            part.CreateTime = model.CreateTime;
            part.MsgType = model.MsgType;
            part.Content = model.Content;
            part.Location_X = model.Location_X;
            part.Location_Y = model.Location_Y;
            part.Scale = model.Scale;
            part.Label = model.Label;
            part.PicUrl = model.PicUrl;
            part.EventKey = model.EventKey;
            part.Event = model.Event;

            part.MsgId = model.MsgId;
            part.MediaId = model.MediaId;
            part.Format = model.Format;
            part.ThumbMediaId = model.ThumbMediaId;
            part.Title = model.Title;
            part.Description = model.Description;
            part.Url = model.Url;
            part.Ticket = model.Ticket;
            part.Latitude = model.Latitude;
            part.Longitude = model.Longitude;
            part.Precision = model.Precision;
            part.Recongnition = model.Recongnition;
            part.XML = model.XML;

            _contentManager.Create(msg);

            _workflowManager.TriggerEvent(name, msg, () => new Dictionary<string, object> { { "Content", msg } });

            var tupleList = _weiXinResp.List();
            var list = Shape.List(Attributes: new Dictionary<string, object> { 
                { "ToUserName", model.FromUserName } ,{ "FromUserName",model.ToUserName}
            });
            list.AddRange(tupleList.Select(t => _contentManager.BuildDisplay(t.Item1, "Detail")));

            var viewModel = Shape.ViewModel()
                .ContentItems(list);

            return View(viewModel);
        }
    }
}