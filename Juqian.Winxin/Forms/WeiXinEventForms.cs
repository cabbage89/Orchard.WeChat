using Orchard.DisplayManagement;
using Orchard.Forms.Services;
using Orchard.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Juqian.Winxin.Forms
{
    public class WeiXinEventForms : IFormProvider
    {
        protected dynamic Shape { get; set; }
        public Localizer T { get; set; }

        public WeiXinEventForms(IShapeFactory shapeFactory)
        {
            Shape = shapeFactory;
            T = NullLocalizer.Instance;
        }
        public void Describe(DescribeContext context)
        {
            context.Form("ActivityWeiXinEventKey", shape => Shape.Form(
                Id: "ActivityWeiXinEventKey",
                _Type: Shape.SelectList(
                    Id: "EventType", Name: "EventType",
                    Title: T("用户消息的事件类型"),
                    Description: T("请选择一个消息事件类型."))
                    .Add(new SelectListItem { Value = "VIEW", Text = T("菜单跳转").Text })
                    .Add(new SelectListItem { Value = "CLICK", Text = T("菜单点击").Text })
                    .Add(new SelectListItem { Value = "SCAN", Text = T("扫描二维码[用户已经关注]").Text })
                    .Add(new SelectListItem { Value = "subscribe", Text = T("扫描二维码[用户首次关注]").Text })
                    .Add(new SelectListItem { Value = "unsubscribe", Text = T("用户取消关注").Text }),

                _EventKey: Shape.Textbox(
                Id: "eventKey", Name: "EventKey",
                Title: T("EventKey"),
                Description: T(@"菜单跳转:跳转的Url<br/>
菜单点击:自定义的EventKey<br/>
首次关注:qrscene_二维码参数值<br/>
已关注:二维码scene_id<br/>
取消关注:忽略此值.<br/>
如果不填,则不对此类消息进行过滤!"),
                Classes: new[] { "medium" })
                ));

            context.Form("ActivityWeiXinLocation", shape => Shape.Form(
                Id: "ActivityWeiXinLocation",
                _lat1: Shape.Textbox(
                Id: "lat1", Name: "lat1",
                Title: T("参照点位置的纬度"),
                Description: T("参照点位置的纬度,可以用<a href='http://api.map.baidu.com/lbsapi/getpoint/index.html' target='_blank'>百度地图工具</a>拾取经纬度."),
                Classes: new[] { "medium" }),

                _lng1: Shape.Textbox(
                Id: "lng1", Name: "lng1",
                Title: T("参照点位置的经度"),
                Description: T("参照点位置的经度"),
                Classes: new[] { "medium" }),

                _Distance: Shape.Textbox(
                Id: "distance", Name: "distance",
                Title: T("用户距离参照点的距离"),
                Description: T("用户距离参照点的距离,单位公里"),
                Classes: new[] { "medium" })
                ));

            context.Form("ActivityWeiXinText", shape => Shape.Form(
                Id: "ActivityWeiXinText",
                _Operator: Shape.SelectList(
                    Id: "operator", Name: "operator",
                    Title: T("运算符"),
                    Description: T("请选择一个运算符测试用户发送的文本消息."))
                    .Add(new SelectListItem { Value = "Equals", Text = T("等于").ToString() })
                    .Add(new SelectListItem { Value = "NotEquals", Text = T("不等于").ToString() })
                    .Add(new SelectListItem { Value = "Contains", Text = T("包含").ToString() })
                    .Add(new SelectListItem { Value = "NotContains", Text = T("不包含").ToString() })
                    .Add(new SelectListItem { Value = "Starts", Text = T("开始于").ToString() })
                    .Add(new SelectListItem { Value = "NotStarts", Text = T("不开始于").ToString() })
                    .Add(new SelectListItem { Value = "Ends", Text = T("结束于").ToString() })
                    .Add(new SelectListItem { Value = "NotEnds", Text = T("不结束于").ToString() }),
                _Text: Shape.TextArea(
                        Id: "textValue", Name: "textValue",
                        Title: T("值"),
                        Description: T("用来测试的字符串值."),
                        Classes: new[] { "large", "text", "tokenized" }
                        )));
        }
    }
}