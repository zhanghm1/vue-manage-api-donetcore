using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VueManage.Domain;
using VueManage.Infrastructure.Common;
using VueManage.Infrastructure.Common.Exceptions;

namespace VueManage.Api.Filter
{
    /// <summary>
    /// 异常错误过滤器
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {

        private LanguageManager _languageManager;
        public ExceptionFilter(LanguageManager languageManager)
        {
            _languageManager = languageManager;
        }
        public void OnException(ExceptionContext context)
        {
            var resp = new ResponseBase()
            {
                Code = ResponseBaseCode.SERVERFAIL
            };
            if (context.Exception is ApiException)
            {
                //这个错误属于正常返回的提示信息
                ApiException apiException = context.Exception as ApiException;
                if (!string.IsNullOrEmpty(apiException.Msg))
                {
                    resp.Code = apiException.Code;
                    resp.Message = apiException.Msg;
                }
                else
                {
                    resp.SetCodeMessage(_languageManager, apiException.Code);
                }
            }
            else
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                resp.SetCodeMessage(_languageManager, ResponseBaseCode.SERVERFAIL);
            }
            
            context.Result = new JsonResult(resp);
        }
    }
}
