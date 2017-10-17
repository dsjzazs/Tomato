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
            //注册委托
            MessageHandle.Instance.RegisterHandle<LoginRequest>(LoginRequest);
            Broker.RunAsync();
            MessageService.Instance.StartService(10);

            Console.ReadKey();
        }
   
        private static void LoginRequest(Context context, LoginRequest Body)
        {     
            //收到的请求
            System.Threading.Thread.Sleep(3000);//模拟查询操作

            Console.WriteLine($"Login :  UserName : {Body.UserName} PassWrod : {Body.PassWrod}");
            context.Response(new LoginResponse()
            {
                Message = "登陆成功,欢迎使用",
                Success = true,
                Session = Guid.NewGuid()
            });
        }
    }
}
