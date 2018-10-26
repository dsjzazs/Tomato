using NetMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Net;

namespace Tomato.Client
{
    public class NetClient
    {
        private NetClient() { }
        public Guid Session { get; set; } = Guid.Empty;
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
                Session = Session,
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
                var req_msg = Serialize(header, body);
                using (var socket = new NetMQ.Sockets.RequestSocket(ServerAddress.RouterAddress))
                {
                    if (socket.TrySendMultipartMessage(Timeout, req_msg))//发送msg
                    {
                        NetMQMessage rep_msg = new NetMQMessage();
                        if (socket.TryReceiveMultipartMessage(Timeout, ref rep_msg))//接收msg
                        {
                            var error = rep_msg[0].ConvertToInt32();
                            if (error >= 800000 && error <= 899999)
                                throw new RemoteServiceException($"错误代码 : {error}\r\n{rep_msg[1].ConvertToString(Encoding.UTF8)}");
                            return Deserialize<R>(rep_msg);
                        }
                        else
                            throw new NetMQ.EndpointNotFoundException($"Request Guid : {header.GUID}.消息接收失败");
                    }
                    else
                        throw new NetMQ.EndpointNotFoundException($"Request Guid : {header.GUID}.消息发送失败");
                }
            });
        }
    }
}
