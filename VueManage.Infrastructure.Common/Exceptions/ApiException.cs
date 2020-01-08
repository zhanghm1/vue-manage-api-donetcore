using System;
using System.Collections.Generic;
using System.Text;

namespace VueManage.Infrastructure.Common.Exceptions
{
    public class ApiException: Exception
    {
        public string Code { get; set; }
        public ApiException(string code,string msg):base(msg)
        {
            Code = code;
        }
    }
}
