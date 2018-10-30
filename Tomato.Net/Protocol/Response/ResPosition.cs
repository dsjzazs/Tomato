using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Net.Protocol.Response
{
    /// <summary>
    /// 职位信息
    /// </summary>
    [ProtoBuf.ProtoContract]
    public class ResPosition :Net.IProtocol
    {
        [ProtoBuf.ProtoMember(1)]
        public bool Success { get; set; }

        [ProtoBuf.ProtoMember(2)]
        public string Message { get; set; }

        public ProtoEnum MessageType => throw new NotImplementedException();

        [ProtoBuf.ProtoMember(3)]
        public string GUID { get; set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        [ProtoBuf.ProtoMember(4)]
        public string Name { get; set; }

        /// <summary>
        /// 职位描述
        /// </summary>
        [ProtoBuf.ProtoMember(5)]
        public string Describe { get; set; }

        /// <summary>
        /// 上级
        /// </summary>
        [ProtoBuf.ProtoMember(6)]
        public string SuperiorName { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [ProtoBuf.ProtoMember(7)]
        public string DepartmentName { get; set; }
    }
}
