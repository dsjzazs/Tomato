using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Net.Protocol
{
    [ProtoBuf.ProtoContract]
    public class ResException : IProtocol
    {
        public ResException()
        {

        }
        private BinaryFormatter Formatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
        public ResException(Exception ex)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                Formatter.Serialize(ms, ex);
                this.Source = ms.ToArray();
            }
        }
        private Exception _exception;
        public Exception GetException()
        {
            using (var ms = new System.IO.MemoryStream(this.Source))
                return (Exception)Formatter.Deserialize(ms);
        }
        [ProtoBuf.ProtoMember(1)] public byte[] Source { get; set; }

        public ProtoEnum MessageType => ProtoEnum.Exception;
    }
}