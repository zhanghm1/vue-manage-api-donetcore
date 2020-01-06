using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueManage.Domain.Entities;

namespace VueManage.Api.Models.Users
{
    public class UserInfoResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public UserInfoResponse GetUserInfo(ApplicationUser user)
        {
            if (user==null)
            {
                return this;
            }
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            Name = user.Name;
            return this;
        }

    }
}
