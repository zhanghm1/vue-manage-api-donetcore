using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VueManage.Domain;
using VueManage.Infrastructure.Common.Exceptions;

namespace VueManage.Api.Filter
{
    /// <summary>
    /// 异常错误过滤器
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
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
                resp.Code = apiException.Code;
                resp.Message = context.Exception.Message;
            }
            else
            {
                
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                resp.Code = ResponseBaseCode.SERVERFAIL;
                resp.Message = "服务出错";
            }
            
            context.Result = new JsonResult(resp);
        }
    }
}
