using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Juqian.Winxin.Models
{
    public class WeiXinRespMenuModel
    {
        public WeiXinMenuModel menu;
    }
    public class WeiXinMenuModel
    {
        public WeiXinMenuModel()
        {
            button = new List<WeiXinMenuItemModel>();
        }
        public IList<WeiXinMenuItemModel> button;
    }
    public class WeiXinMenuItemModel
    {
        public WeiXinMenuItemModel()
        {
            sub_button = new List<WeiXinMenuItemModel>();
        }
        public WeiXinMenuItemModel(string name)
            : this()
        {
            this.name = name;

        }
        public string type;
        public string name;
        public string key;
        public string url;
        public IList<WeiXinMenuItemModel> sub_button;
    }
}