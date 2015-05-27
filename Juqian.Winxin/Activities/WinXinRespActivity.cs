using Juqian.Winxin.Models;
using Juqian.Winxin.Services;
using Orchard;
using Orchard.Localization;
using Orchard.Workflows.Models;
using Orchard.Workflows.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.MediaLibrary.Models;

namespace Juqian.Winxin.Activities
{
    public class WinXinRespActivity : Task
    {
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IWeiXinResp _weiXinResp;
        private readonly IContentManager _contentManager;
        private readonly IWeiXinSdk _weiXinSdk;
        private readonly IWinXinService _winXinService;
        public WinXinRespActivity(IWorkContextAccessor workContextAccessor
            , IWeiXinResp weiXinResp
            , IWeiXinSdk weiXinSdk
            , IWinXinService winXinService
            , IContentManager contentManager)
        {
            _workContextAccessor = workContextAccessor;
            _weiXinResp = weiXinResp;
            _contentManager = contentManager;
            _weiXinSdk = weiXinSdk;
            _winXinService = winXinService;
        }

        public Localizer T { get; set; }

        public override string Name
        {
            get { return "微信响应消息"; }
        }

        public override LocalizedString Category
        {
            get { return T("微信"); }
        }

        public override LocalizedString Description
        {
            get { return T("向微信用户响应消息,如文本,图片,语音,视频等"); }
        }

        public override string Form
        {
            get { return "ActivityWinXinResp"; }
        }

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            yield return T("Done");
        }

        void Process(Func<dynamic> mediaIdFiled, Func<dynamic> part, Func<dynamic> createAtFiled
            , Action<string> setMediaId, Action<int> setCreateAt, string type)
        {
            string mediaId1 = mediaIdFiled();
            int create1 = 0;
            int.TryParse(createAtFiled(), out create1);
            if (string.IsNullOrWhiteSpace(mediaId1) || create1 == 0
                        || _winXinService.ConvertWXDateTimeInt(DateTime.Now) - create1 >= 3600 * 24 * 3)
            {
                var model1 = _weiXinSdk.UploadFile(part(), type);
                if (model1 != null)
                {
                    //将上传后的媒体项保存到内容中.
                    setMediaId(model1.media_id);
                    setCreateAt(model1.created_at);
                }
            }
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            var part = workflowContext.Content.As<WXMsgPart>();
            var contentId = activityContext.GetState<int>("contentId");
            var contentItem = _contentManager.Get(contentId);

            switch (contentItem.TypeDefinition.Name)
            {
                case "WXImageMsg":
                    Process(() => ((dynamic)contentItem).WXImageMsg.WXMsgImageMediaIdField.Value
                        , () => ((dynamic)contentItem).WXImageMsg.WXMsgImageField.MediaParts[0]
                        , () => ((dynamic)contentItem).WXImageMsg.WXMsgImageMediaCreateAtField.Value
                        , (val) => ((dynamic)contentItem).WXImageMsg.WXMsgImageMediaIdField.Value = val
                        , (val) => ((dynamic)contentItem).WXImageMsg.WXMsgImageMediaCreateAtField.Value = val.ToString()
                        , "image");
                    break;
                case "WXMusicMsg":
                    Process(() => ((dynamic)contentItem).WXMusicMsg.WXMsgMusicThumbMediaIdField.Value
                        , () => ((dynamic)contentItem).WXMusicMsg.WXMsgMusicThumbField.MediaParts[0]
                        , () => ((dynamic)contentItem).WXMusicMsg.WXMsgMusicThumbMediaCreateAtField.Value
                        , (val) => ((dynamic)contentItem).WXMusicMsg.WXMsgMusicThumbMediaIdField.Value = val
                        , (val) => ((dynamic)contentItem).WXMusicMsg.WXMsgMusicThumbMediaCreateAtField.Value = val.ToString()
                        , "thumb");
                    break;
                case "WXVideoMsg":
                    Process(() => ((dynamic)contentItem).WXVideoMsg.WXMsgVideoMediaIdField.Value
                        , () => ((dynamic)contentItem).WXVideoMsg.WXMsgVideoField.MediaParts[0]
                        , () => ((dynamic)contentItem).WXVideoMsg.WXMsgVideoMediaCreateAtField.Value
                        , (val) => ((dynamic)contentItem).WXVideoMsg.WXMsgVideoMediaIdField.Value = val
                        , (val) => ((dynamic)contentItem).WXVideoMsg.WXMsgVideoMediaCreateAtField.Value = val.ToString()
                        , "video");
                    break;
                case "WXVoiceMsg":
                    Process(() => ((dynamic)contentItem).WXVoiceMsg.WXMsgVoiceMediaIdField.Value
                        , () => ((dynamic)contentItem).WXVoiceMsg.WXMsgVoiceField.MediaParts[0]
                        , () => ((dynamic)contentItem).WXVoiceMsg.WXMsgVoiceMediaCreateAtField.Value
                        , (val) => ((dynamic)contentItem).WXVoiceMsg.WXMsgVoiceMediaIdField.Value = val
                        , (val) => ((dynamic)contentItem).WXVoiceMsg.WXMsgVoiceMediaCreateAtField.Value = val.ToString()
                        , "voice");
                    break;
            }

            _weiXinResp.Add(new Tuple<ContentItem, string>(contentItem, part.FromUserName));

            yield return T("Done");
        }
    }
}