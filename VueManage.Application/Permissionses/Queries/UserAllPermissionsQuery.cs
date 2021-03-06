﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VueManage.Application.Permissionses.Queries;
using VueManage.Domain.Base;
using VueManage.Domain.Entities;
using VueManage.Infrastructure.EFCore;

namespace VueManage.Application.Permissionses
{
    public class UserAllPermissionsQuery : IRequest<List<UserAllPermissionsQueryResponse>>
    {
        public int UserId { get; set; }
    }
    public class UserAllPermissionsQueryResponse: PermissionsTreeQueryResponse
    {

    }
}
