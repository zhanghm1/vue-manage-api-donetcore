using System;
using System.Collections.Generic;
using System.Text;

namespace VueManage.Domain.Base
{
    public interface IEntityBase
    {
        public int Id { get; set; }
    }
    public abstract class EntityBase:IEntityBase
    {
        public int Id { get; set; }
    }
}
