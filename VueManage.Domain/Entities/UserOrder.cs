using System;
using System.Collections.Generic;
using System.Text;
using VueManage.Domain.Base;
using VueManage.Domain.Enums.EntityEnums;

namespace VueManage.Domain.Entities
{
    public class UserOrder: EntityBase
    {
        public string OrderNo { get; set; }
        public int UserId { get; set; }
        /// <summary>
        /// 实际金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        public decimal OriginalMoney { get; set; }
        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal DiscountMoney { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public OrderStatus OrderStatus { get; set; }

    }
}
