using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Net;

namespace Tomato.ServiceTaskPlan
{
    public class TaskPlanManager : ServiceBase
    {
        public static TaskPlanManager Instance { get; } = new TaskPlanManager();

        public override string ServiceName => "任务计划";

        #region 启动和停止模块
        public void Initialization()
        {
            //MessageHandle.Instance.RegisterHandle<>();
            //MessageHandle.Instance.RegisterHandle<>();
        }

        public void UnInitialization()
        {
        }
        #endregion


    }
}
