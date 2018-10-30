using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Net.Protocol.Request
{
    /// <summary>
    /// 请求职位列表
    /// </summary>
    [ProtoBuf.ProtoContract]
    public class ReqPositionList : IProtocol
    {
        [ProtoBuf.ProtoMember(1)]
        public string Rule { get; set; }

        public ProtoEnum MessageType => throw new NotImplementedException();
    }
}
