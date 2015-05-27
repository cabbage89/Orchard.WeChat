using Juqian.Winxin.Models;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Juqian.Winxin.Drivers
{
    public class WXMsgPartDriver : ContentPartDriver<WXMsgPart>
    {
        //必须重写这两个方法,_contentManager.New("类型")之后,此part才能As出来.
        protected override void Exporting(WXMsgPart part, ExportContentContext context)
        {
            context.Element(part.PartDefinition.Name).SetAttributeValue("ToUserName ", part.ToUserName);
            context.Element(part.PartDefinition.Name).SetAttributeValue("FromUserName ", part.FromUserName);
            context.Element(part.PartDefinition.Name).SetAttributeValue("CreateTime ", part.CreateTime);
            context.Element(part.PartDefinition.Name).SetAttributeValue("MsgType ", part.MsgType);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Content ", part.Content);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Location_X ", part.Location_X);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Location_Y ", part.Location_Y);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Scale ", part.Scale);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Label ", part.Label);
            context.Element(part.PartDefinition.Name).SetAttributeValue("PicUrl ", part.PicUrl);
            context.Element(part.PartDefinition.Name).SetAttributeValue("EventKey ", part.EventKey);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Event ", part.Event);

            context.Element(part.PartDefinition.Name).SetAttributeValue("MsgId ", part.MsgId);
            context.Element(part.PartDefinition.Name).SetAttributeValue("MediaId ", part.MediaId);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Format ", part.Format);
            context.Element(part.PartDefinition.Name).SetAttributeValue("ThumbMediaId ", part.ThumbMediaId);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Title ", part.Title);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Description ", part.Description);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Url ", part.Url);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Ticket ", part.Ticket);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Latitude ", part.Latitude);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Longitude ", part.Longitude);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Precision ", part.Precision);
            context.Element(part.PartDefinition.Name).SetAttributeValue("Recongnition ", part.Recongnition);
        }

        protected override void Importing(WXMsgPart part, ImportContentContext context)
        {
            part.ToUserName = context.Attribute(part.PartDefinition.Name, "ToUserName");
            part.FromUserName = context.Attribute(part.PartDefinition.Name, "FromUserName");
            part.CreateTime = context.Attribute(part.PartDefinition.Name, "CreateTime");
            part.MsgType = context.Attribute(part.PartDefinition.Name, "MsgType");
            part.Content = context.Attribute(part.PartDefinition.Name, "Content");

            double Location_X, Location_Y, Latitude, Longitude, Precision;
            int Scale;

            if (double.TryParse(context.Attribute(part.PartDefinition.Name, "Location_X"), out Location_X))
                part.Location_X = Location_X;
            if (double.TryParse(context.Attribute(part.PartDefinition.Name, "Location_Y"), out Location_Y))
                part.Location_Y = Location_Y;
            if (int.TryParse(context.Attribute(part.PartDefinition.Name, "Scale"), out Scale))
                part.Scale = Scale;

            part.Label = context.Attribute(part.PartDefinition.Name, "Label");
            part.PicUrl = context.Attribute(part.PartDefinition.Name, "PicUrl");
            part.EventKey = context.Attribute(part.PartDefinition.Name, "EventKey");
            part.Event = context.Attribute(part.PartDefinition.Name, "Event");

            part.MsgId = int.Parse(context.Attribute(part.PartDefinition.Name, "MsgId"));
            part.MediaId = context.Attribute(part.PartDefinition.Name, "MediaId");
            part.Format = context.Attribute(part.PartDefinition.Name, "Format");
            part.ThumbMediaId = context.Attribute(part.PartDefinition.Name, "ThumbMediaId");
            part.Title = context.Attribute(part.PartDefinition.Name, "Title");
            part.Description = context.Attribute(part.PartDefinition.Name, "Description");
            part.Url = context.Attribute(part.PartDefinition.Name, "Url");
            part.Ticket = context.Attribute(part.PartDefinition.Name, "Ticket");

            if (double.TryParse(context.Attribute(part.PartDefinition.Name, "Latitude"), out Latitude))
                part.Latitude = Latitude;
            if (double.TryParse(context.Attribute(part.PartDefinition.Name, "Longitude"), out Longitude))
                part.Longitude = Longitude;
            if (double.TryParse(context.Attribute(part.PartDefinition.Name, "Precision"), out Precision))
                part.Precision = Precision;

            part.Recongnition = context.Attribute(part.PartDefinition.Name, "Recongnition");
        }
    }
}