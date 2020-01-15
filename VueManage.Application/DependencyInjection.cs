using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using VueManage.Domain.Entities;
using AutoMapper;
using VueManage.Application.Permissionses;
using AutoMapper.Configuration;
using VueManage.Application.Users;

namespace VueManage.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

             AutoMapperConfig.RegisterMappings();

            services.AddScoped<PermissionsQuery>();
            services.AddScoped<UserQuery>();
        }
    }
}
