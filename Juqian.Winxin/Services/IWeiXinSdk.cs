using Orchard;
using Orchard.MediaLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juqian.Winxin.Services
{
    public interface IWeiXinSdk : IDependency
    {
        string MenuGet();
        bool MenuDelete();
        bool MenuCreate(string json);

        MediaModel UploadFile(MediaPart part, string type);
    }

    public class MediaModel
    {
        public string type { get; set; }
        public string media_id { get; set; }
        public int created_at { get; set; }
        public string errcode { get; set; }
        public string errmsg { get; set; }
    }
}
