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
        public const string WorkerAddress = @"tcp://localhost:7777";

        //客户端请求地址
        public const string RouterAddress = @"tcp://localhost:6666";
        public const string RegisterServiceAddress = @"tcp://localhost:7026";
        public const string IP = @"tcp://localhost";
    }
}
