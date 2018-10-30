using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Net.Protocol.Response
{
    /// <summary>
    /// 返回部门
    /// </summary>
    [ProtoBuf.ProtoContract]
    public class ResDepartment : IProtocol
    {
        [ProtoBuf.ProtoMember(1)]
        public bool Success { get; set; }

        [ProtoBuf.ProtoMember(2)]
        public string Message { get; set; }

        public ProtoEnum MessageType => ProtoEnum.ResDepartment;

        [ProtoBuf.ProtoMember(3)]
        public string GUID { get; set; }

        [ProtoBuf.ProtoMember(4)]

        public string Name { get; set; }

        [ProtoBuf.ProtoMember(5)]

        public string Describe { get; set; }

        [ProtoBuf.ProtoMember(6)]

        public string SuperiorName { get; set; }

        [ProtoBuf.ProtoMember(7)]

        public string CompanyName { get; set; }

    }
}
