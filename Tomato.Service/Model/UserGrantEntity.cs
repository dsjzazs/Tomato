using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Service.Model
{
    public class UserGrantEntity
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 角色列表
        /// </summary>
        public virtual ICollection<RoleEntity> RoleList { get; set; }
    }
}
