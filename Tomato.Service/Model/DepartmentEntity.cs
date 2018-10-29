using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Service.Model
{
    public class DepartmentEntity
    {
        [Key]
        public int Id { get; set; }

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
        public int SuperiorId { get; set; }

        /// <summary>
        /// 公司名
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 部门职位
        /// </summary>
        public virtual ICollection<PositionEntity> PositionList { get; set; }
    }
}
