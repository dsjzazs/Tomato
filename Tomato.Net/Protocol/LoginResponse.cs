using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Net;
using Tomato.Net.Protocol;

namespace Tomato.Protocol
{
    /// <summary>
    /// 登陆响应
    /// </summary>
    [ProtoBuf.ProtoContract]
    public class LoginResponse : IProtocol
    {
        /// <summary>
        /// 登陆否成功
        /// </summary>
        [ProtoBuf.ProtoMember(1)]
        public bool Success { get; set; }
        /// <summary>
        /// 登陆令牌
        /// </summary>
        [ProtoBuf.ProtoMember(2)]
        public Guid Session { get; set; }
        /// <summary>
        /// 系统信息
        /// </summary>
        [ProtoBuf.ProtoMember(3)]
        public string Message { get; set; }
        public ProtoEnum MessageType => ProtoEnum.LoginResponse;
    }
}
