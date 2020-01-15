using IdentityServer4.Models;
using IdentityServer4.Validation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VueManage.Api.Models;
using VueManage.Application.Permissionses;
using VueManage.Domain;
using VueManage.Domain.Entities;
using VueManage.Infrastructure.EFCore;

namespace VueManage.Api
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _dbContext;
        private IHttpContextAccessor _accessor;
        protected IMediator _Mediator;
        public ResourceOwnerPasswordValidator(IHttpContextAccessor accessor, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, IMediator mediator)
        {
            this._accessor = accessor;
            this._userManager = userManager;
            this._dbContext = dbContext;
            this._Mediator = mediator;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            ResponseBase resp = new ResponseBase();


            var httpcontext = _accessor.HttpContext;
            var userid = httpcontext.Request.Form["UserId"].ToString();


            //根据context.UserName和context.Password与数据库的数据做校验，判断是否合法
            var user = await _userManager.FindByNameAsync(context.UserName);
            if (user == null)
            {
                resp.Code = ResponseBaseCode.FAIL;
            }
            if (await _userManager.CheckPasswordAsync(user, context.Password))
            {
                context.Result = new GrantValidationResult(
                 subject: user.Id.ToString(),
                 authenticationMethod: "custom",
                 authTime: DateTime.UtcNow,
                 claims: GetUserClaims(user),

                 customResponse: new Dictionary<string, object>() { 
                     //显示在返回的json中
                     {"UserId",user.Id },
                     {"UserName",user.UserName },
                     {"UserRole",await GetUserRole(user) },
                     {"UserPermission",GetUserAllPermissions(user)  }
                 }
                );
            }
            else
            {
                resp.Code = ResponseBaseCode.FAIL;
                //验证失败
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");
            }

        }
        //可以根据需要设置相应的 Claim
        private Claim[] GetUserClaims(ApplicationUser user)
        {
            List<Claim> list = new List<Claim>();
            list.Add(new Claim(UserClaims.UserId, user.Id.ToString()));
            list.Add(new Claim(UserClaims.UserName, user.UserName));
            list.Add(new Claim(UserClaims.UserPermissions, string.Join(',', GetUserAllPermissions(user).Select(a=>a.Code)) ));

            if (!string.IsNullOrEmpty(user.Email))
            {
                list.Add(new Claim(UserClaims.Email, user.Email));
            }
            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                list.Add(new Claim(UserClaims.PhoneNumber, user.PhoneNumber));
            }
            return list.ToArray();
        }

        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<Permissions> GetUserAllPermissions(ApplicationUser user)
        {
            UserAllPermissionsQuery request = new UserAllPermissionsQuery();
            request.UserId = user.Id;
            var linq = from ur in _dbContext.UserRoles
                       join rp in _dbContext.RolePermissions on ur.RoleId equals rp.RoleId
                       join p in _dbContext.Permissions on rp.PermissionsId equals p.Id
                       select p;

            return linq.ToList();
        }

        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<IList<string>> GetUserRole(ApplicationUser user)
        {
            var linqs = from ur in _dbContext.UserRoles
                        join r in _dbContext.Roles on ur.RoleId equals r.Id
                        where ur.UserId == user.Id
                        select r.Name;

            IList<string> resp = linqs.ToList();
            return resp;
        }
    }

    public class UserClaims
    {
        public static string UserId = "UserId";
        public static string UserName = "UserName";
        public static string Email = "Email";
        public static string PhoneNumber = "PhoneNumber";
        public static string UserPermissions = "UserPermissions";
        public static string UserRoles = "UserRoles";
    }
}
