using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Entity.Model
{
    /// <summary>
    /// 权限表
    /// </summary>
    public class AuthorityEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid GUID { get; set; }

        public int AuthorityId { get; set; }

        public string Name { get; set; }

        public string Describe { get; set; }
    }
}
