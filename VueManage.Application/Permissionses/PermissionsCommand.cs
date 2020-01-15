using MediatR;
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

namespace VueManage.Application.Permissionses
{
    public class PermissionsCommand : IRequestHandler<EditRoleCommand, bool>
    {
        private readonly IRepository<ApplicationRole> _roleRepository;
        private readonly IRepository<Permissions> _permissionsRepository;
        private readonly IRepository<RolePermissions> _rolePermissionsRepository;
        private readonly ApplicationDbContext _dbContext;
        private readonly IRepository<ApplicationUser> _userRepository;

        public PermissionsCommand(
            IRepository<ApplicationRole> roleRepository,
            IRepository<RolePermissions> rolePermissionsRepository,
            IRepository<ApplicationUser> userRepository,
            ApplicationDbContext dbContext,
            IRepository<Permissions> permissionsRepository

            )
        {
            _roleRepository = roleRepository;
            _rolePermissionsRepository = rolePermissionsRepository;
            _userRepository = userRepository;
            _dbContext = dbContext;
            _permissionsRepository = permissionsRepository;
        }
        public async Task<bool> Handle(EditRoleCommand request, CancellationToken cancellationToken)
        {
            // TODO 开事务

            ApplicationRole role = new ApplicationRole();
            if (request.Id > 0)
            {
                role = await _roleRepository.FindAsync(a => a.Id == request.Id);
                if (role == null)
                {
                    throw new ApiException(ResponseBaseCode.NotFind);
                }

                role.Description = request.Description;

                await _roleRepository.UpdateAsync(role);
            }
            else
            {
                role.NormalizedName = request.Name;
                role.Name = request.Name;
                role.Description = request.Description;

                await _roleRepository.AddAsync(role);
                await _rolePermissionsRepository.SaveChangeAsync();

                request.Id = role.Id;
            }


            // 已有的权限
            var rolePer = await _rolePermissionsRepository.ListAsync(a => a.RoleId == role.Id);
            var listPermissions = new List<Permissions>();
            if (request.PermissionsIds!=null && request.PermissionsIds.Count>0)
            {
                // 期望添加的权限
                listPermissions = (await _permissionsRepository.ListAsync(a => request.PermissionsIds.Contains(a.Id))).ToList();
            }
            
            

            // 需要添加的权限
            var addPer = listPermissions.Where(a => !rolePer.Select(a => a.PermissionsId).Contains(a.Id)).ToList();
            // 需要减少的权限
            var jianPer = rolePer.Where(a => !listPermissions.Select(a => a.Id).Contains(a.PermissionsId)).ToList();

            await _rolePermissionsRepository.RealDeleteAsync(jianPer);
            addPer.Select(a => new RolePermissions() {RoleId= role.Id,PermissionsId=a.Id });

            await  _rolePermissionsRepository.AddAsync(addPer.Select(a => new RolePermissions() { RoleId = role.Id, PermissionsId = a.Id }));
            await _rolePermissionsRepository.SaveChangeAsync();


            return true;
        }
    }
}
