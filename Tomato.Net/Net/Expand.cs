using NetMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Net
{
    public static class Expand
    {
        public static void Response<T>(this Context context, T body) where T : IProtocol
        {
            var header = new Header
            {
                GUID = context.Header.GUID,
                IsResponse = true,
                Session = context.Header.Session,
                MessageType = body.MessageType,
            };
            context.Socket.Response(header, body);
        }
        public static void Response<T>(this NetMQ.NetMQSocket socket, Header header, T body) where T : IProtocol
        {
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
            socket.TrySendMultipartMessage(TimeSpan.FromSeconds(1), mqms);
        }
    }
}
