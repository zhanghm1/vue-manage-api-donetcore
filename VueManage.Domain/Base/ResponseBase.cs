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

    public static class ResponseBaseCode
    {
        public static string SERVERFAIL = "SERVERFAIL";
        public static string SUCCESS = "SUCCESS";
        public static string FAIL = "FAIL";
        public static string Unauthorized = "Unauthorized";
        public static string NotFind = "NotFind";
    }


}
