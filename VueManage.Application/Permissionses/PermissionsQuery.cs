using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VueManage.Application.Permissionses.Queries;
using VueManage.Domain;
using VueManage.Domain.Base;
using VueManage.Domain.Entities;
using VueManage.Infrastructure.EFCore;

namespace VueManage.Application.Permissionses
{

    public class PermissionsQuery : IRequestHandler<UserAllPermissionsQuery, List<UserAllPermissionsQueryResponse>>
    {
        private readonly IRepository<ApplicationRole> _roleRepository;
        private readonly IRepository<RolePermissions> _rolePermissionsRepository;
        private readonly IRepository<Permissions> _permissionsRepository;
        private readonly ApplicationDbContext  _dbContext;
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly IMapper _mapper;

        public PermissionsQuery(
            IRepository<ApplicationRole> roleRepository,
            IRepository<RolePermissions> rolePermissionsRepository,
            IRepository<ApplicationUser> userRepository,
            ApplicationDbContext dbContext,
            IMapper mapper,
            IRepository<Permissions> permissionsRepository

            )
        {
            _roleRepository = roleRepository;
            _rolePermissionsRepository = rolePermissionsRepository;
            _userRepository = userRepository;
            _dbContext = dbContext;
            _mapper = mapper;
            _permissionsRepository = permissionsRepository;
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

                resp = _mapper.Map<List<UserAllPermissionsQueryResponse>>(GetTreeList(PermissionsList, 0));
            }
            return resp;
        }
        /// <summary>
        /// 递归结构
        /// </summary>
        /// <param name="PermissionsList"></param>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        private List<PermissionsTreeQueryResponse> GetTreeList(IEnumerable<Permissions> PermissionsList,int ParentId)
        {
            List<PermissionsTreeQueryResponse> resp = new List<PermissionsTreeQueryResponse>();

            List<Permissions> childList = PermissionsList.Where(a => a.ParentId == ParentId).ToList();
            if (childList.Count==0)
            {
                return null;
            }
            foreach (var item in childList)
            {
                var child = new PermissionsTreeQueryResponse()
                {
                    Code= item.Code,
                    Id= item.Id,
                    IsMenu= item.IsMenu,
                    Name= item.Name,
                    ParentId= item.ParentId
                };

                child.Childs = GetTreeList(PermissionsList, item.Id);

                resp.Add(child);
            }

            return resp;
        }

        public async Task<RoleEditResponse> GetRoleEdit(int roleId)
        {
            var role = await _roleRepository.FindAsync(a => a.Id == roleId);
            var rolePermissions = await _rolePermissionsRepository.ListAsync(a => a.RoleId == roleId);

            var roleEditResponse = _mapper.Map<RoleEditResponse>(role);
            roleEditResponse.PermissionsIds = rolePermissions.Select(a => a.PermissionsId).ToList();

            var PermissionsList = await _permissionsRepository.ListAsync(a => !a.IsDeleted);

            roleEditResponse.AllPermissions = _mapper.Map<List<PermissionsTreeQueryResponse>>(GetTreeList(PermissionsList, 0));


            return roleEditResponse;
        }
        public async Task<List<PermissionsTreeQueryResponse>> GetPermissionsList()
        {
            var PermissionsList = await _permissionsRepository.ListAsync(a => !a.IsDeleted);
           return _mapper.Map<List<PermissionsTreeQueryResponse>>(GetTreeList(PermissionsList, 0));

        }
        public async Task<PageResponse<RoleListResponse>> GetRoleList(PageRequest pageRequest)
        {
            //PageResponse<RoleListResponse> resp = new PageResponse<RoleListResponse>();
            var roles = await _roleRepository.PageListAsync<int,RoleListResponse>(pageRequest, a => a.Id > 0, a => a.Id, false);

            
            return roles;
        }
    }
}
