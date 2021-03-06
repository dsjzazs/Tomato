﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Protocol
{
    [ProtoBuf.ProtoContract]
    public class ReqRegisterService
    {
        [ProtoBuf.ProtoMember(1)]
        public bool IsRegister { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public List<int> ProtocolList { get; set; } = new List<int>();
        [ProtoBuf.ProtoMember(3)]
        public string ServiceName { get; set; }
    }
    [ProtoBuf.ProtoContract]
    public class ResRegisterService
    {
        [ProtoBuf.ProtoMember(1)] public int Port { get; set; }
        [ProtoBuf.ProtoMember(2)] public bool Success { get; set; }
        [ProtoBuf.ProtoMember(3)] public string Message { get; set; }
    }
}
