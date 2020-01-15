using System;
using System.Collections.Generic;
using System.Text;

namespace VueManage.Domain
{
    /// <summary>
    /// 分页参数提交的基类
    /// </summary>
    public class PageRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

    }
    /// <summary>
    /// 分页参数提交的基类 带其他参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PageRequest<T>: PageRequest
    {
        public T Data { get; set; }
    }
    /// <summary>
    /// 分页返回参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageResponse<T>
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
