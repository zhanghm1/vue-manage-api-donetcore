using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VueManage.Application.Permissionses;
using VueManage.Application.Permissionses.Queries;
using VueManage.Domain;
using VueManage.Domain.Entities;

namespace VueManage.Application
{
    public class PermissionsMappingProfile : Profile
    {
        public PermissionsMappingProfile()
        {
            CreateMap<ApplicationRole, RoleListResponse>();
            CreateMap<ApplicationRole, RoleEditResponse>();

            CreateMap<PermissionsTreeQueryResponse, UserAllPermissionsQueryResponse>();
        }
    }
}