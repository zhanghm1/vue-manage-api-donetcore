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
using VueManage.Application.Permissionses;
using VueManage.Domain;
using VueManage.Domain.Entities;

namespace VueManage.Api.Controllers
{
    public class UserController : ApiControllerBase
    {
        public UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, UserManager<ApplicationUser> userManager)
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
            ApplicationUser user = await _userManager.FindByNameAsync(userRegister.UserName);
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
        public async Task<ResponseBase<UserInfoResponse>> GetInfo()
        {
            ResponseBase<UserInfoResponse> resp = new ResponseBase<UserInfoResponse>();
            resp.Data = new UserInfoResponse();
            bool IsAuthenticated = HttpContext.User.Identity.IsAuthenticated;
            var user = await _userManager.GetUserAsync(HttpContext.User);

            resp.Data.GetUserInfo(user);

            return resp;
        }
        [HttpGet]
        [Route("UserPermissions")]
        public async Task<ResponseBase<List<UserAllPermissionsQueryResponse>>> UserPermissions()
        {
            UserAllPermissionsQuery request = new UserAllPermissionsQuery();
            request.UserId = this.CurrentUserId;
            ResponseBase<List<UserAllPermissionsQueryResponse>> resp = new ResponseBase<List<UserAllPermissionsQueryResponse>>();
            
            resp.Data = await Mediator.Send(request);
            return resp;
        }
    }
}
