using System;
using System.Collections.Generic;
using System.Text;

namespace VueManage.Application.Permissionses.Queries
{
    public class RoleEditResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<int> PermissionsIds { get; set; }

        public List<PermissionsTreeQueryResponse> AllPermissions { get; set; }
    }
}
