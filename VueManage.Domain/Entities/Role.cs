using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using VueManage.Domain.Base;

namespace VueManage.Domain.Entities
{
    public class ApplicationRole : IdentityRole<int>, IEntityBase
    {
        public string Description { get; set; }
    }
}
