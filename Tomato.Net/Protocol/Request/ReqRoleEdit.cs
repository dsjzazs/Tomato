using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Protocol.Enum;

namespace Tomato.Protocol.Request
{
    /// <summary>
    /// 角色添加--如用户,部门,权限
    /// </summary>
    public class ReqRoleEdit
    {
        public RoleEditTypeEnum RoleEditType { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 添加类型ID
        /// </summary>
        public int FromId { get; set; }
    }
}
