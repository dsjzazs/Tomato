using NetMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Route
{
    /// <summary>
    /// 代理
    /// </summary>
    internal static class Broker
    {

        /// <summary>
        /// 发送和接收超时
        /// </summary>
        public static TimeSpan Timeout { get; set; } = TimeSpan.FromMilliseconds(1000);
        private static NetMQ.Sockets.RouterSocket router = new NetMQ.Sockets.RouterSocket();
        public static NetMQ.NetMQPoller Poller { get; } = new NetMQPoller();
        public static bool IsRuning { get { return Poller.IsRunning; } }
        /// <summary>
        /// 启用路由Poller
        /// </summary>
        public static void RunAsync()
        {
            if (Poller.IsRunning)
                return;

            router.Bind(ServerAddress.RouterAddress);
            router.ReceiveReady += Router_ReceiveReady;
            //  dealer.ReceiveReady += Dealer_ReceiveReady;
            Poller.Add(router);
            //  Poller.Add(dealer);
            Poller.Add(new NetMQTimer(Timeout));
            Poller.RunAsync();
            //此处需要传入Poller,否则Proxy.Start会以同步方式启动
            //    NetMQ.Proxy proxy = new Proxy(router, dealer, null, Poller);
            //     proxy.Start();
        }

        private static void RegisterService_REP_ReceiveReady(object sender, NetMQSocketEventArgs e)
        {
            var msg = e.Socket.ReceiveMultipartMessage();

        }
        /// <summary>
        /// 客户端路由请求接入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Router_ReceiveReady(object sender, NetMQSocketEventArgs e)
        {
            try
            {
                //前两个字节是路由的信息
                var msg = e.Socket.ReceiveMultipartMessage();
                //索引[2]才是协议ID
                var protocolID = msg[2].ConvertToInt32();
                var service = ServiceManager.FillService(protocolID);
                if (service == null)
                {
                    _response_exception(msg, e, 800001, $"消息请求：{protocolID}，找不到模块！");
                    return;
                }

                var res = service.Dealer.TrySendMultipartMessage(msg);
                Console.WriteLine($"路由 接入 : {protocolID} >> {service.ServiceName} 转发 : {res}");
                if (res)
                    ProxyBetween(service.Dealer, e.Socket, null);
                else
                {
                    ServiceManager.Uninstall(service);
                    _response_exception(msg, e, 800002, $"消息请求：{protocolID}，找不到模块！");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        /// <summary>
        /// 向服务模块响应异常信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="e"></param>
        /// <param name="errorCode"></param>
        /// <param name="text"></param>
        private static void _response_exception(NetMQMessage msg, NetMQSocketEventArgs e, int errorCode, string text)
        {
            Console.WriteLine($"response Exception  code : {errorCode},{text}");
            var res_msg = new NetMQMessage();
            res_msg.Append(msg[0]);
            res_msg.Append(msg[1]);
            res_msg.Append(errorCode);
            res_msg.Append(text, Encoding.UTF8);
            e.Socket.SendMultipartMessage(res_msg);
        }


        /// <summary>
        /// 将所有消息帧原封不动的发送至另外一端
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="control"></param>
        private static void ProxyBetween(IReceivingSocket from, IOutgoingSocket to, IOutgoingSocket control)
        {
            Msg msg = default(Msg);
            msg.InitEmpty();
            Msg msg2 = default(Msg);
            msg2.InitEmpty();
            bool hasMore;
            do
            {
                from.Receive(ref msg);
                hasMore = msg.HasMore;
                if (control != null)
                {
                    msg2.Copy(ref msg);
                    control.Send(ref msg2, hasMore);
                }
                to.Send(ref msg, hasMore);
            }
            while (hasMore);
            msg2.Close();
            msg.Close();
        }
    }

}
