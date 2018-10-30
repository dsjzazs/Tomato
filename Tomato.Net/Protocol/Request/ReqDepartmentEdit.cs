using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Net.Protocol;

namespace Tomato.Net.Protocol.Request
{
    [ProtoBuf.ProtoContract]
    public class ReqDepartmentEdit : Net.IProtocol
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        [ProtoBuf.ProtoMember(1)]
        public string Name { get; set; }

        /// <summary>
        /// 部门描述
        /// </summary>
        [ProtoBuf.ProtoMember(2)]
        public string Describe { get; set; }

        /// <summary>
        /// 上级部门
        /// </summary>
        [ProtoBuf.ProtoMember(3)]
        public string SuperiorName { get; set; }

        public ProtoEnum MessageType => ProtoEnum.ReqDepartmentEdit;
    }
}
