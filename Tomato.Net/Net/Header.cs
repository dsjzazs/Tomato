using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Net
{
    [ProtoBuf.ProtoContract]
    public class Header
    {
        /// <summary>
        /// 消息唯一ID
        /// </summary>
        [ProtoBuf.ProtoMember(1)]
        public Guid GUID { get; set; }
        /// <summary>
        /// 消息发送时间
        /// </summary>
        [ProtoBuf.ProtoMember(2)]
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 身份唯一ID
        /// </summary>
        [ProtoBuf.ProtoMember(3)]

        public Guid Session { get;  set; }
        /// <summary>
        /// 是否为响应包
        /// </summary>
        [ProtoBuf.ProtoMember(4)]
        public bool IsResponse { get; set; }

        public Tomato.Net.Protocol.ProtoEnum MessageType { get; set; }

    }
}
