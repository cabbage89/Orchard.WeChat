using Juqian.Winxin.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Juqian.Winxin.Handlers
{
    public class WXMsgPartHandle : ContentHandler
    {
        public WXMsgPartHandle(IRepository<WXMsgPartRecord> repository) 
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}