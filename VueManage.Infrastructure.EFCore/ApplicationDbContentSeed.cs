using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            await AddPermissions();


            await AddRole(new string[] { "user_manage", "user_list", "user_edit" });

            

            await AddUsers();
            await AddProduct();
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
        public async Task AddProduct()
        {
            string ProductNo = "A00000001";
            if (!_dbContext.Product.Any(a => a.ProductNo == ProductNo))
            {
                _dbContext.Product.Add(new Product()
                {
                    OriginalPrice = 50,
                    Name = "测试商品",
                    Number = 50,
                    Price = 45,
                    ProductNo = ProductNo

                });
                await _dbContext.SaveChangesAsync();

            }
        }
        public async Task<ApplicationRole> AddRole(string[] Permissions)
        {
            string RoleName = "管理员";
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
        public async Task AddPermissions()
        {
            var user_manage = new Permissions() { Code = "user_manage", Name = "用户管理", IsMenu = true, ParentId = 0 };
            var user_list = new Permissions() { Code = "user_list", Name = "用户列表", IsMenu = true, ParentId = user_manage.Id };
            var user_edit = new Permissions() { Code = "user_edit", Name = "用户编辑", IsMenu = true, ParentId = user_manage.Id };


            await AddPermissions(user_manage);

            user_list.ParentId = user_manage.Id;
            await AddPermissions(user_list);

            user_edit.ParentId = user_manage.Id;
            await AddPermissions(user_edit);



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
