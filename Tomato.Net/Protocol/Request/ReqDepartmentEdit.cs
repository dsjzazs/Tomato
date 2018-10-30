using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Protocol;

namespace Tomato.Protocol.Request
{
    public class ReqDepartmentEdit : Net.IProtocol
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 部门描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 上级部门
        /// </summary>
        public string SuperiorName { get; set; }

        public ProtoEnum MessageType => throw new NotImplementedException();
    }
}
