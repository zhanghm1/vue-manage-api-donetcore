using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VueManage.Domain;
using VueManage.Domain.Base;
using VueManage.Domain.Entities;
using VueManage.Infrastructure.Common.Exceptions;
using VueManage.Infrastructure.EFCore;

namespace VueManage.Application.Users.Commands
{
    public class UserEditCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public List<int> UserRoleIds { get; set; }
    }
    public class UserEditCommandHandler : IRequestHandler<UserEditCommand, bool>
    {

        private readonly IRepository<ApplicationUser> _userRepository;
        private UserManager<ApplicationUser> _userManager;
        private readonly IRepository<ApplicationRole> _roleRepository;
        private readonly ApplicationDbContext _dbContext;
        public UserEditCommandHandler(IRepository<ApplicationUser> userRepository
            , UserManager<ApplicationUser> userManager
            , IRepository<ApplicationRole> roleRepository
            , ApplicationDbContext dbContext)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleRepository = roleRepository;
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(UserEditCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser();
            if (request.Id > 0)
            {
                user = await _userRepository.FindAsync(a => a.Id == request.Id);
                //if (user.LockoutEnabled)
                //{
                //    throw new ApiException(ResponseBaseCode.NotFind, "账号被锁定");
                //}
                user.Name = request.Name;
                user.PhoneNumber = request.PhoneNumber;
                await _userRepository.UpdateAsync(user); 
            }
            else
            {
                if ( await _userRepository.AnyAsync(a=>a.UserName== request.UserName))
                {
                    throw new ApiException(ResponseBaseCode.EXISTED);
                }
                user.Name = request.Name;
                user.PhoneNumber = request.PhoneNumber;
                await _userManager.CreateAsync(user, request.Password);
            }

            var result = await _userRepository.SaveChangeAsync() > 0;

            request.Id = user.Id;
            if(result)
            {
                if (user.PasswordHash != request.Password && request.Id > 0)
                {
                    //设置密码
                    var token =  await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, request.Password);
                }
                var roles = new List<ApplicationRole>() ;

                if (request.UserRoleIds != null && request.UserRoleIds.Count > 0)
                {
                    // 期望添加的权限
                    roles = (await _roleRepository.ListAsync(a => request.UserRoleIds.Contains(a.Id))).ToList();
                }

                var userRoleIds = _dbContext.UserRoles.Where(a => a.UserId == request.Id).ToList();

                // 需要添加的权限
                var addPer = roles.Where(a => !userRoleIds.Select(d => d.RoleId).Contains(a.Id)).ToList();
                // 需要减少的权限
                var jianPer = userRoleIds.Where(a => !roles.Select(d => d.Id).Contains(a.RoleId)).ToList();


                _dbContext.UserRoles.RemoveRange(jianPer);
                _dbContext.UserRoles.AddRange(addPer.Select(a=>new IdentityUserRole<int>() {RoleId =a.Id , UserId= user.Id }));

                await _dbContext.SaveChangesAsync();
            }
            
            return result;
        }
    }
}
