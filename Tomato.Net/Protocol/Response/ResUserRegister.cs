using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Protocol
{
    [ProtoBuf.ProtoContract]
    public class ResUserRegister : Net.IProtocol
    {
        [ProtoBuf.ProtoMember(1)]
        public bool Success { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public string Message { get; set; }
        [ProtoBuf.ProtoMember(3)]
        public Guid Session { get; set; }
        public ProtoEnum MessageType => ProtoEnum.RegisterResponse;
    }
}
