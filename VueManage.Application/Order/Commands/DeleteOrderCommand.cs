using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VueManage.Domain;
using VueManage.Domain.Base;
using VueManage.Domain.Entities;
using VueManage.Infrastructure.Common.Exceptions;

namespace VueManage.Application.Order
{
    public class DeleteOrderCommand : IRequest<bool>
    {
        public string OrderNo { get; set; }
        public int UserId { get; set; }
        public class CreateOrderRequestHandler : IRequestHandler<DeleteOrderCommand, bool>
        {
            private readonly IRepository<UserOrder> _userOrderRepository;
            private readonly IRepository<UserOrderItem> _userOrderItemRepository;
            private readonly IRepository<Product> _productRepository;

            public CreateOrderRequestHandler(IRepository<UserOrder> userOrderRepository
                , IRepository<UserOrderItem> userOrderItemRepository
                , IRepository<Product> productRepository)
            {
                _userOrderRepository = userOrderRepository;
                _userOrderItemRepository = userOrderItemRepository;
                _productRepository = productRepository;
            }

            public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
            {
                var userOrder = await _userOrderRepository.FindAsync(a => a.OrderNo == request.OrderNo && !a.IsDeleted );
                if (userOrder==null)
                {
                    throw new ApiException(ResponseBaseCode.NotFind, "没有找到订单");
                }
                if (userOrder.UserId!= request.UserId)
                {
                    throw new ApiException(ResponseBaseCode.FAIL, "不是您的订单");
                }
                //可以加状态判断

                List<Product> products = new List<Product>();
                var list = await _userOrderItemRepository.ListAsync(a => a.UserOrderId == userOrder.Id);
                foreach (var item in list)
                {
                    var product = await _productRepository.FindAsync(a => a.ProductNo == item.ProductNo);
                    product.Number += item.ProductNumber;
                    products.Add(product);
                }


                using (var transaction = _userOrderRepository.BeginTransaction())
                {
                    try
                    {
                        await _userOrderRepository.DeleteAsync(userOrder);
                        await _userOrderItemRepository.DeleteAsync(list);
                        await _productRepository.UpdateAsync(products);

                        await _userOrderRepository.SaveChangeAsync();

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        //日志
                        await transaction.RollbackAsync();
                        return false;
                    }
                }
            }
        }
    }
}
