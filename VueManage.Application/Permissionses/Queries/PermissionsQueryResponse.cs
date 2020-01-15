using System;
using System.Collections.Generic;
using System.Text;

namespace VueManage.Application.Permissionses.Queries
{
    /// <summary>
    /// 权限树查询返回
    /// </summary>
    public class PermissionsTreeQueryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsMenu { get; set; }
        public int ParentId { get; set; }

        public List<PermissionsTreeQueryResponse> Childs { get; set; }
    }
}
