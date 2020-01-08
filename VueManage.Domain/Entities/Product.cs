using System;
using System.Collections.Generic;
using System.Text;
using VueManage.Domain.Base;

namespace VueManage.Domain.Entities
{
    public class Product : EntityBase
    {
        public string ProductNo { get; set; }
        public string Name{ get; set; }

        public decimal Price { get; set; }

        public decimal OriginalPrice { get; set; }
        /// <summary>
        /// 库存数量
        /// </summary>
        public int Number { get; set; }
    }
}
