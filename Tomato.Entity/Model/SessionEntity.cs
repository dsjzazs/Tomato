using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Entity.Model
{
   public class SessionEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid GUID { get; set; }
        /// <summary>
        /// 验证登录
        /// </summary>
        public bool Verified { get; set; }
        /// <summary>
        /// 验证时间
        /// </summary>
        public DateTime? VerifiedTime { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? ExpirationTime { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        public virtual UserEntity User { get; set; }
    }
}
