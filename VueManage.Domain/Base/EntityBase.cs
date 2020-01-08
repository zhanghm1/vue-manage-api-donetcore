using System;
using System.Collections.Generic;
using System.Text;

namespace VueManage.Domain.Base
{
    public interface IEntityBase
    {
        public int Id { get; set; }
    }
    public interface IEntityDeletedBase
    {
        public bool IsDeleted { get; set; }
    }

    public abstract class EntityBase : IEntityBase, IEntityDeletedBase
    {
        public int Id { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}
