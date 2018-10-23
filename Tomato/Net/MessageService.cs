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
        public static MessageService Instance { get; } = new MessageService();
        public event EventHandler<RequestEventArgs> RequestEvent;
        private List<NetMQ.Sockets.ResponseSocket> _sockList = new List<NetMQ.Sockets.ResponseSocket>();
        private List<NetMQ.NetMQPoller> _pollerList = new List<NetMQ.NetMQPoller>();
        public bool IsRunning { get; private set; }
        /// <summary>
        /// 启动服务,开始接收消息
        /// </summary>
        /// <param name="Consumer">消费者数量</param>
        /// <returns></returns>
        public bool StartService(int Consumer)
        {
            if (IsRunning)
                return false;

            for (int i = 0; i < Consumer; i++)
            {
                NetMQ.Sockets.ResponseSocket response = new NetMQ.Sockets.ResponseSocket();
                response.Connect(Broker.WorkerAddress);
                response.ReceiveReady += Response_ReceiveReady;
                /* 修正客户端无法异步请求的问题
                 * 每一个REP必须使用独立的一条线程
                 * 当REP收到请求之后,只有回复以后才能接受下一个请求
                 * 如果所有Consumer(REP)都正在工作,则其他请求就会挂起
                 * 所以最大同时请求数量受Consumer的影响
                 */
                var poller = new NetMQPoller() { response, new NetMQTimer(Timeout) };
                poller.RunAsync();
                _pollerList.Add(poller);
                _sockList.Add(response);
            }
            return IsRunning = true;
        }
        /// <summary>
        /// 停止消息接收服务
        /// </summary>
        /// <returns></returns>
        public bool StopService()
        {
            if (IsRunning == false)
                return false;

            for (int i = 0; i < _sockList.Count; i++)
            {
                _sockList[i].Dispose();
                _pollerList[i].StopAsync();
            }
            return true;
        }
        private void ReceiveMessage(Header header, byte[] BodyBytes, NetMQSocket Socket)
        {
            using (var dbContext = new Tomato.Model.Model())
            {
                Model.Session session = null;
                if (header.Session != null)
                    session = dbContext.SessionDB.FirstOrDefault(j => j.GUID == header.Session && j.ExpirationTime <= DateTime.Now);
                if (session != null)
                    session.ExpirationTime = DateTime.Now.AddHours(1);//刷新session有效期

                var context = Context.CreateContext(session?.User, header, dbContext, Socket);
                var eventArgs = new RequestEventArgs() { Context = context };
                RequestEvent?.Invoke(this, eventArgs);
                if (eventArgs.Cancel == true)
                    return;
                //交给注册的委托
                var handle = MessageHandle.Instance.GetHandle(context.Header.MessageType);
                if (handle != null)
                    handle.Invoke(context, BodyBytes);//全程务必保持由当前线程同步执行
                else
                    Console.WriteLine($"未注册的委托 : {context.Header.MessageType}");
                dbContext.SaveChanges();
            }
        }
        /// <summary>
        /// 消息接收事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Response_ReceiveReady(object sender, NetMQSocketEventArgs e)
        {
            NetMQMessage mqMsg = null;
            if (e.Socket.TryReceiveMultipartMessage(Timeout, ref mqMsg))
            {
                var messageType = (Protocol.ProtoEnum)mqMsg[0].ConvertToInt32();
                var HeaderBytes = mqMsg[1].Buffer;
                var BodyBytes = mqMsg[2].Buffer;
                Header header;
                using (var ms = new System.IO.MemoryStream(HeaderBytes))
                    header = ProtoBuf.Serializer.Deserialize<Header>(ms);
                header.MessageType = messageType;
                this.ReceiveMessage(header, BodyBytes, e.Socket);
            }
        }
    }

    public class RequestEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
        public Context Context { get; set; }
    }
}