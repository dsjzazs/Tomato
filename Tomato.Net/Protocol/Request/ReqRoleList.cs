using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Net.Protocol.Request
{
    [ProtoBuf.ProtoContract]
    public class ReqRoleList : IProtocol
    {
        [ProtoBuf.ProtoMember(1)]
        public string Rule { get; set; }

        public ProtoEnum MessageType => throw new NotImplementedException();
    }
}
