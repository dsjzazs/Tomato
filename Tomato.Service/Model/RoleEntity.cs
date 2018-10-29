using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Service.Model
{
    public class RoleEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Describe { get; set; }

        public virtual ICollection<AuthorityEntity> AuthorityList { get; set; }

        public virtual ICollection<UserGrantEntity> UserGrantList { get; set; }
    }
}
