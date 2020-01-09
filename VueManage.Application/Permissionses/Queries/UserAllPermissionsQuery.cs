using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VueManage.Domain.Base;
using VueManage.Domain.Entities;
using VueManage.Infrastructure.EFCore;

namespace VueManage.Application.Permissionses
{
    public class UserAllPermissionsQuery : IRequest<List<UserAllPermissionsQueryResponse>>
    {
        public int UserId { get; set; }
    }
    public class UserAllPermissionsQueryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsMenu { get; set; }
        public int ParentId { get; set; }

        public List<UserAllPermissionsQueryResponse> Childs { get; set; }
    }
    public class UserAllPermissionsCommandHandler : IRequestHandler<UserAllPermissionsQuery, List<UserAllPermissionsQueryResponse>>
    {
        private readonly IRepository<ApplicationRole> _roleRepository;
        private readonly IRepository<RolePermissions> _rolePermissionsRepository;
        private readonly ApplicationDbContext  _dbContext;
        private readonly IRepository<ApplicationUser> _userRepository;

        public UserAllPermissionsCommandHandler(
            IRepository<ApplicationRole> roleRepository,
            IRepository<RolePermissions> rolePermissionsRepository,
            IRepository<ApplicationUser> userRepository,
            ApplicationDbContext dbContext

            )
        {
            _roleRepository = roleRepository;
            _rolePermissionsRepository = rolePermissionsRepository;
            _userRepository = userRepository;
            _dbContext = dbContext;
        }

        public async Task<List<UserAllPermissionsQueryResponse>> Handle(UserAllPermissionsQuery request, CancellationToken cancellationToken)
        {
            List<UserAllPermissionsQueryResponse> resp = new List<UserAllPermissionsQueryResponse>();

            List<Permissions> PermissionsList = new List<Permissions>();
            var user = await _userRepository.FindNoTrackingAsync(request.UserId);
            if (user!=null)
            {
                var userRolelist = _dbContext.UserRoles.Where(a => a.UserId == request.UserId).ToList();
                foreach (var item in userRolelist)
                {
                    var permissionses = _rolePermissionsRepository.Include(a=>a.Permissions).Where(a => a.RoleId == item.RoleId).Select(a => a.Permissions).ToList();
                    if (permissionses.Count>0)
                    {
                        PermissionsList.AddRange(permissionses.Where(a=>a!=null));
                    }
                    
                }
                PermissionsList = PermissionsList.Distinct().ToList();

                resp = GetList(PermissionsList, 0);
            }
            return resp;
        }
        /// <summary>
        /// 递归结构
        /// </summary>
        /// <param name="PermissionsList"></param>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        private List<UserAllPermissionsQueryResponse> GetList(List<Permissions> PermissionsList,int ParentId)
        {
            List<UserAllPermissionsQueryResponse> resp = new List<UserAllPermissionsQueryResponse>();

            List<Permissions> childList = PermissionsList.Where(a => a.ParentId == ParentId).ToList();
            if (childList.Count==0)
            {
                return null;
            }
            foreach (var item in childList)
            {
                var child = new UserAllPermissionsQueryResponse()
                {
                    Code= item.Code,
                    Id= item.Id,
                    IsMenu= item.IsMenu,
                    Name= item.Name,
                    ParentId= item.ParentId
                };

                child.Childs = GetList(PermissionsList, item.Id);

                resp.Add(child);
            }

            return resp;
        }
    }
}
