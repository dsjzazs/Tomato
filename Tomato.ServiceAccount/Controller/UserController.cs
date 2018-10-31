using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Net;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using Tomato.Entity.Model;
using Tomato.Net.Protocol.Request;
using Tomato.Protocol;
using Tomato.Service;
using Tomato.Net.Protocol.Response;

namespace Tomato.ServiceAccount.Controller
{
    public class UserController
    {
        public static UserController Instance { get; } = new UserController();


        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="context"></param>
        /// <param name="body"></param>
        public void ReqUserRegisterHandle(Context context, ReqUserRegister body)
        {
            var db = context.DbContext;
            var user = db.UserEntities.FirstOrDefault(i => i.UserName == body.UserName);
            if (user == null)
            {
                user = new UserEntity()
                {
                    Email = body.Email,
                    Gender = body.Gender,
                    NickName = body.NickName,
                    Password = body.Password,
                    UserName = body.UserName,
                };
                var session = new SessionEntity()
                {
                    ExpirationTime = DateTime.Now.AddHours(1),
                    User = user,
                    Verified = true,
                    VerifiedTime = DateTime.Now
                };
                db.UserEntities.Add(user);
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

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="context"></param>
        /// <param name="body"></param>
        public void ReqUserLoginHandle(Context context, ReqUserLogin body)
        {
            var db = context.DbContext;
            var user = db.UserEntities.FirstOrDefault(i => i.UserName == body.UserName && i.Password == body.PassWord);
            if (user != null)
            {
                var session = new SessionEntity()
                {
                    ExpirationTime = DateTime.Now.AddHours(1),
                    User = user,
                    Verified = true,
                    VerifiedTime = DateTime.Now
                };
                db.SessionEntities.Add(session);
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
                //异常模式
                throw new NotImplementedException("账号或密码错误");

                /*
                context.Response(new ResUserLogin()
                {
                    Message = "账号或密码错误!",
                    Success = false,
                });
                
             */
            }
        }



    }
}
