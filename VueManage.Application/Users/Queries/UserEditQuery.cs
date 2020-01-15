using System;
using System.Collections.Generic;
using System.Text;
using VueManage.Domain.Entities;

namespace VueManage.Application.Users.Queries
{
    public class UserEditQuery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }

        public List<int> UserRoleIds { get; set; }

        public List<UserEditRole> AllRoles { get; set; }
    }
    public class UserEditRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
