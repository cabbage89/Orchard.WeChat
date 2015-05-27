using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Juqian.Winxin.Models
{
    public class WXResultException : Exception
    {
        public int Code;
        public WXResultException() { }
        public WXResultException(int errcode, string errmsg)
            : base(errmsg)
        {
            this.Code = errcode;
        }
    }
}