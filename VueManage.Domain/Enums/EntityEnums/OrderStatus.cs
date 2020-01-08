using System;
using System.Collections.Generic;
using System.Text;

namespace VueManage.Domain.Enums.EntityEnums
{
    public enum OrderStatus
    {
        WaitPay=0,
        WaitSending,
        WaitConfirm,
        Complete,
        Commented,
        /// <summary>
        /// 退款退货
        /// </summary>
        ApplyRefund,
        /// <summary>
        /// 退款退货发货
        /// </summary>
        ApplyRefundSending,
        /// <summary>
        /// 退款退货完成
        /// </summary>
        ApplyRefundComplete,
    }
}
