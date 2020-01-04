using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VueManage.Api.Models;
using VueManage.Api.Models.Accounts;
using VueManage.Api.Models.Users;
using VueManage.Domain;

namespace VueManage.Api.Controllers
{
    [Authorize]
    public class UserController : ApiControllerBase
    {
        public UserManager<IdentityUser> _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }
        [HttpGet]
        [AllowAnonymous]
        public string Get()
        {
            return "api";
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<ResponseBase> Register(UserRegisterRequest userRegister)
        {
            ResponseBase resp = new ResponseBase();
            IdentityUser user = await _userManager.FindByNameAsync(userRegister.UserName);
            if (user != null)
            {
                resp.Code = ResponseBaseCode.FAIL;
                return resp;
            }
            user = userRegister.ToEntity();
            var result = await _userManager.CreateAsync(user, userRegister.Password);
            if (result.Succeeded)
            {
                resp.Code = ResponseBaseCode.SUCCESS;
            }
            else
            {

                resp.Code = ResponseBaseCode.FAIL;
                // resp.Message= result.Errors
            }

            return resp;
        }


        [HttpGet]
        [Route("Info")]
        public ResponseBase<UserInfoResponse> GetInfo()
        {
            ResponseBase<UserInfoResponse> resp = new ResponseBase<UserInfoResponse>();
            resp.Data = new UserInfoResponse();

            return resp;
        }
    }
}
