using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Juqian.Winxin.Models
{
    public class WXReqModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            StreamReader sr = new StreamReader(HttpContext.Current.Request.InputStream);
            var postStr = sr.ReadToEnd();
            HttpContext.Current.Request.InputStream.Seek(0, SeekOrigin.Begin);

            if (!string.IsNullOrEmpty(postStr))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(postStr);

                XmlElement rootElement = doc.DocumentElement;

                WXReqModel model = new WXReqModel();
                model.XML = postStr;
                model.ToUserName = rootElement.SelectSingleNode("ToUserName").InnerText;
                model.FromUserName = rootElement.SelectSingleNode("FromUserName").InnerText;
                model.CreateTime = rootElement.SelectSingleNode("CreateTime").InnerText;
                model.MsgType = rootElement.SelectSingleNode("MsgType").InnerText;

                switch (model.MsgType)
                {
                    case "text":
                        model.Content = rootElement.SelectSingleNode("Content").InnerText;
                        break;
                    case "link":
                        model.Title = rootElement.SelectSingleNode("Title").InnerText;
                        model.Description = rootElement.SelectSingleNode("Description").InnerText;
                        model.Url = rootElement.SelectSingleNode("Url").InnerText;
                        break;
                    case "location":
                        double Location_X, Location_Y; int Scale;
                        if (double.TryParse(rootElement.SelectSingleNode("Location_X").InnerText, out Location_X))
                            model.Location_X = Location_X;
                        if (double.TryParse(rootElement.SelectSingleNode("Location_Y").InnerText, out Location_Y))
                            model.Location_Y = Location_Y;
                        if (int.TryParse(rootElement.SelectSingleNode("Scale").InnerText, out Scale))
                            model.Scale = Scale;

                        model.Label = rootElement.SelectSingleNode("Label").InnerText;
                        break;
                    case "image":
                        model.PicUrl = rootElement.SelectSingleNode("PicUrl").InnerText;
                        model.MediaId = rootElement.SelectSingleNode("MediaId").InnerText;
                        break;
                    case "voice":
                        model.PicUrl = rootElement.SelectSingleNode("Format").InnerText;
                        model.MediaId = rootElement.SelectSingleNode("MediaId").InnerText;
                        try { model.Recongnition = rootElement.SelectSingleNode("Recongnition").InnerText; }
                        catch { }
                        break;
                    case "video":
                        model.PicUrl = rootElement.SelectSingleNode("ThumbMediaId").InnerText;
                        model.MediaId = rootElement.SelectSingleNode("MediaId").InnerText;
                        break;
                    case "event":
                        model.EventKey = rootElement.SelectSingleNode("EventKey").InnerText;//用于自定义菜单
                        model.Event = rootElement.SelectSingleNode("Event").InnerText;
                        switch (model.Event)
                        {
                            case "LOCATION":
                                double lat, lng, pre;
                                double.TryParse(rootElement.SelectSingleNode("Latitude").InnerText, out lat);
                                double.TryParse(rootElement.SelectSingleNode("Longitude").InnerText, out lng);
                                double.TryParse(rootElement.SelectSingleNode("Precision").InnerText, out pre);
                                model.Latitude = lat;
                                model.Longitude = lng;
                                model.Precision = pre;
                                break;
                            case "SCAN":
                            case "subscribe":
                                model.Ticket = rootElement.SelectSingleNode("Ticket").InnerText;
                                break;
                        }
                        break;
                }
                try
                {
                    model.MsgId = int.Parse(rootElement.SelectSingleNode("MsgId").InnerText);
                }
                catch { }

                return model;
            }
            return null;
        }
    }
}