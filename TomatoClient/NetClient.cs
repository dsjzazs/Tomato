using NetMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Net;

namespace TomatoClient
{
    public class NetClient
    {
        private NetClient() { }
        private Guid userSession;
        /// <summary>
        /// 发送和接收超时
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromMilliseconds(5000);
        public static NetClient Instance { get; } = new NetClient();
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="R">内容对象</typeparam>
        /// <param name="msg">消息体</param>
        /// <returns></returns>
        private R Deserialize<R>(NetMQMessage msg) where R : IProtocol
        {
            var messageType = (Tomato.Protocol.ProtoEnum)msg[0].ConvertToInt32();//对象类型
            var HeaderBytes = msg[1].ToByteArray();//头bytes
            var BodyBytes = msg[2].ToByteArray();//对象bytes
            Header header2;
            using (var ms = new System.IO.MemoryStream(HeaderBytes))
                header2 = ProtoBuf.Serializer.Deserialize<Header>(ms);
            header2.MessageType = messageType;
            using (var ms = new System.IO.MemoryStream(BodyBytes))
                return ProtoBuf.Serializer.Deserialize<R>(ms);
        }
        /// <summary>
        /// 序列化为NetMQMessage对象
        /// </summary>
        /// <param name="header">头部对象</param>
        /// <param name="body">内容对象</param>
        /// <returns></returns>
        private NetMQMessage Serialize(Header header, IProtocol body)
        {
            NetMQ.NetMQMessage mqms = new NetMQMessage();
            header.SendTime = DateTime.Now;
            //对象类型
            mqms.Append((Int32)body.MessageType);
            using (var ms = new System.IO.MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(ms, header);
                //头bytes
                mqms.Append(ms.ToArray());
            }
            using (var ms2 = new System.IO.MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(ms2, body);
                //对象
                mqms.Append(ms2.ToArray());
            }
            //结束帧
            mqms.AppendEmptyFrame();
            return mqms;
        }
        /// <summary>
        /// 发送请求
        /// </summary>
        /// <typeparam name="R">接收对象类型</typeparam>
        /// <param name="body">内容对象</param>
        /// <returns></returns>
        public Task<R> Request<R>(IProtocol body) where R : IProtocol
        {
            return Request<R>(new Header()
            {
                Session = userSession,
                GUID = Guid.NewGuid(),
                MessageType = body.MessageType,
                IsResponse = false,
                SendTime = DateTime.Now
            }, body);
        }
        /// <summary>
        /// 发送请求
        /// </summary>
        /// <typeparam name="R">接收对象类型</typeparam>
        /// <param name="header">头部对象</param>
        /// <param name="body">内容对象</param>
        /// <returns></returns>
        public Task<R> Request<R>(Header header, IProtocol body) where R : IProtocol
        {
            return Task.Factory.StartNew<R>(() =>
            {
                var message = Serialize(header, body);
                using (var socket = new NetMQ.Sockets.RequestSocket(@"tcp://localhost:6666"))
                {
                    socket.TrySendMultipartMessage(Timeout, message);//发送msg
                    NetMQMessage resMessage = new NetMQMessage();
                    socket.TryReceiveMultipartMessage(Timeout, ref resMessage);//接收msg
                    return Deserialize<R>(resMessage);
                }
            });
        }

    }
}
