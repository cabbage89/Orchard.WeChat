using Juqian.Winxin.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Localization;
using Orchard.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Juqian.Winxin.Handlers
{
    public class WinXinSettingsPartHandler : ContentHandler
    {
        public Localizer T { get; set; }
        public new ILogger Logger { get; set; }
        public WinXinSettingsPartHandler()
        {
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;

            Filters.Add(new ActivatingFilter<WinXinSettingsPart>("Site"));
            OnInitializing<WinXinSettingsPart>((context, part) => { });
        }

        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("WeiXin")));
        }
    }
}