using Orchard.ContentManagement.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Juqian.Winxin.Models
{
    public class WXMsgPartRecord : ContentPartRecord
    {
        /// <summary>
        /// 消息接收方微信号，一般为公众平台账号微信号
        /// </summary>
        public virtual string ToUserName { get; set; }
        /// <summary>
        /// 消息发送方微信号
        /// </summary>
        public virtual string FromUserName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual string CreateTime { get; set; }
        /// <summary>
        /// 信息类型 地理位置:location,文本消息:text,消息类型:image
        /// </summary>
        public virtual string MsgType { get; set; }
        /// <summary>
        /// 信息内容
        /// </summary>
        public virtual string Content { get; set; }
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public virtual double Location_X { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public virtual double Location_Y { get; set; }
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public virtual int Scale { get; set; }
        /// <summary>
        /// 地理位置信息
        /// </summary>
        public virtual string Label { get; set; }
        /// <summary>
        /// 图片链接地址
        /// </summary>
        public virtual string PicUrl { get; set; }
        /// <summary>
        /// 事件Key
        /// </summary>
        public virtual string EventKey { get; set; }
        /// <summary>
        /// 事件类型
        /// </summary>
        public virtual string Event { get; set; }

        public virtual int MsgId { get; set; }

        public virtual string MediaId { get; set; }

        public virtual string Format { get; set; }

        public virtual string ThumbMediaId { get; set; }

        public virtual string Title { get; set; }

        public virtual string Description { get; set; }

        public virtual string Url { get; set; }

        public virtual string Ticket { get; set; }

        public virtual double Latitude { get; set; }

        public virtual double Longitude { get; set; }

        public virtual double Precision { get; set; }

        public virtual string Recongnition { get; set; }

        public virtual string XML { get; set; }
    }
}