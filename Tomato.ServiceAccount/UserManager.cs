using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Net;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using Tomato.Model;
using Tomato.Protocol.Request;
using Tomato.Protocol.Response;

namespace Tomato.ServiceAccount
{
    public class UserManager : ServiceBase
    {
        public override string ServiceName => "用户管理模块";
        public UserManager()
        {
            //注册委托
            MessageHandle.RegisterHandle<ReqUserLogin>(LoginRequestHandle);
            MessageHandle.RegisterHandle<ReqUserRegister>(RegisterRequestHandel);
        }
        private void RegisterRequestHandel(Context context, ReqUserRegister body)
        {
            var db = context.DbContext;
            var user = db.UserDB.FirstOrDefault(i => i.UserName == body.UserName);
            if (user == null)
            {
                user = new UserInfo()
                {
                    Email = body.Email,
                    Gender = body.Gender,
                    NickName = body.NickName,
                    Password = body.Password,
                    UserName = body.UserName,
                };
                var session = new Session()
                {
                    ExpirationTime = DateTime.Now.AddHours(1),
                    User = user,
                    Verified = true,
                    VerifiedTime = DateTime.Now
                };
                db.UserDB.Add(user);
                db.SaveChanges();
                Console.WriteLine(user.GUID);

                Console.WriteLine($"Register :  UserName : {user.UserName}  NickName : {user.NickName} PassWrod : {user.Password}");
                context.Response(new ResUserRegister()
                {
                    Message = "注册成功!",
                    Success = true,
                    Session = session.GUID
                });
            }
            else
            {
                context.Response(new ResUserRegister()
                {
                    Message = "账号已存在!",
                    Success = false,
                });
            }
        }

        private void LoginRequestHandle(Context context, ReqUserLogin Body)
        {
            var db = context.DbContext;
            var user = db.UserDB.FirstOrDefault(i => i.UserName == Body.UserName && i.Password == Body.PassWord);
            if (user != null)
            {
                var session = new Session()
                {
                    ExpirationTime = DateTime.Now.AddHours(1),
                    User = user,
                    Verified = true,
                    VerifiedTime = DateTime.Now
                };
                db.SessionDB.Add(session);
                db.SaveChanges();
                context.Response(new ResUserLogin()
                {
                    Message = "登陆成功!",
                    Success = true,
                    Session = session.GUID
                });
                Console.WriteLine($"Login :  UserName : {user.UserName}  NickName : {user.NickName} PassWrod : {user.Password}");
            }
            else
            {
                context.Response(new ResUserLogin()
                {
                    Message = "账号或密码错误!",
                    Success = false,
                });
            }
        }
    }
}
