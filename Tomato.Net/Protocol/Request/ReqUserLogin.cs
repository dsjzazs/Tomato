using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Protocol.Request
{
    /// <summary>
    /// 登陆请求
    /// </summary>
    [ProtoBuf.ProtoContract]
    public class ReqUserLogin : Net.IProtocol
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
        public string PassWord { get; set; }

        public ProtoEnum MessageType => ProtoEnum.LoginRequest;
    }
}
