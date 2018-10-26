using NetMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Net;
using Tomato.Net.Protocol;

namespace Tomato.Route
{
    public class ServiceModelInfo
    {
        public string ServiceName { get; set; }
        public NetMQ.Sockets.DealerSocket Dealer { get; set; }
        public List<int> ProtocolList { get; set; } = new List<int>();
        public bool Exist(int protocolID)
        {
            return ProtocolList.Any(i => i == protocolID);
        }
    }
    public class ServiceManager
    {
        private static NetMQ.Sockets.ResponseSocket RegisterService_REP = new NetMQ.Sockets.ResponseSocket();
        private static NetMQ.NetMQPoller Poller { get; } = new NetMQPoller();
        /// <summary>
        /// 服务列表
        /// </summary>
        internal static List<ServiceModelInfo> service_list = new List<ServiceModelInfo>();
        public static void Initialize()
        {
            RegisterService_REP.Bind(ServerAddress.RegisterServiceAddress);
            RegisterService_REP.ReceiveReady += RegisterService_REP_ReceiveReady;
            Poller.Add(RegisterService_REP);
            Poller.RunAsync();
        }

        /// <summary>
        /// 卸载服务
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool Uninstall(List<int> list)
        {
            if (list == null)
                return false;

            foreach (var item in list)
            {
                var service = FillService(item);
                if (service != null)
                {
                    service.Dealer.Dispose();
                    service_list.Remove(service);
                }
            }
            return false;
        }
        /// <summary>
        /// 卸载服务
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public static bool Uninstall(ServiceModelInfo service)
        {
            if (service == null)
                return false;

            if (service_list.Contains(service))
            {
                Console.WriteLine($"Unload Service:{service.ServiceName}  ProtocolList:{_listToString(service.ProtocolList)}");
                return service_list.Remove(service);
            }
            return false;
        }
        /// <summary>
        /// 根据协议ID搜索可用服务
        /// </summary>
        /// <param name="protocolID"></param>
        /// <returns></returns>
        public static ServiceModelInfo FillService(int protocolID)
        {
            foreach (var item in service_list)
            {
                if (item.Exist(protocolID))
                    return item;
            }
            return null;
        }
        /// <summary>
        /// 检查协议是否重复注册
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static bool _checkProtocol(List<int> list)
        {
            foreach (var item in list)
            {
                if (FillService(item) != null)
                    return false;
            }
            return true;
        }
        /// <summary>
        /// 注册服务,并响应端口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void RegisterService_REP_ReceiveReady(object sender, NetMQSocketEventArgs e)
        {
            Tomato.Net.Protocol.RegisterServiceResponse response;
            var res = ProtoBuf.Serializer.Deserialize<RegisterServiceRequest>(new System.IO.MemoryStream(e.Socket.ReceiveFrameBytes()));
            if (res.IsRegister)
            {
              //  if (_checkProtocol(res.ProtocolList)==false)
                ServiceManager.Uninstall(res.ProtocolList);

                var service = new ServiceModelInfo();
                service.Dealer = new NetMQ.Sockets.DealerSocket();
                var port = service.Dealer.BindRandomPort(ServerAddress.IP);
                service.ProtocolList = res.ProtocolList;
                service.ServiceName = res.ServiceName;
                service_list.Add(service);
                Console.WriteLine($"Load Service:{res.ServiceName} Port:{port} ProtocolList:{_listToString(res.ProtocolList)}");
                response = new RegisterServiceResponse() { Port = port, Success = true, Message = "服务模块加载成功" };
            }
            else
            {
                ServiceManager.Uninstall(res.ProtocolList);
                Console.WriteLine($"Uninstall Service : {res.ServiceName}");
                response = new RegisterServiceResponse() { Port = 0, Success = true, Message = "服务模块卸载成功" };
            }

            using (var stream = new System.IO.MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(stream, response);
                e.Socket.SendFrame(stream.ToArray());
            }

        }
        private static string _listToString(System.Collections.IEnumerable list)
        {
            if (list == null)
                return null;
            System.Text.StringBuilder sb = new StringBuilder();
            foreach (var item in list)
                sb.Append($"[{item.ToString()}]");
            return sb.ToString();
        }
    }
}
