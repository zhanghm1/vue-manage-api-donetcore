using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VueManage.Domain.Base;
using VueManage.Domain.Entities;
using VueManage.Infrastructure.EFCore;

namespace VueManage.Application.Permissionses
{
    public class EditRoleCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public List<int> PermissionsIds { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
