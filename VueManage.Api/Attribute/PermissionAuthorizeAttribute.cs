using Microsoft.AspNetCore.Authorization;
using System;

namespace VueManage.Api
{
    /// <summary>
    /// 需要指定的权限才能访问
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PermissionAuthorizeAttribute  :Attribute
    {
        public string Permission { get; set; }
        public PermissionAuthorizeAttribute(string permission) 
        {
           this.Permission = permission;
        }
    }
    /// <summary>
    /// 需要指定的角色才能访问
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RoleAuthorizeAttribute : Attribute
    {
        public string RoleName { get; set; }
        public RoleAuthorizeAttribute(string roleName)
        {
            this.RoleName = roleName;
        }
    }
}