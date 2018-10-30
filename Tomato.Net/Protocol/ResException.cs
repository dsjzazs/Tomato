using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Net.Protocol
{
    [ProtoBuf.ProtoContract]
    public class ResException
    {
        /// <summary>
        /// 获取或设置导致错误的应用程序或对象的名称。
        /// </summary>
        [ProtoBuf.ProtoMember(1)] public virtual string Source { get; set; }
        /// <summary>
        /// 获取或设置指向与此异常关联的帮助文件链接。
        /// </summary>
        [ProtoBuf.ProtoMember(2)] public virtual string HelpLink { get; set; }
        /// <summary>
        /// 获取调用堆栈上的即时框架字符串表示形式。
        /// </summary>
        [ProtoBuf.ProtoMember(3)] public virtual string StackTrace { get; set; }
        /// <summary>
        /// 摘要:
        ///     获取导致当前异常的 System.Exception 实例。
        /// 
        /// 返回结果:
        ///     描述导致当前异常的错误的一个对象。 System.Exception.InnerException 属性返回的值与传递到 System.Exception.#ctor(System.String,System.Exception)
        ///     构造函数中的值相同，如果没有向构造函数提供内部异常值，则为 null。 此属性是只读的。
        /// </summary>
        [ProtoBuf.ProtoMember(4)] public ResException InnerException { get; set; }
        /// <summary>
        ///  获取描述当前异常的消息。
        /// </summary>
        [ProtoBuf.ProtoMember(5)] public virtual string Message { get; set; }
        /// <summary>
        /// 获取或设置 HRESULT（一个分配给特定异常的编码数字值）。
        /// </summary>
        [ProtoBuf.ProtoMember(6)] public int HResult { get; set; }
    }
}