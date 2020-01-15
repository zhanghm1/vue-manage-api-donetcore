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
using VueManage.Application.Users;
using VueManage.Application.Users.Commands;
using VueManage.Application.Users.Queries;
using VueManage.Domain;
using VueManage.Domain.Base;
using VueManage.Domain.Entities;

namespace VueManage.Api.Controllers
{
    public class UserController : ApiControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserController> _logger;
        private UserQuery _userQuery;

        public UserController(ILogger<UserController> logger, UserManager<ApplicationUser> userManager, UserQuery userQuery)
        {
            _logger = logger;
            _userManager = userManager;
            _userQuery = userQuery;
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
                resp.SetCodeMessage(languageManager, ResponseBaseCode.EXISTED);
                return resp;
            }
            user = userRegister.ToEntity();
            var result = await _userManager.CreateAsync(user, userRegister.Password);
            if (result.Succeeded)
            {
                resp.SetCodeMessage(languageManager, ResponseBaseCode.SUCCESS);
            }
            else
            {
                resp.SetCodeMessage(languageManager, ResponseBaseCode.FAIL);
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
        [HttpGet]
        [Route("UserList")]
        [PermissionAuthorize(PermissionsCode.system_user_list)]
        public async Task<ResponseBase<PageResponse<ApplicationUser>>> UserList(int pageindex=1,int pagesize=20)
        {
            UserListQuery pageRequest = new UserListQuery() { 
                PageIndex= pageindex,
                PageSize= pagesize
            };
            ResponseBase<PageResponse<ApplicationUser>> response = new ResponseBase<PageResponse<ApplicationUser>>();

            response.Data = await Mediator.Send(pageRequest);
            return response;
        }
        [HttpGet]
        [Route("UserEdit/{Id}")]
        [PermissionAuthorize(PermissionsCode.system_user_edit)]
        public async Task<ResponseBase<UserEditQuery>> UserEdit(int Id)
        {

            ResponseBase<UserEditQuery> resp = new ResponseBase<UserEditQuery>();
            resp.Data = await _userQuery.GetEditUser(Id);

            return resp;
        }
        [HttpPost]
        [Route("UserEdit")]
        [PermissionAuthorize(PermissionsCode.system_user_edit)]
        public async Task<ResponseBase<int>> UserEdit(UserEditCommand request)
        {
            ResponseBase<int> resp = new ResponseBase<int>();

            if (await Mediator.Send(request))
            {
                resp.Data = request.Id;
            }
            else 
            {
                resp.SetCodeMessage(languageManager,ResponseBaseCode.FAIL);
            }
            return resp;
        }
    }
}
