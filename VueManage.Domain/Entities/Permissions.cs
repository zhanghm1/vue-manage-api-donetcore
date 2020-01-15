using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VueManage.Domain.Base;

namespace VueManage.Domain.Entities
{
    /// <summary>
    /// 权限表
    /// 此表的数据主键应保持和Code对应不变 一个Code终身只能有一个对应的ID，
    /// </summary>
    public class Permissions : IEntityBase, IEntityDeletedBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        /// <summary>
        /// 是否菜单
        /// </summary>
        public bool IsMenu { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ParentId { get; set; }
        
        public bool IsDeleted { get; set ; }
    }
}
