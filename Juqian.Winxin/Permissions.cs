using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Juqian.Winxin
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageWeiXin = new Permission { Description = "管理微信配置", Name = "ManageWeiXin" };
        public virtual Feature Feature { get; set; }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] {ManageWeiXin}
                },
            };
        }

        public IEnumerable<Permission> GetPermissions()
        {
            return new[] {
                ManageWeiXin,
            };
        }
    }
}