using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Net;
using Tomato.Net.Protocol.Request;
using Tomato.Net.Protocol.Response;
using Tomato.Protocol;

namespace Tomato.ServiceAccount.Controller
{
    public class DepartmentManager
    {
        public static DepartmentManager Instance { get; } = new DepartmentManager();

        /// <summary>
        /// 部门创建
        /// </summary>
        /// <param name="context"></param>
        /// <param name="body"></param>
        public void ReqDepartmentEditHandle(Context context, ReqDepartmentEdit body)
        {
            //检测一下权限
            var db = context.DbContext;
            var depart = db.DepartmentEntities.FirstOrDefault(i => i.Name == body.Name);
            if (depart == null)
            {
                depart = new Service.Model.DepartmentEntity
                {
                    Name = body.Name,
                    CompanyName = "一品科技",
                    Describe = body.Describe,
                    PositionList = new List<Service.Model.PositionEntity>(),
                    SuperiorName = body.SuperiorName
                };
                db.DepartmentEntities.Add(depart);
            }
            else
            {
                depart.Name = body.Name;
                depart.SuperiorName = body.SuperiorName;
                depart.Describe = body.Describe;
            }
            db.SaveChanges();

            context.Response(new ResCurrencyResult()
            {
                Message = "添加/修改成功!",
                Success = true,
            });
        }


        //部门分配职位

        //部门分配用户
    }
}
