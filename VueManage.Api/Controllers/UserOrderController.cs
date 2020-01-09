using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VueManage.Api.Models;
using VueManage.Api.Models.Accounts;
using VueManage.Api.Models.Users;
using VueManage.Application.Order;
using VueManage.Domain;
using VueManage.Domain.Entities;
using VueManage.Infrastructure.Common.Exceptions;

namespace VueManage.Api.Controllers
{
    public class UserOrderController : ApiControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserOrderController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("CreateOrder")]
        public async Task<ResponseBase> CreateOrder(CreateOrderCommand request)
        {
            request.UserId = this.CurrentUserId;
            ResponseBase resp = new ResponseBase();
            bool result = await Mediator.Send(request);
            if (!result)
            {
                resp.Code = ResponseBaseCode.FAIL;
                resp.Message = "订单提交失败";
            }
            return resp;
        }
        [HttpPost]
        [Route("DeleteOrder")]
        public async Task<ResponseBase> DeleteOrder(DeleteOrderCommand request)
        {
            request.UserId = this.CurrentUserId;
            ResponseBase resp = new ResponseBase();
            bool result = await Mediator.Send(request);
            if (!result)
            {
                resp.SetCodeMessage(languageManager, ResponseBaseCode.FAIL);
            }
            
            return resp;
        }

    }
}
