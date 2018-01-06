using System;
using System.Data.Entity;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tomato.Model
{
    public class Session
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
        public virtual UserInfo User { get; set; }
    }
}
