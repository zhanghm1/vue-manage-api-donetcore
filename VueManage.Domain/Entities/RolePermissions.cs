using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VueManage.Domain.Base;

namespace VueManage.Domain.Entities
{
    public class RolePermissions: EntityBase
    {
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public ApplicationRole Role { get; set; }
        public int PermissionsId { get; set; }
        [ForeignKey("PermissionsId")]
        public Permissions Permissions { get; set; }
    }
}
