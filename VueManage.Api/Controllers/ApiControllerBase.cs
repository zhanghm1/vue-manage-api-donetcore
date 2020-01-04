using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace VueManage.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiControllerBase : ControllerBase
    {
        //public readonly ILogger<ApiControllerBase> _logger;

        //public ApiControllerBase(ILogger<ApiControllerBase> logger)
        //{
        //    _logger = logger;
        //}
    }
}
