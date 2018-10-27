using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
namespace Tomato.Net
{
    /// <summary>
    /// 服务会话的简单上下文对象
    /// </summary>
    public class Context
    {
        internal static Context CreateContext(Model.IUser user, Header header, Model.EntityModel dbContext, NetMQ.NetMQSocket socket)
        {
            return new Context()
            {
                Header = header,
                User = user,
                Socket = socket,
                DbContext = dbContext
            };
        }
        public TimeSpan Timeout { get; set; } = TimeSpan.FromMilliseconds(1000);
        public Model.EntityModel DbContext { get; private set; }
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

            if (body == null)
                throw new ArgumentNullException("body is Null !!");

            if (header == null)
                throw new ArgumentNullException("header is Null !!");

            if (_answer)
                throw new Exception("该请求只能响应一次消息!");


            NetMQ.NetMQMessage mqms = new NetMQMessage();
            header.SendTime = DateTime.Now;
            //数据包类型
            mqms.Append((int)body.MessageType);
            using (var ms = new System.IO.MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(ms, header);//数据包头
                mqms.Append(ms.ToArray());
            }
            using (var ms2 = new System.IO.MemoryStream())
            {
#if DEBUG
                Console.WriteLine("Header:");
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(header));
                Console.WriteLine("Body:");
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(body));
                Console.WriteLine("------------------------------------------");
#endif
                ProtoBuf.Serializer.Serialize(ms2, body);//对象
                mqms.Append(ms2.ToArray());
            }
            //结束帧
            mqms.AppendEmptyFrame();
            return _answer = Socket.TrySendMultipartMessage(Timeout, mqms);
        }
    }
}
