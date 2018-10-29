using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Service.Model
{
    public class PositionEntity
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 上级
        /// </summary>
        public int SuperiorId { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        public DepartmentEntity Department { get; set; }

        /// <summary>
        /// 权限集合
        /// </summary>
        public ICollection<AuthorityEntity> AuthorityList { get; set; }
    }
}
