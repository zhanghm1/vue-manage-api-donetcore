using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VueManage.Domain;
using VueManage.Domain.Base;
using VueManage.Domain.Entities;
using VueManage.Infrastructure.Common;
using VueManage.Infrastructure.Common.Exceptions;

namespace VueManage.Application.Order
{
    public class CreateOrderCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public string Address { get; set; }

        public List<CreateOrderProdectRequest> Item{get;set;}
    }
    public class CreateOrderProdectRequest
    {
        public int ProdictId { get; set; }
        public int Number { get; set; }
    }
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
    {
        private readonly IRepository<UserOrder> _userOrderRepository;
        private readonly IRepository<UserOrderItem> _userOrderItemRepository;
        private readonly IRepository<Product> _productRepository;

        private readonly LanguageManager _languageManager;

        public CreateOrderCommandHandler(IRepository<UserOrder> userOrderRepository
            , IRepository<UserOrderItem> userOrderItemRepository
            , IRepository<Product> productRepository
            , LanguageManager languageManager
            )
        {
            _userOrderRepository = userOrderRepository;
            _userOrderItemRepository = userOrderItemRepository;
            _productRepository = productRepository;
            _languageManager = languageManager;
        }

        public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            List<UserOrderItem> orderItem = new List<UserOrderItem>();
            List<Product> products = new List<Product>();
            foreach (var item in request.Item)
            {
                var product = await _productRepository.FindAsync(item.ProdictId);
                products.Add(product);

                orderItem.Add(new UserOrderItem()
                {
                    OrderItemNo = Guid.NewGuid().ToString().Replace("-", ""),
                    OrderStatus = Domain.Enums.EntityEnums.OrderStatus.WaitPay,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductNo = product.ProductNo,
                    ProductOriginalPrice = product.OriginalPrice,
                    ProductPrice = product.Price,
                    UserId = request.UserId,
                    UserOrderId = 0,
                    ProductNumber = item.Number
                });
                if (product.Number > item.Number)
                {
                    product.Number -= item.Number;
                }
                else
                {
                    throw new ApiException(ResponseBaseCode.FAIL);
                }
            }
            UserOrder userOrder = new UserOrder()
            {
                Address = request.Address,
                OrderNo = Guid.NewGuid().ToString().Replace("-", ""),
                Money = orderItem.Sum(a => a.ProductPrice),
                OrderStatus = Domain.Enums.EntityEnums.OrderStatus.WaitPay,
                UserId = request.UserId,
                DiscountMoney = orderItem.Sum(a => a.ProductOriginalPrice - a.ProductPrice),
                OriginalMoney = orderItem.Sum(a => a.ProductOriginalPrice),
                CreateTime = DateTime.Now
            };


            using (var transaction = _userOrderRepository.BeginTransaction())
            {

                try
                {
                    await _userOrderRepository.AddAsync(userOrder);
                    await _userOrderRepository.SaveChangeAsync();

                    orderItem.ForEach(a =>
                    {
                        a.UserOrderId = userOrder.Id;
                    });
                    await _userOrderItemRepository.AddAsync(orderItem);
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
