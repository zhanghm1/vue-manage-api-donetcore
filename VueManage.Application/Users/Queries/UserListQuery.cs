using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VueManage.Domain;
using VueManage.Domain.Base;
using VueManage.Domain.Entities;

namespace VueManage.Application.Users.Queries
{
    public class UserListQuery : PageRequest, IRequest<PageResponse<ApplicationUser>>
    {

    }

}
