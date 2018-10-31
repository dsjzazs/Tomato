using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Net;
using Tomato.Net.Protocol.Request;
using Tomato.Service;

namespace Tomato.ServiceAccount.Controller
{
    public class AuthorityController 
    {
        public static AuthorityController Instance { get; } = new AuthorityController();

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
