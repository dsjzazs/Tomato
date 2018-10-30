using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Net;
using Tomato.Service.Model;
using Tomato.ServiceAccount.Controller;

namespace Tomato.ServiceAccount
{


    class Program
    {

        static void Main(string[] args)
        {
            EntityModel.InitializeDb(true );
            var service = new AccountManager();
            service.StartService(10);
            while (true)
            {
                if (Console.ReadLine() == "exit")
                {
                    service.StopService();
                    return;
                }
            }
        }
    }
}
