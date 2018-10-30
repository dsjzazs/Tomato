using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Net.Protocol.Request
{
    /// <summary>
    /// 请求获取部门列表
    /// </summary>
    [ProtoBuf.ProtoContract]
    public class ReqDepartmentList : Net.IProtocol
    {
        //请求部门==假设此处可以加上一个筛选返回规则
        [ProtoBuf.ProtoMember(1)]
        public string Rule { get; set; }

        public ProtoEnum MessageType => ProtoEnum.ReqDepartmentList;
    }
}
