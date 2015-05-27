using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Juqian.Winxin.Models
{
    public class WeiXinClickMenuItemPart : ContentPart
    {
        public string Key
        {
            get { return this.Retrieve(x => x.Key); }
            set { this.Store(x => x.Key, value); }
        }
    }
}