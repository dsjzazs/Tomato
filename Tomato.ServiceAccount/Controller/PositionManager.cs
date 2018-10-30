using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Net;
using Tomato.Protocol.Request;
using Tomato.Protocol.Response;
using Tomato.Service;

namespace Tomato.ServiceAccount.Controller
{
    public class PositionManager : IModular
    {
        public static PositionManager Instance { get; } = new PositionManager();

        public ModularEnum Modular => ModularEnum.职位编辑;

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
                //创建一个新的职位
                position = new Service.Model.PositionEntity
                {
                    Name = body.Name,

                    Department = body.DepartmentName,
                    Describe = body.Describe,
                    SuperiorName = body.SuperiorName,
                };
            }
            else
            {
                position.Describe = body.Describe;
                position.Department = body.DepartmentName;
                position.SuperiorName = body.SuperiorName;
            }
            db.SaveChanges();
        }

    }
}
