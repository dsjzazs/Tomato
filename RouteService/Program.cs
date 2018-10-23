using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("正在启动代理...");
            ServiceManager.Initialize();
            Broker.RunAsync();
            Console.WriteLine("启动成功");
            Console.ReadLine();
        }
    }
}
