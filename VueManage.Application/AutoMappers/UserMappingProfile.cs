using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VueManage.Application.Permissionses;
using VueManage.Application.Permissionses.Queries;
using VueManage.Application.Users.Queries;
using VueManage.Domain;
using VueManage.Domain.Entities;

namespace VueManage.Application
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<ApplicationUser, UserEditQuery>();
            CreateMap<ApplicationRole, UserEditRole>();
        }
    }
}