using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomato.Net.Protocol
{
    public enum ProtoEnum
    {
        Exception = 100,
        ReqUserLogin = 1001,
        ResUserLogin = 1002,
        ReqUserRegister = 1003,
        ResUserRegister = 1004,
        ReqDepartmentEdit = 1005,
        ResDepartment = 1006,
        ReqDepartmentList = 1007,
        ReqRoleEdit = 1008,
        ReqRoleList = 1009,
    }
}
