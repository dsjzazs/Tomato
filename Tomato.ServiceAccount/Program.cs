using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Model;
using Tomato.Net;

namespace Tomato.ServiceAccount
{


    class Program
    {

        static void Main(string[] args)
        {
            EntityModel.InitializeDb(true);
            var service = new UserManager();
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
