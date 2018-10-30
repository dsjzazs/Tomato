using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
using Tomato.Net;
using Tomato.Net.Protocol;
using Tomato.Protocol;
using Tomato.Service.Model;

namespace Tomato
{
    /// <summary>
    /// 服务模块基类,所有服务模块必须继承
    /// </summary>
    public abstract class ServiceBase
    {
        /// <summary>
        /// 发送和接收超时
        /// </summary>
        public TimeSpan Timeout { get; set; }
        /// <summary>
        /// 消息请求事件
        /// </summary>
        public event EventHandler<RequestEventArgs> RequestEvent;
        private List<NetMQ.Sockets.ResponseSocket> _sockList;
        private List<NetMQ.NetMQPoller> _pollerList;
        /// <summary>
        /// 消息回调
        /// </summary>
        public MessageHandle MessageHandle { get; }
        /// <summary>
        /// 服务名称
        /// </summary>
        public abstract string ServiceName { get; }
        /// <summary>
        /// 服务是否已运行
        /// </summary>
        public bool IsRunning { get; private set; }
        private log4net.ILog Logger;
        public ServiceBase()
        {
            this.Timeout = TimeSpan.FromMilliseconds(1000);
            this._sockList = new List<NetMQ.Sockets.ResponseSocket>();
            this._pollerList = new List<NetMQ.NetMQPoller>();
            this.MessageHandle = new MessageHandle();
            this.Logger = log4net.LogManager.GetLogger(ServiceName);
        }
        /// <summary>
        /// 初始化服务模块
        /// 在这里,我们将已注册的protocolID发送到路由端 Tomato.RouteService
        /// </summary>
        /// <returns></returns>
        private ResRegisterService _InitializeService(bool isRegister = true)
        {
            Logger.Debug($"{this.ServiceName} 正在向路由注册模块...");
            NetMQ.Sockets.RequestSocket req = new NetMQ.Sockets.RequestSocket();
            req.Connect(ServerAddress.RegisterServiceAddress);
            using (var stream = new System.IO.MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(stream, new ReqRegisterService()
                {
                    ServiceName = ServiceName,
                    ProtocolList = MessageHandle.DicHandles.Keys.Select(i => (int)i).ToList(),
                    IsRegister = isRegister,
                });
                if (req.TrySendFrame(stream.ToArray()))
                    return ProtoBuf.Serializer.Deserialize<ResRegisterService>(new System.IO.MemoryStream(req.ReceiveFrameBytes()));
            }
            return null;
        }
        /// <summary>
        /// 启动服务,开始接收消息
        /// </summary>
        /// <param name="Consumer">线程数</param>
        /// <returns></returns>
        public bool StartService(int Consumer)
        {
            if (IsRunning)
                return false;

            Logger.Debug($"{this.ServiceName} 启用线程数量:{Consumer}");
            //向路由注册服务
            var initial_res = _InitializeService();
            if (initial_res?.Success == true)
            {
                Logger.Info($"{this.ServiceName} 服务模块注册成功");
                for (int i = 0; i < Consumer; i++)
                {
                    NetMQ.Sockets.ResponseSocket response = new NetMQ.Sockets.ResponseSocket();
                    response.Connect($"{ServerAddress.IP}:{initial_res.Port}");
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
                    IsRunning = true;
                }
            }
            else
                Logger.Error($"{this.ServiceName} 路由注册服务失败!");
            return IsRunning;
        }
        /// <summary>
        /// 停止消息接收服务
        /// </summary>
        /// <returns></returns>
        public bool StopService()
        {
            _InitializeService(false);

            //for (int i = 0; i < _sockList.Count; i++)
            //    _sockList[i].Dispose();
            //for (int i = 0; i < _pollerList.Count; i++)
            //    _pollerList[i].Dispose();

            //_sockList.Clear();
            //_pollerList.Clear();
            return true;
        }
        /// <summary>
        /// 服务接收消息处理函数
        /// </summary>
        /// <param name="header"></param>
        /// <param name="BodyBytes"></param>
        /// <param name="Socket"></param>
        private void Service_ReceiveMessage(Header header, byte[] BodyBytes, NetMQSocket Socket)
        {
            //初始化Model,并在会话结束后dispose
            using (var dbContext = new EntityModel())
            {
                Session session = null;
                if (header.Session != null)
                    session = dbContext.SessionDB.FirstOrDefault(j => j.GUID == header.Session && j.ExpirationTime <= DateTime.Now);

                IUser user;
                if (session == null)
                    //没有登陆的话,分配一个游客账户
                    user = dbContext.UserDB.FirstOrDefault(i => i.UserName == "Guest");
                else
                {
                    //刷新session有效期
                    session.ExpirationTime = DateTime.Now.AddHours(1);
                    user = session.User;
                }

                if (user == null)
                {
                    Logger.Error($"Service Receive Error : User is Null !!!");
                    return;
                }

                //创建一个简单的会话上下文
                var context = Context.CreateContext(user, header, dbContext, Socket);
                var eventArgs = new RequestEventArgs() { Context = context };
                RequestEvent?.Invoke(this, eventArgs);
                if (eventArgs.Cancel == true)//如果接收的请求被取消,则不委托执行
                    return;
                try
                {
                    //交给注册的委托
                    var handle = MessageHandle.GetHandle(context.Header.MessageType);
                    if (handle != null)
                        handle.Invoke(context, BodyBytes);//全程务必保持由当前线程同步执行
                    else
                        Logger.Warn($"未注册的委托 : {context.Header.MessageType}");
                }
                catch (Exception ex)
                {
                    if (context._answer == false)
                        context.Response(new ResException(ex));
                }


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
                if (mqMsg.FrameCount < 3)
                {
                    Logger.Error($"TryReceive Exceptrion : Not the correct message frame");
                    return;
                }
                var messageType = (ProtoEnum)mqMsg[0].ConvertToInt32();
                var HeaderBytes = mqMsg[1].Buffer;
                var BodyBytes = mqMsg[2].Buffer;
                Header header;
                using (var ms = new System.IO.MemoryStream(HeaderBytes))
                    header = ProtoBuf.Serializer.Deserialize<Header>(ms);
                header.MessageType = messageType;
                this.Service_ReceiveMessage(header, BodyBytes, e.Socket);
            }
            else
                Logger.Error($"TryReceive Exceptrion : Message Receive Fail!");
        }
    }

    public class RequestEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
        public Context Context { get; set; }
    }
}