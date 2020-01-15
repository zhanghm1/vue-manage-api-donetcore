using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VueManage.Domain;
using VueManage.Infrastructure.Common;

namespace VueManage.Api.Filter
{
    /// <summary>
    /// 权限验证过滤器
    /// </summary>
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private LanguageManager _languageManager;
        public AuthorizationFilter(LanguageManager languageManager)
        {
            _languageManager = languageManager;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            
            if (context.ActionDescriptor.EndpointMetadata.Any(item => item is AllowAnonymousAttribute))
            {
                return;
            }
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }

            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                ResponseBase resp = new ResponseBase();

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                
                resp.SetCodeMessage(_languageManager, ResponseBaseCode.Unauthorized);
                context.Result = new JsonResult(resp);
            }
            else 
            {   
                //是否有权限验证特性
                if (context.ActionDescriptor.EndpointMetadata.Any(item => item is PermissionAuthorizeAttribute))
                {
                    var claim = context.HttpContext.User.Claims.Where(a => a.Type == UserClaims.UserPermissions).FirstOrDefault();
                    
                    var permissionAttr = (PermissionAuthorizeAttribute) context.ActionDescriptor.EndpointMetadata.FirstOrDefault(item => item is PermissionAuthorizeAttribute);

                    if (claim==null || !claim.Value.Contains(permissionAttr.Permission))
                    {
                        ResponseBase resp = new ResponseBase();

                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        resp.SetCodeMessage(_languageManager, ResponseBaseCode.NotApiAccess);
                        context.Result = new JsonResult(resp);
                    }
                }
                //是否有角色验证特性
                if (context.ActionDescriptor.EndpointMetadata.Any(item => item is RoleAuthorizeAttribute))
                {
                    var claim = context.HttpContext.User.Claims.Where(a => a.Type == UserClaims.UserRoles).FirstOrDefault();

                    var roleAttr = (RoleAuthorizeAttribute)context.ActionDescriptor.EndpointMetadata.FirstOrDefault(item => item is RoleAuthorizeAttribute);

                    if (claim == null || !claim.Value.Contains(roleAttr.RoleName))
                    {
                        ResponseBase resp = new ResponseBase();

                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        resp.SetCodeMessage(_languageManager, ResponseBaseCode.NotApiAccess);
                        context.Result = new JsonResult(resp);
                    }
                }

            }
        }

    }
}
