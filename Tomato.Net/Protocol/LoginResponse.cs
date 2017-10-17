using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Net;
using Tomato.Protocol;

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
    /// <summary>
    /// 登陆请求
    /// </summary>
    [ProtoBuf.ProtoContract]
    public class LoginRequest : IProtocol
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        [ProtoBuf.ProtoMember(1)]
        public string UserName { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        [ProtoBuf.ProtoMember(2)]
        public string PassWrod { get; set; }

        public ProtoEnum MessageType => ProtoEnum.LoginRequest;
    }
}
