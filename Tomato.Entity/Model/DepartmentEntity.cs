using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Entity.Model
{
    public class DepartmentEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid GUID { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 上级部门ID
        /// </summary>
        public string SuperiorName { get; set; }

        /// <summary>
        /// 公司名
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 部门职位集合
        /// </summary>
        public virtual ICollection<PositionEntity> PositionList { get; set; }
    }
}
