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

namespace VueManage.Api.Filter
{
    /// <summary>
    /// 权限验证过滤器
    /// </summary>
    public class AuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }

            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                //IActionResult
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new JsonResult(new ResponseBase()
                {
                    Code = ResponseBaseCode.Unauthorized,
                    Message = "身份验证无效"
                });
            }
        }

    }
}
