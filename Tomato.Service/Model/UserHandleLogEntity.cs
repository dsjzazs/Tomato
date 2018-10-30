using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Net.Protocol;

namespace Tomato.Service.Model
{
    /// <summary>
    /// 用户操作信息表
    /// </summary>
    public class UserHandleLogEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid GUID { get; set; }

        public string UserName { get; set; }

        public ModularEnum Modular { get; set; }

        public ProtoEnum Proto { get; set; }

        public string Describe { get; set; }

        public DateTime? HandleTime { get; set; }
    }
}
