using Juqian.Winxin.Models;
using Juqian.Winxin.Services;
using Orchard.Localization;
using Orchard.Workflows.Models;
using Orchard.Workflows.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;

namespace Juqian.Winxin.Activities
{
    public abstract class WinXinActivity : Event
    {
        public Localizer T { get; set; }

        public override bool CanStartWorkflow
        {
            get { return true; }
        }

        public override bool CanExecute(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            return true;
        }

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            return new[] { T("Done") };
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            yield return T("Done");
        }

        public override LocalizedString Category
        {
            get { return T("微信"); }
        }
    }

    public class WX_TextActivity : WinXinActivity
    {
        public override string Name
        {
            get { return "微信文本消息"; }
        }

        public override LocalizedString Description
        {
            get { return T("当微信用户向公众账号发文本消息时."); }
        }

        public override string Form
        {
            get
            {
                return "ActivityWeiXinText";
            }
        }
        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            return new[] { T("条件满足"), T("条件不满足") };
        }

        public override bool CanExecute(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            var part = workflowContext.Content.As<WXMsgPart>();
            return part.MsgType == "text";
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            var part = workflowContext.Content.As<WXMsgPart>();
            var content = part.Content;
            var op = activityContext.GetState<string>("operator");
            var value = activityContext.GetState<string>("textValue");
            switch (op)
            {
                case "Equals":
                    if (content == value)
                    {
                        yield return T("条件满足");
                        yield break;
                    }
                    break;
                case "NotEquals":
                    if (content != value)
                    {
                        yield return T("条件满足");
                        yield break;
                    }
                    break;
                case "Contains":
                    if (content.Contains(value))
                    {
                        yield return T("条件满足");
                        yield break;
                    }
                    break;
                case "NotContains":
                    if (!content.Contains(value))
                    {
                        yield return T("条件满足");
                        yield break;
                    }
                    break;
                case "Starts":
                    if (content.StartsWith(value))
                    {
                        yield return T("条件满足");
                        yield break;
                    }
                    break;
                case "NotStarts":
                    if (!content.StartsWith(value))
                    {
                        yield return T("条件满足");
                        yield break;
                    }
                    break;
                case "Ends":
                    if (content.EndsWith(value))
                    {
                        yield return T("条件满足");
                        yield break;
                    }
                    break;
                case "NotEnds":
                    if (!content.EndsWith(value))
                    {
                        yield return T("条件满足");
                        yield break;
                    }
                    break;
            }


            yield return T("条件不满足");
        }
    }
    public class WX_ImageActivity : WinXinActivity
    {
        public override string Name
        {
            get { return "微信图片消息"; }
        }

        public override LocalizedString Description
        {
            get { return T("当微信用户向公众账号发图片消息时."); }
        }
    }
    public class WX_VoiceActivity : WinXinActivity
    {
        public override string Name
        {
            get { return "微信语音消息"; }
        }

        public override LocalizedString Description
        {
            get { return T("当微信用户向公众账号发语音消息时."); }
        }
    }
    public class WX_VideoActivity : WinXinActivity
    {
        public override string Name
        {
            get { return "微信视频消息"; }
        }

        public override LocalizedString Description
        {
            get { return T("当微信用户向公众账号发视频消息时."); }
        }
    }

    public class WX_LinkActivity : WinXinActivity
    {
        public override string Name
        {
            get { return "微信链接消息"; }
        }

        public override LocalizedString Description
        {
            get { return T("当微信用户向公众账号发链接消息时."); }
        }
    }
    public class WX_EventActivity : WinXinActivity
    {
        readonly IWinXinService _winXinService;
        public WX_EventActivity(IWinXinService winXinService)
        {
            _winXinService = winXinService;
        }
        public override string Name
        {
            get { return "微信事件消息"; }
        }

        public override string Form
        {
            get
            {
                return "ActivityWeiXinEventKey";
            }
        }

        public override LocalizedString Description
        {
            get { return T("当微信用户向公众账号发事件消息时."); }
        }

        public override bool CanExecute(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            var part = workflowContext.Content.As<WXMsgPart>();
            if (part.MsgType != "event")
                return false;

            var type = activityContext.GetState<string>("EventType");
            if (type != part.Event)
                return false;

            var eventKey = activityContext.GetState<string>("EventKey");
            if (!string.IsNullOrWhiteSpace(eventKey) && eventKey != part.EventKey)
                return false;

            return true;
        }
    }

    public class WX_LocationEventActivity : WinXinActivity
    {
        readonly IWinXinService _winXinService;
        public WX_LocationEventActivity(IWinXinService winXinService)
        {
            _winXinService = winXinService;
        }
        public override string Name
        {
            get { return "微信上报地理位置事件"; }
        }
        public override string Form
        {
            get
            {
                return "ActivityWeiXinLocation";
            }
        }

        public override LocalizedString Description
        {
            get { return T("用户同意上报地理位置后，每次进入公众号会话时，都会在进入时上报地理位置，或在进入会话后每5秒上报一次地理位置."); }
        }

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            return new[] { T("在范围内"), T("在范围外"), T("缺省") };
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            double lat1 = 0, lng1 = 0, distance = 0, distance1 = 0;
            bool flag = true;
            var part = workflowContext.Content.As<WXMsgPart>();

            try
            {
                lat1 = activityContext.GetState<double>("lat1");
                lng1 = activityContext.GetState<double>("lng1");
                distance = activityContext.GetState<double>("distance");
                distance1 = _winXinService.GetDistance(lat1, lng1, part.Latitude, part.Longitude);
            }
            catch { flag = false; }

            if (lat1 + lat1 + distance == 0 || !flag)
            {
                yield return T("缺省");
                yield break;
            }

            if (distance1 <= distance)
            {
                yield return T("在范围内");
                yield break;
            }
            else
            {
                yield return T("在范围外");
                yield break;
            }
        }

        public override bool CanExecute(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            var part = workflowContext.Content.As<WXMsgPart>();
            return part.MsgType == "event" && part.Event == "LOCATION";
        }
    }

    public class WX_LocationActivity : WinXinActivity
    {
        readonly IWinXinService _winXinService;

        public WX_LocationActivity(IWinXinService winXinService)
        {
            _winXinService = winXinService;
        }

        public override string Name
        {
            get { return "微信地理位置消息"; }
        }

        public override string Form
        {
            get
            {
                return "ActivityWeiXinLocation";
            }
        }

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            return new[] { T("在范围内"), T("在范围外"), T("缺省") };
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            double lat1 = 0, lng1 = 0, distance = 0, distance1 = 0;
            bool flag = true;
            var part = workflowContext.Content.As<WXMsgPart>();

            try
            {
                lat1 = activityContext.GetState<double>("lat1");
                lng1 = activityContext.GetState<double>("lng1");
                distance = activityContext.GetState<double>("distance");
                distance1 = _winXinService.GetDistance(lat1, lng1, part.Location_X, part.Location_Y);
            }
            catch { flag = false; }

            if (lat1 + lat1 + distance == 0 || !flag)
            {
                yield return T("缺省");
                yield break;
            }

            if (distance1 <= distance)
            {
                yield return T("在范围内");
                yield break;
            }
            else
            {
                yield return T("在范围外");
                yield break;
            }
        }

        public override LocalizedString Description
        {
            get { return T("当微信用户向公众账号发地理位置消息时."); }
        }

        public override bool CanExecute(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            var part = workflowContext.Content.As<WXMsgPart>();
            return part.MsgType == "location";
        }
    }
}