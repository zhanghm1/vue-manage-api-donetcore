using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VueManage.Domain;
using VueManage.Domain.Entities;

namespace VueManage.Infrastructure.EFCore
{
    public class ApplicationDbContentSeed
    {
        ApplicationDbContext _dbContext;
        UserManager<ApplicationUser> _userManager;
        public ApplicationDbContentSeed(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }


        public async Task Init()
        {
            await AddAllPermissions();


            await AddRole();

            

            await AddUsers();

        }
        public async Task AddUsers()
        {
            string UserName = "admin";
            string password = "Admin123456!";

            if (!_dbContext.Users.Any(a => a.UserName == UserName))
            {
                var user = new ApplicationUser()
                {
                    UserName = UserName,
                    Name = UserName,

                };
                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    string RoleName = "管理员";
                    var role = _dbContext.Roles.Where(a => a.Name == RoleName).FirstOrDefault();
                    if (!_dbContext.UserRoles.Where(a => a.UserId == user.Id && a.RoleId == role.Id).Any())
                    {
                        _dbContext.UserRoles.Add(new IdentityUserRole<int>() { 
                        RoleId=role.Id,
                        UserId=user.Id
                        
                        });
                        _dbContext.SaveChanges();
                    }

                }
            }
        }

        public async Task<ApplicationRole> AddRole()
        {
            string RoleName = "管理员";
            string[] Permissions = new string[] {
                
                
                PermissionsCode.system_manage,
                PermissionsCode.system_user_list,
                PermissionsCode.system_user_edit,
                PermissionsCode.system_role_list,
                PermissionsCode.system_role_edit,
            };

            var role = _dbContext.Roles.FirstOrDefault(a => a.Name == RoleName);
            if (role==null)
            {
                role = new ApplicationRole() { Name = RoleName };
                _dbContext.Roles.Add(role);
                await _dbContext.SaveChangesAsync();

                foreach (var item in Permissions)
                {
                   var permissions = _dbContext.Permissions.FirstOrDefault(a => a.Code == item);
                    if (permissions!=null)
                    {
                        _dbContext.RolePermissions.Add(new RolePermissions() { RoleId = role.Id, PermissionsId = permissions.Id });
                    }
                }
            }
            return role;
        }
        public async Task AddAllPermissions()
        {
            #region 父级权限  模块
            var system_manage = new Permissions() {Id=1, Code = PermissionsCode.system_manage, Name = "系统管理", IsMenu = true, ParentId = 0 };

            List<Permissions> parentPermissions = new List<Permissions>();

            parentPermissions.Add(system_manage);

            foreach (var item in parentPermissions)
            {
                await AddPermissions(item);
            }
            #endregion



            #region 子权限

            var system_role = new Permissions() { Id = 101, Code = PermissionsCode.system_role_list, Name = "角色列表", IsMenu = true, ParentId = system_manage.Id };
            var system_role_edit = new Permissions() { Id = 102, Code = PermissionsCode.system_role_edit, Name = "角色编辑", IsMenu = false, ParentId = system_manage.Id };
            var system_user_list = new Permissions() { Id= 103, Code = PermissionsCode.system_user_list, Name = "用户列表", IsMenu = true, ParentId = system_manage.Id };
            var system_user_edit = new Permissions() { Id = 104, Code = PermissionsCode.system_user_edit, Name = "用户编辑", IsMenu = false, ParentId = system_manage.Id };

            

            List<Permissions> childPermissions = new List<Permissions>();

            childPermissions.Add(system_role);
            childPermissions.Add(system_role_edit);
            childPermissions.Add(system_user_list);
            childPermissions.Add(system_user_edit);


            foreach (var item in childPermissions)
            {
                await AddPermissions(item);
            } 
            #endregion

        }
        public async Task AddPermissions(Permissions permissions)
        {
            if (!_dbContext.Permissions.Any(a => a.Code == permissions.Code))
            {
                _dbContext.Permissions.Add(permissions);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
