using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Net;
using Tomato.Net.Protocol.Request;

namespace Tomato.ServiceAccount.Controller
{
    public class AuthorityManager : IModular
    {
        public static AuthorityManager Instance { get; } = new AuthorityManager();
        public ModularEnum Modular => ModularEnum.权限编辑;

        /// <summary>
        /// 检测权限是否权限
        /// </summary>
        /// <param name="context"></param>
        /// <param name="authority"></param>
        /// <returns></returns>
        public bool CheckAuthority(Context context, int authority)
        {
            foreach (var item in context.User.UserGrant.RoleList)
            {
                var state = item.AuthorityList.Any(i => i.AuthorityId == authority);
                if (state)
                {
                    return state;
                }
            }
            return false;
        }
    }
}
