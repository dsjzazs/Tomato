using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Net;
using Tomato.Net.Protocol;
using Tomato.Net.Protocol.Request;
using Tomato.Net.Protocol.Response;
using Tomato.Protocol;
using Tomato.Service;

namespace Tomato.ServiceAccount.Controller
{
    public class DepartmentManager : IModular
    {
        public static DepartmentManager Instance { get; } = new DepartmentManager();

        public ModularEnum Modular => ModularEnum.部门编辑;

        /// <summary>
        /// 部门创建
        /// </summary>
        /// <param name="context"></param>
        /// <param name="body"></param>
        public void ReqDepartmentEditHandle(Context context, ReqDepartmentEdit body)
        {
            //检测一下权限
            Console.WriteLine("添加成功");
            var db = context.DbContext;
            try
            {
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
                context.Response(new ResDepartment()
                {
                    Success = true,
                    GUID = depart.GUID.ToString(),
                });
            }
            catch (Exception e)
            {
                context.Response(new ResException()
                {
                   
                });
            }
          

           
        }

    }
}
