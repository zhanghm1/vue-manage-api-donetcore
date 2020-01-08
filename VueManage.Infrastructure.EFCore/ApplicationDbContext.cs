using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueManage.Domain.Entities;

namespace VueManage.Infrastructure.EFCore
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole,int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Product { get; set; }
        public DbSet<UserOrder> UserOrder { get; set; }
        public DbSet<UserOrderItem> UserOrderItem { get; set; }
    }
}
