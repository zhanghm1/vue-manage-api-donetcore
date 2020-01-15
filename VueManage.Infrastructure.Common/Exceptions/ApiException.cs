using System;
using System.Collections.Generic;
using System.Text;

namespace VueManage.Infrastructure.Common.Exceptions
{
    public class ApiException: Exception
    {
        public string Code { get; set; }
        public string Msg { get; set; }
        public ApiException(string code)
        {
            Code = code;
        }
        public ApiException(string code,string msg)
        {
            Code = code;
            Msg = msg;
        }
    }
}
