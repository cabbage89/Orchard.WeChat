using Juqian.Winxin.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Juqian.Winxin.Drivers
{
    public class WinXinSettingsPartDriver : ContentPartDriver<WinXinSettingsPart>
    {
        private const string TemplateName = "Parts/WinXinSettings";
        public WinXinSettingsPartDriver()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override string Prefix { get { return "WinXinSettings"; } }

        protected override DriverResult Editor(WinXinSettingsPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_WinXinSettings_Edit",
                    () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix))
                    .OnGroup("WeiXin");
        }

        protected override DriverResult Editor(WinXinSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            return ContentShape("Parts_WinXinSettings_Edit", () =>
            {
                var previousSecret = part.Secret;
                updater.TryUpdateModel(part, Prefix, null, null);

                // restore password if the input is empty, meaning it has not been reseted
                if (string.IsNullOrEmpty(part.Secret))
                {
                    part.Secret = previousSecret;
                }
                return shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix);
            })
                .OnGroup("WeiXin");
        }
    }
}