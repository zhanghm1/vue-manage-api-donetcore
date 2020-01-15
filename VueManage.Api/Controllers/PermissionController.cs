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
using VueManage.Application.Permissionses.Queries;
using VueManage.Application.Users.Commands;
using VueManage.Application.Users.Queries;
using VueManage.Domain;
using VueManage.Domain.Base;
using VueManage.Domain.Entities;

namespace VueManage.Api.Controllers
{
    public class PermissionController : ApiControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private PermissionsQuery _permissionsQuery;

        public PermissionController(ILogger<UserController> logger, PermissionsQuery permissionsQuery)
        {
            _logger = logger;
            _permissionsQuery = permissionsQuery;


        }
        [HttpGet]
        [Route("List")]
        [PermissionAuthorize(PermissionsCode.system_role_list)]
        public async Task<ResponseBase<PageResponse<RoleListResponse>>> List(int pageindex=1,int pagesize=20)
        {
            PageRequest pageRequest = new PageRequest() { 
                PageIndex= pageindex,
                PageSize= pagesize
            };
            ResponseBase<PageResponse<RoleListResponse>> response = new ResponseBase<PageResponse<RoleListResponse>>();

            response.Data = await _permissionsQuery.GetRoleList(pageRequest);
            return response;
        }
        [HttpGet]
        [Route("Edit/{Id}")]
        [PermissionAuthorize(PermissionsCode.system_role_edit)]
        public async Task<ResponseBase<RoleEditResponse>> Edit(int Id)
        {
            ResponseBase<RoleEditResponse> resp = new ResponseBase<RoleEditResponse>();
            if (Id > 0)
            {
                resp.Data = await _permissionsQuery.GetRoleEdit(Id);
            }
            else {
                resp.Data = new RoleEditResponse();
                resp.Data.AllPermissions = await _permissionsQuery.GetPermissionsList();
            }
            
            
            return resp;
        }
        [HttpPost]
        [Route("Edit")]
        [PermissionAuthorize(PermissionsCode.system_role_edit)]
        public async Task<ResponseBase<int>> Edit(EditRoleCommand request)
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
