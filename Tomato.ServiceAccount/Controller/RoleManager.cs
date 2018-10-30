using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Net;
using Tomato.Protocol.Request;
using Tomato.Service;

namespace Tomato.ServiceAccount.Controller
{
    //角色控制器
    public class RoleManager : IModular
    {
        public static RoleManager Instance { get; } = new RoleManager();

        public ModularEnum Modular => ModularEnum.角色编辑;

        //角色创建

        //角色分配用户

        //角色分配权限

        //角色是否存在用户
    }
}
