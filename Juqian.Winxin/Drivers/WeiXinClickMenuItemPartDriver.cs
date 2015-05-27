using System;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Core.Navigation.Models;
using Orchard.Localization;
using Orchard;
using Juqian.Winxin.Models;

namespace Juqian.Winxin.Drivers
{
    public class WeiXinClickMenuItemPartDriver : ContentPartDriver<WeiXinClickMenuItemPart>
    {
        private const string TemplateName = "Parts.WeiXinClickMenuItemPart.Edit";

        public WeiXinClickMenuItemPartDriver(IOrchardServices services)
        {
            T = NullLocalizer.Instance;
            Services = services;
        }

        public Localizer T { get; set; }
        public IOrchardServices Services { get; set; }

        protected override string Prefix { get { return "WeiXinClickMenuItemPart"; } }

        protected override DriverResult Editor(WeiXinClickMenuItemPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_WeiXinClickMenuItemPart_Edit", () =>
            {
                return shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix);
            });
        }

        protected override DriverResult Editor(WeiXinClickMenuItemPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            if (updater.TryUpdateModel(part, Prefix, null, null))
            {
                if (String.IsNullOrWhiteSpace(part.Key))
                {
                    updater.AddModelError("Key", T("事件Key是必须的."));
                }
            }

            return Editor(part, shapeHelper);
        }

        protected override void Importing(WeiXinClickMenuItemPart part, ImportContentContext context)
        {
            IfNotNull(context.Attribute(part.PartDefinition.Name, "Key"), x => part.Key = x);
        }

        private static void IfNotNull<T>(T value, Action<T> then) where T : class
        {
            if (value != null)
            {
                then(value);
            }
        }

        protected override void Exporting(WeiXinClickMenuItemPart part, ExportContentContext context)
        {
            context.Element(part.PartDefinition.Name).SetAttributeValue("Key", part.Key);
        }
    }
}
