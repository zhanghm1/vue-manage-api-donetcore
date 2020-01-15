using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueManage.Infrastructure.Common;

namespace VueManage.Domain
{
    public class ResponseBase
    {
        public ResponseBase()
        { 
        
        }
        public string Code { get; set; } = ResponseBaseCode.SUCCESS;
        public string Message { get; set; }

        public void SetCodeMessage(LanguageManager languageManager,string code)
        {
            this.Code = code;
            if (languageManager != null)
            {
                this.Message = languageManager.GetMessage(code);
            }
            else
            {
                this.Message = code;
            }
            
        }
    }
    public class ResponseBase<T>: ResponseBase
    {
        public T Data { get; set; }
    }

    /// <summary>
    /// 返回值的code,
    /// 如果需要适配多语言的Message，请在VueManage.Infrastructure.Common/LanguageResources对应资源下面添加对应说明，并使用  ResponseBase对象的 SetCodeMessage方法赋值
    /// </summary>
    public static class ResponseBaseCode
    {
        public static string SERVERFAIL = "SERVERFAIL";
        public static string SUCCESS = "SUCCESS";
        public static string FAIL = "FAIL";
        public static string Unauthorized = "Unauthorized";
        public static string NotFind = "NotFind";
        /// <summary>
        /// 已存在
        /// </summary>
        public static string EXISTED = "EXISTED";
        /// <summary>
        /// 没有API访问权限
        /// </summary>
        public static string NotApiAccess = "NotApiAccess";
    }


}
