using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Entity.Model
{
    public class UserHandleLogEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid GUID { get; set; }

        public string UserName { get; set; }

        public int Modular { get; set; }

        public int Proto { get; set; }

        public string Describe { get; set; }

        public DateTime? HandleTime { get; set; }
    }
}
