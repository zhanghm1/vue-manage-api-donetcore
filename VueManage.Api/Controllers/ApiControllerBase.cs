using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using VueManage.Infrastructure.Common;

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

        private LanguageManager _languageManager { get; set; }
        public LanguageManager languageManager
        {
            get
            {
                _languageManager = HttpContext.RequestServices.GetService<LanguageManager>();
                string contentLanguage = HttpContext.Request.Headers["Content-Language"].ToString();
                //_languageManager.Area = "zh-cn"; //初始化地区
                return _languageManager;
            }
        }

    }
}
