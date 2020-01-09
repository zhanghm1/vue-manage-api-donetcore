using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace VueManage.Infrastructure.Common.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UserLanguageMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LanguageMiddleware>();
        }
    }

    public class LanguageMiddleware
    {
        private readonly RequestDelegate _next;

        public LanguageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string contentLanguage = context.Request.Headers["Content-Language"].ToString();
            var _languageManager = context.RequestServices.GetService<LanguageManager>();
            _languageManager.Area = contentLanguage;
            Console.WriteLine("invoke");
            await _next.Invoke(context);
        }
    }
}
