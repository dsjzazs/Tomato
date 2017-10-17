using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
namespace Tomato.Net
{
    public class MessageService
    {
        /// <summary>
        /// 发送和接收超时
        /// </summary>
        public static TimeSpan Timeout { get; set; } = TimeSpan.FromMilliseconds(1000);
        private MessageService() { }
        private static NetMQ.NetMQPoller Poller { get; } = new NetMQPoller();
        public static MessageService Instance { get; } = new MessageService();
        private List<NetMQ.Sockets.ResponseSocket> _sockList = new List<NetMQ.Sockets.ResponseSocket>();
        public bool IsRuning { get { return Poller.IsRunning; } }
        /// <summary>
        /// 启动服务,开始接收消息
        /// </summary>
        /// <param name="Consumer">消费者数量</param>
        /// <returns></returns>
        public bool StartService(int Consumer)
        {
            if (Poller.IsRunning)
                return false;

            for (int i = 0; i < Consumer; i++)
            {
                NetMQ.Sockets.ResponseSocket response = new NetMQ.Sockets.ResponseSocket();
                response.Connect(Broker.WorkerAddress);
                response.ReceiveReady += Response_ReceiveReady;
                Poller.Add(response);
                _sockList.Add(response);
            }
            Poller.Add(new NetMQTimer(Timeout));
            Poller.RunAsync();
            return Poller.IsRunning;
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        /// <returns></returns>
        public bool StopService()
        {
            if (Poller.IsRunning == false)
                return false;
            foreach (var item in _sockList)
            {
                Poller.Remove(item);
                item.Dispose();
            }
            Poller.StopAsync();
            return true;
        }
        /// <summary>
        /// 消息接收事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Response_ReceiveReady(object sender, NetMQSocketEventArgs e)
        {
            List<byte[]> bytes = null;
            if (e.Socket.TryReceiveMultipartBytes(Timeout, ref bytes))
            {
                var messageType = (Protocol.ProtoEnum)BitConverter.ToUInt32(bytes[0], 0);
                var HeaderBytes = bytes[1];
                var BodyBytes = bytes[2];
                Header header;
                using (var ms = new System.IO.MemoryStream(HeaderBytes))
                    header = ProtoBuf.Serializer.Deserialize<Header>(ms);
                header.MessageType = messageType;
                var context = Context.Create(User.Create(), header, e.Socket);
                //交给注册的委托
                var handle = MessageHandle.Instance.GetHandle(context.Header.MessageType);
                if (handle != null)
                    handle.Invoke(context, BodyBytes);//全程务必保持由当前线程同步执行
                else
                    Console.WriteLine($"未注册的委托 : {context.Header.MessageType}");
            }
        }
    }
}