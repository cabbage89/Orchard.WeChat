using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Juqian.Winxin.Models
{
    public class WinXinSettingsPart : ContentPart
    {
        public string AppId
        {
            get { return this.Retrieve(x => x.AppId); }
            set { this.Store(x => x.AppId, value); }
        }

        public string Secret
        {
            get { return this.Retrieve(x => x.Secret); }
            set { this.Store(x => x.Secret, value); }
        }

        public string AccessToken
        {
            get { return this.Retrieve(x => x.AccessToken); }
            set { this.Store(x => x.AccessToken, value); }
        }

        public int ExpiresIn
        {
            get { return this.Retrieve(x => x.ExpiresIn); }
            set { this.Store(x => x.ExpiresIn, value); }
        }

        public string ValidToken
        {
            get { return this.Retrieve(x => x.ValidToken); }
            set { this.Store(x => x.ValidToken, value); }
        }

        public string UserName
        {
            get { return this.Retrieve(x => x.UserName); }
            set { this.Store(x => x.UserName, value); }
        }
    }
}