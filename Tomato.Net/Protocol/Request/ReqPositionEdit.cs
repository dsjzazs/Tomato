using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Protocol;

namespace Tomato.Protocol.Request
{
    public class ReqPositionEdit : Net.IProtocol
    {
        /// <summary>
        /// 职位名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 职位描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 上级职位
        /// </summary>
        public string SuperiorName { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        public ProtoEnum MessageType => throw new NotImplementedException();
    }
}
