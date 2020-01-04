using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VueManage.Api.Models;
using VueManage.Domain;

namespace VueManage.Api.IdentityServer4
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public UserManager<IdentityUser> _userManager;
        private IHttpContextAccessor _accessor;
        public ResourceOwnerPasswordValidator(IHttpContextAccessor accessor, UserManager<IdentityUser> userManager)
        {
            this._accessor = accessor;
            this._userManager = userManager;
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
                 subject: context.UserName,
                 authenticationMethod: "custom",
                 authTime: DateTime.UtcNow,
                 claims: GetUserClaims(user),
                 customResponse: new Dictionary<string, object>() { 
                     //显示在返回的json中
                     {"UserId",user.Id },
                     {"UserName",user.UserName },
                     {"UserRole",await GetUserRole(user) },
                     {"UserPermission",await GetUserPermission(user)}
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
        private Claim[] GetUserClaims(IdentityUser user)
        {
            var claims = new Claim[4];
            claims[0] = new Claim("UserId", user.Id);
            claims[1] = new Claim("UserName", user.UserName);
            if (!string.IsNullOrEmpty(user.Email))
            {
                claims[2] = new Claim("Email", user.Email);
            }
            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                claims[3] = new Claim("PhoneNumber", user.PhoneNumber);
            }
            return claims;
        }

        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<List<string>> GetUserPermission(IdentityUser user)
        {
            List<string> resp = new List<string>();
            resp.Add("user_list");
            resp.Add("user_edit");
            resp.Add("systemuser");

            return resp;
        }
        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<IList<string>> GetUserRole(IdentityUser user)
        {
            IList<string> resp = await _userManager.GetRolesAsync(user);
            return resp;
        }
    }
}
