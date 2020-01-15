using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VueManage.Application.Users.Queries;
using VueManage.Domain;
using VueManage.Domain.Base;
using VueManage.Domain.Entities;
using VueManage.Infrastructure.EFCore;

namespace VueManage.Application.Users
{

    public class UserQuery : IRequestHandler<UserListQuery, PageResponse<ApplicationUser>>
    {
        private readonly IRepository<ApplicationRole> _roleRepository;

        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public UserQuery(IRepository<ApplicationUser> userRepository, IRepository<ApplicationRole> roleRepository,
            IMapper mapper, ApplicationDbContext dbContext
            )
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PageResponse<ApplicationUser>> Handle(UserListQuery request, CancellationToken cancellationToken)
        {
            var list = await _userRepository.PageListAsync(request, a => a.Id > 0, a => a.Id, false);

            return list;
        }
        public async Task<UserEditQuery> GetEditUser(int Id)
        {
            var user = await _userRepository.FindAsync(a => a.Id == Id);
            var resp = _mapper.Map<UserEditQuery>(user);
            resp.UserRoleIds =  _dbContext.UserRoles.Where(a => a.UserId == Id).Select(a => a.RoleId).ToList();
            resp.Password = user.PasswordHash;
            var roles = await _roleRepository.ListAsync(a => a.Id > 0);

            resp.AllRoles = _mapper.Map<List<UserEditRole>>(roles);


            return resp;
        }
    }
}
