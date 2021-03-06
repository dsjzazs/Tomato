﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Net.Protocol;

namespace Tomato.Net.Protocol.Request
{
    [ProtoBuf.ProtoContract]
    public class ReqPositionEdit : Net.IProtocol
    {
        /// <summary>
        /// 职位名称
        /// </summary>
        [ProtoBuf.ProtoMember(1)]
        public string Name { get; set; }

        /// <summary>
        /// 职位描述
        /// </summary>
        [ProtoBuf.ProtoMember(2)]
        public string Describe { get; set; }

        /// <summary>
        /// 上级职位
        /// </summary>
        [ProtoBuf.ProtoMember(3)]
        public string SuperiorName { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [ProtoBuf.ProtoMember(4)]
        public string DepartmentName { get; set; }

        public ProtoEnum MessageType => throw new NotImplementedException();

    
    }
}
