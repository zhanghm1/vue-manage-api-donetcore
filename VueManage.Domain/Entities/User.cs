﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using VueManage.Domain.Base;

namespace VueManage.Domain.Entities
{
    public class ApplicationUser : IdentityUser<int>, IEntityBase
    {
        public string Name { get; set; }

    }
}
