using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Entity.Model
{
    public class RoleEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid GUID { get; set; }

        public string Name { get; set; }

        public string Describe { get; set; }

        public virtual ICollection<AuthorityEntity> AuthorityList { get; set; }

        public virtual ICollection<UserGrantEntity> UserGrantList { get; set; }
    }
}
