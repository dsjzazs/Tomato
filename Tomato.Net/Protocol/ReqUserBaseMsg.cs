using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Tomato.Net.Protocol
{
    [ProtoContract]
    public class ReqUserBaseMsg
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        [ProtoMember(1)]
        public Guid Session { get; set; }
    }
}
