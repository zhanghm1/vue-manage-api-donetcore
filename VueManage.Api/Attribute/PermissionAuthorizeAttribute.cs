using Microsoft.AspNetCore.Authorization;
using System;

namespace VueManage.Api
{
    /// <summary>
    /// ��Ҫָ����Ȩ�޲��ܷ���
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
    /// ��Ҫָ���Ľ�ɫ���ܷ���
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