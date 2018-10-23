using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Net;
using Tomato.Protocol;
using System.Messaging;
using NetMQ;
namespace Tomato
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<Model.Model>());
            //       Broker.RunAsync();
            MessageService.Instance.ServiceName = "用户管理模块";
            UserManager.Instance.Initialize();
            MessageService.Instance.StartService(10);
            Console.ReadKey();
        }


    }
}
