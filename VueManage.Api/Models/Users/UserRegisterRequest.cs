using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VueManage.Api.Models.Accounts
{
    public class UserRegisterRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public IdentityUser ToEntity()
        {
            return new IdentityUser() {
                UserName= UserName,
                
            };
        }
    }
}
