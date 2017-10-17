using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
namespace Tomato.Net
{
    public class Context
    {
        public static Context Create(User user, Header header, NetMQ.NetMQSocket socket)
        {
            return new Context()
            {
                Header = header,
                User = user,
                Socket = socket
            };
        }
        public Header Header { get; private set; }
        public User User { get; private set; }
        public NetMQ.NetMQSocket Socket { get; private set; }
    }
}
