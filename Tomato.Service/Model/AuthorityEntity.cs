using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Service.Model
{
    /// <summary>
    /// 权限表
    /// </summary>
    public class AuthorityEntity
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 模块
        /// </summary>
        public ModularEntity Modular { get; set; }

    }
}
