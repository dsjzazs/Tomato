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
        public static Context Create(EF.IUser user, Header header, EF.Model dbContext, NetMQ.NetMQSocket socket)
        {
            return new Context()
            {
                Header = header,
                User = user,
                Socket = socket,
                DbContext = dbContext
            };
        }
        public EF.Model DbContext { get; private set; }
        public Header Header { get; private set; }
        public EF.IUser User { get; private set; }
        public NetMQ.NetMQSocket Socket { get; private set; }
    }
}
