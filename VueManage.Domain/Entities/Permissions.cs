using System;
using System.Collections.Generic;
using System.Text;
using VueManage.Domain.Base;

namespace VueManage.Domain.Entities
{
    public class Permissions : EntityBase
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsMenu { get; set; }

        public int ParentId { get; set; }
    }
}
