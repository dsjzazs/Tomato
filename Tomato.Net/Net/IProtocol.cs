using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Net.Protocol;

namespace Tomato.Net
{
    public interface IProtocol
    {
        ProtoEnum MessageType { get; }
    }

    public class RemoteServiceException : Exception
    {
        public RemoteServiceException() { }
        public RemoteServiceException(string text) : base(text) { }
    }
}
