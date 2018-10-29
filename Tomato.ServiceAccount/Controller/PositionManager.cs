using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Net;
using Tomato.Net.Protocol.Request;
using Tomato.Net.Protocol.Response;

namespace Tomato.ServiceAccount.Controller
{
    public class PositionManager
    {
        public static PositionManager Instance { get; } = new PositionManager();

        /// <summary>
        /// 职位创建
        /// </summary>
        /// <param name="context"></param>
        /// <param name="body"></param>
        public void ReqpositionEdit(Context context, ReqPositionEdit body)
        {
            //检测权限

            var db = context.DbContext;
            var position = db.PositionEntities.FirstOrDefault(i => i.Name == body.Name);

            if (position == null)
            {
                
            }
            else
            {
                
            }
        }


        //职位分配用户
    }
}
