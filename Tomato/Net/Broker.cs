using NetMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Net
{
    /// <summary>
    /// 代理
    /// </summary>
    internal static class Broker
    {
        public const string WorkerAddress = @"tcp://localhost:7777";
        public const string RouterAddress = @"tcp://localhost:6666";
        /// <summary>
        /// 发送和接收超时
        /// </summary>
        public static TimeSpan Timeout { get; set; } = TimeSpan.FromMilliseconds(1000);
        private static NetMQ.Sockets.RouterSocket router = new NetMQ.Sockets.RouterSocket();
        private static NetMQ.Sockets.DealerSocket dealer = new NetMQ.Sockets.DealerSocket();
        public static NetMQ.NetMQPoller Poller { get; } = new NetMQPoller();
        public static bool IsRuning { get { return Poller.IsRunning; } }
        /// <summary>
        /// 启用路由Poller
        /// </summary>
        public static void RunAsync()
        {
            if (Poller.IsRunning)
                return;

            router.Bind(RouterAddress);
            dealer.Bind(WorkerAddress);
            Poller.Add(router);
            Poller.Add(dealer);
            //此处需要传入Poller,否则Proxy.Start会以同步方式启动
            Poller.Add(new NetMQTimer(Timeout));
            Poller.RunAsync();
            NetMQ.Proxy proxy = new Proxy(router, dealer, null, Poller);
            proxy.Start();
        }
    }
}
