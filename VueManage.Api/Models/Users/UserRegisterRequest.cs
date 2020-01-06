using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueManage.Domain.Entities;

namespace VueManage.Api.Models.Accounts
{
    public class UserRegisterRequest
    {
        public string UserName { get; set; }
        public string Name { get; set; }

        public string Password { get; set; }

        public ApplicationUser ToEntity()
        {
            return new ApplicationUser() {
                UserName= UserName,
                Name=Name
            };
        }
    }
}
