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
            string UserName = "admin";
            string password = "Admin123456!";

            if (!_dbContext.Users.Any(a=>a.UserName== UserName))
            {
               var result = await _userManager.CreateAsync(new ApplicationUser()
                {
                    UserName= UserName,
                    Name= UserName
                }, password);
                if (!result.Succeeded)
                { 
                    //log
                }

            }
            
        }
    }
}
