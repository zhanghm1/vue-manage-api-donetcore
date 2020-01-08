using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace VueManage.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiControllerBase : ControllerBase
    {
        //public readonly ILogger<ApiControllerBase> _logger;

        //public ApiControllerBase(ILogger<ApiControllerBase> logger, IMediator mediator)
        //{
        //    this._logger = logger;
        //    this.Mediator = mediator;
        //}


        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        public int CurrentUserId { 
            get 
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var claim = HttpContext.User.Claims.Where(a => a.Type == UserClaims.UserId).FirstOrDefault();
                   return Convert.ToInt32(claim.Value);
                }
                return 0;
            } 
        }
    }
}
