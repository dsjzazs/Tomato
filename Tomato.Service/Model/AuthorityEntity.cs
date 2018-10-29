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

        public int AuthorityId { get; set; }

        public string Name { get; set; }

        public string Describe { get; set; }
    }
}
