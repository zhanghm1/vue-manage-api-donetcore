using System;
using System.Collections.Generic;
using System.Text;
using VueManage.Domain.Base;
using VueManage.Domain.Enums.EntityEnums;

namespace VueManage.Domain.Entities
{
    public class UserOrderItem : EntityBase
    {
        public string OrderItemNo { get; set; }
        public int ProductId{get;set;}
        public string ProductNo { get; set; }
        public int UserId { get; set; }
        public int UserOrderId { get; set; }
        public string ProductName { get; set; }

        public int ProductNumber { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductOriginalPrice { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }
}
