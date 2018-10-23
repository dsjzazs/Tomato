using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Net.Protocol;
using Tomato.Protocol;
namespace Tomato.Net
{
    public interface INetworkHandle
    {
        void Invoke(Context context, byte[] bodyBytes);
    }
    internal class NetworkHandleBase<T> : INetworkHandle where T : IProtocol, new()
    {
        internal HandlerDelegate<T> Handle { get; set; }
        public NetworkHandleBase(HandlerDelegate<T> fun)
        {
            Handle = fun;
        }
        public void Invoke(Context context, byte[] bodyBytes)
        {
            IProtocol body;
            using (var ms = new System.IO.MemoryStream(bodyBytes))
                body = ProtoBuf.Serializer.Deserialize<T>(ms);
            Handle.Invoke(context, (T)body);
        }
    }
    public delegate void HandlerDelegate<T>(Context context, T body) where T : IProtocol, new();
    public class MessageHandle
    {
        private MessageHandle() { }
        public static MessageHandle Instance { get; } = new MessageHandle();
        internal readonly Dictionary<ProtoEnum, INetworkHandle> DicHandles  = new Dictionary<ProtoEnum, INetworkHandle>();
        /// <summary>
        /// 注册消息处理委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="MessageType"></param>
        /// <param name="handle"></param>
        public void RegisterHandle<T>(HandlerDelegate<T> handle) where T : IProtocol, new()
        {
            var MessageType = new T().MessageType;
            if (DicHandles.ContainsKey(MessageType))
                throw new ArgumentException($"{MessageType}消息句柄已被注册!");
            DicHandles.Add(MessageType, new NetworkHandleBase<T>(new HandlerDelegate<T>(handle)));
        }
        public bool Exist(ProtoEnum MessageType)
        {
            return DicHandles.ContainsKey(MessageType);
        }

        public bool UnloadHandle<T>() where T : IProtocol, new()
        {
            return DicHandles.Remove(new T().MessageType);
        }
        /// <summary>
        /// 注销委托
        /// </summary>
        /// <param name="MessageType"></param>
        public void UnloadHandle(ProtoEnum MessageType)
        {
            if (DicHandles.ContainsKey(MessageType))
                throw new ArgumentException($"{MessageType}消息句柄不存在!");
            DicHandles.Remove(MessageType);
        }
        /// <summary>
        /// 获取委托
        /// </summary>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public INetworkHandle GetHandle(ProtoEnum messageType)
        {
            if (DicHandles.TryGetValue(messageType, out INetworkHandle handle))
                return handle;
            return null;
        }


    }
}
