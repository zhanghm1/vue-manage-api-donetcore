using System;
using System.Collections.Generic;
using System.Text;

namespace VueManage.Domain.Base
{
    public abstract class PageRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

    }
    public class PageList<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// 总行数
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public List<T> List { get; set; }
        
    }
}
