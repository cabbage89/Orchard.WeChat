using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Juqian.Winxin.Models
{
    public class WXMsgPart : ContentPart<WXMsgPartRecord>
    {
        /// <summary>
        /// 消息接收方微信号，一般为公众平台账号微信号
        /// </summary>
        public string ToUserName { get { return Record.ToUserName; } set { Record.ToUserName = value; } }
        /// <summary>
        /// 消息发送方微信号
        /// </summary>
        public string FromUserName { get { return Record.FromUserName; } set { Record.FromUserName = value; } }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get { return Record.CreateTime; } set { Record.CreateTime = value; } }
        /// <summary>
        /// 信息类型 地理位置:location,文本消息:text,消息类型:image
        /// </summary>
        public string MsgType { get { return Record.MsgType; } set { Record.MsgType = value; } }
        /// <summary>
        /// 信息内容
        /// </summary>
        public string Content { get { return Record.Content; } set { Record.Content = value; } }
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public double Location_X { get { return Record.Location_X; } set { Record.Location_X = value; } }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public double Location_Y { get { return Record.Location_Y; } set { Record.Location_Y = value; } }
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public int Scale { get { return Record.Scale; } set { Record.Scale = value; } }
        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label { get { return Record.Label; } set { Record.Label = value; } }
        /// <summary>
        /// 图片链接地址
        /// </summary>
        public string PicUrl { get { return Record.PicUrl; } set { Record.PicUrl = value; } }
        /// <summary>
        /// 事件Key
        /// </summary>
        public string EventKey { get { return Record.EventKey; } set { Record.EventKey = value; } }
        /// <summary>
        /// 事件类型
        /// </summary>
        public string Event { get { return Record.Event; } set { Record.Event = value; } }

        public int MsgId { get { return Record.MsgId; } set { Record.MsgId = value; } }

        public string MediaId { get { return Record.MediaId; } set { Record.MediaId = value; } }

        public string Format { get { return Record.Format; } set { Record.Format = value; } }

        public string ThumbMediaId { get { return Record.ThumbMediaId; } set { Record.ThumbMediaId = value; } }

        public string Title { get { return Record.Title; } set { Record.Title = value; } }

        public string Description { get { return Record.Description; } set { Record.Description = value; } }

        public string Url { get { return Record.Url; } set { Record.Url = value; } }

        public string Ticket { get { return Record.Ticket; } set { Record.Ticket = value; } }

        public double Latitude { get { return Record.Latitude; } set { Record.Latitude = value; } }

        public double Longitude { get { return Record.Longitude; } set { Record.Longitude = value; } }

        public double Precision { get { return Record.Precision; } set { Record.Precision = value; } }

        public string Recongnition { get { return Record.Recongnition; } set { Record.Recongnition = value; } }

        public string XML { get { return Record.XML; } set { Record.XML = value; } }
    }
}