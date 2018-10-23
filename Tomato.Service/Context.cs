using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
namespace Tomato.Net
{
    public class Context
    {
        internal static Context CreateContext(Model.IUser user, Header header, Model.Model dbContext, NetMQ.NetMQSocket socket)
        {
            return new Context()
            {
                Header = header,
                User = user,
                Socket = socket,
                DbContext = dbContext
            };
        }
        public Model.Model DbContext { get; private set; }
        public Header Header { get; private set; }
        public Model.IUser User { get; private set; }
        private NetMQ.NetMQSocket Socket;
        private bool _answer;
        public bool Response<T>(T body) where T : IProtocol
        {
            var header = new Header
            {
                GUID = Header.GUID,
                IsResponse = true,
                Session = Header.Session,
                MessageType = body.MessageType,
            };
            return this.Response(header, body);
        }
        public bool Response<T>(Header header, T body) where T : IProtocol
        {
            if (_answer)
                throw new Exception("该请求只能响应一次消息!");

            NetMQ.NetMQMessage mqms = new NetMQMessage();
            header.SendTime = DateTime.Now;
            //数据包类型
            mqms.Append(new NetMQFrame(BitConverter.GetBytes((UInt32)body.MessageType)));
            using (var ms = new System.IO.MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(ms, header);//数据包头
                mqms.Append(ms.ToArray());
            }
            using (var ms2 = new System.IO.MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(ms2, body);//对象
                mqms.Append(ms2.ToArray());
            }
            //结束帧
            mqms.Append(NetMQFrame.Empty);
            return _answer = Socket.TrySendMultipartMessage(TimeSpan.FromSeconds(1), mqms);
        }
    }
}
