using Orchard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Juqian.Winxin.Models
{
    [ModelBinder(typeof(WXReqModelBinder))]
    public class WXReqModel
    {
        /// <summary>
        /// 消息接收方微信号，一般为公众平台账号微信号
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 消息发送方微信号
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 信息类型 地理位置:location,文本消息:text,消息类型:image
        /// </summary>
        public string MsgType { get; set; }

        public int MsgId { get; set; }

        public string MediaId { get; set; }

        public string Format { get; set; }

        public string ThumbMediaId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string Ticket { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Precision { get; set; }

        public string Recongnition { get; set; }
        /// <summary>
        /// 信息内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public double Location_X { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public double Location_Y { get; set; }
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public int Scale { get; set; }
        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 图片链接地址
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 事件Key
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 事件类型
        /// </summary>
        public string Event { get; set; }
        /// <summary>
        /// 原始请求XML
        /// </summary>
        public string XML { get; set; }
    }
}