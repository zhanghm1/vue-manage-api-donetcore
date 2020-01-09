using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using VueManage.Domain.Base;
using VueManage.Domain.Entities;

namespace VueManage.Infrastructure.EFCore
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureEFCore(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                   options.UseSqlServer(
                       Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ApplicationDbContentSeed>();

            services.AddScoped(typeof(IRepository<>),typeof(EFRepository<>));


            // 使用 AddIdentityCore  添加核心功能，其他功能需要手动添加,登录用的自定义，没有用SignManage,
            // 如果用 AddIdentity   添加全部功能 Sign等 
             services.AddIdentityCore<ApplicationUser>()
            //services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //UserStoreBase<ApplicationUser, ApplicationRole,int,IdentityUserClaim<int>,IdentityUserRole<int>, IdentityUserLogin<int>,IdentityUserToken<int>,IdentityRoleClaim<int>
        }
    }
}
