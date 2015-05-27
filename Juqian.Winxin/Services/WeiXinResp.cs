using Juqian.Winxin.Models;
using Orchard;
using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Juqian.Winxin.Services
{
    public interface IWeiXinResp : IDependency
    {
        void Add(Tuple<ContentItem, string> model);

        IEnumerable<Tuple<ContentItem,string>> List();
    }
    public class WeiXinResp : IWeiXinResp
    {
        private readonly IList<Tuple<ContentItem, string>> _entries;

        public WeiXinResp()
        {
            _entries = new List<Tuple<ContentItem, string>>();
        }
        public void Add(Tuple<ContentItem, string> model)
        {
            _entries.Add(model);
        }

        public IEnumerable<Tuple<ContentItem, string>> List()
        {
            return _entries;
        }
    }
}