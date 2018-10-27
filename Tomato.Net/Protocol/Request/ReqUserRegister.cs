using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Net;

namespace Tomato.Protocol
{
    [ProtoBuf.ProtoContract]
    public class ReqUserRegister : IProtocol
    {
        [ProtoBuf.ProtoMember(1)]
        public string UserName { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public string Password { get; set; }
        [ProtoBuf.ProtoMember(3)]
        public string NickName { get; set; }
        [ProtoBuf.ProtoMember(4)]
        public string Email { get; set; }
        [ProtoBuf.ProtoMember(5)]
        public GenderEnum Gender { get; set; }
        public ProtoEnum MessageType => ProtoEnum.RegisterRequest;
    }
}
