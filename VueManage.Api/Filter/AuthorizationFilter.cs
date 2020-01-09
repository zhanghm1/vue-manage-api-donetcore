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
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }

            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                ResponseBase resp = new ResponseBase();
                //IActionResult
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                //context.Result = new JsonResult(new ResponseBase()
                //{
                //    Code = ResponseBaseCode.Unauthorized,
                //    Message = "身份验证无效"
                //});

                resp.SetCodeMessage(_languageManager, ResponseBaseCode.Unauthorized);
                context.Result = new JsonResult(resp);
            }
        }

    }
}
