using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Net.Protocol.Response
{
    [ProtoBuf.ProtoContract]
    public class ResDepartmentList : Net.IProtocol
    {
        [ProtoBuf.ProtoMember(1)]
        public bool Success { get; set; }

        [ProtoBuf.ProtoMember(2)]
        public string Message { get; set; }

        public ProtoEnum MessageType => throw new NotImplementedException();

        [ProtoBuf.ProtoMember(3)]
        public int Count { get; set; }

        [ProtoBuf.ProtoMember(4)]
        public List<ResDepartment> List { get; set; }

    }
}
