using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Net
{
    public static class ServerAddress
    {
        /// <summary>
        /// 工作者连接地址
        /// </summary>
        public const string WorkerAddress = @"tcp://192.168.8.251:7777";

        //客户端请求地址
        public const string RouterAddress = @"tcp://192.168.8.251:6666";
        public const string RegisterServiceAddress = @"tcp://192.168.8.251:7026";
        public const string IP = @"tcp://192.168.8.251";
    }
}
