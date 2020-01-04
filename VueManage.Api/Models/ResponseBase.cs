using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VueManage.Api.Models
{
    public class ResponseBase
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
    public class ResponseBase<T>: ResponseBase
    {
        public T Data { get; set; }
    }

    public static class ResponseBaseCode
    {
        public static string SUCCESS = "SUCCESS";
        public static string FAIL = "FAIL";
    }
}
