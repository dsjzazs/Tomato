using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Model;
using Tomato.Net;

namespace Tomato.AccountService
{


    class Program
    {

        static void Main(string[] args)
        {
            EntityModel.InitializeDb(true);
            MessageService.Instance.ServiceName = "用户管理模块";
            UserManager.Instance.Initialize();
            MessageService.Instance.StartService(10);
            while (true)
            {
                if (Console.ReadLine() == "exit")
                {
                    MessageService.Instance.StopService();
                    return;
                }
            }
        }
    }
}
