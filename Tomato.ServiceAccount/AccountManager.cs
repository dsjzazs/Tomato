﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tomato.Net;
using Tomato.Net.Protocol.Request;
using Tomato.Protocol;
using Tomato.Protocol.Request;

namespace Tomato.ServiceAccount
{
    public class AccountManager : ServiceBase
    {
        public override string ServiceName => "员工管理模块";

        //注册委托
        public AccountManager()
        {
            MessageHandle.RegisterHandle<ReqUserLogin>(Controller.UserManager.Instance.ReqUserLoginHandle);
            MessageHandle.RegisterHandle<ReqUserRegister>(Controller.UserManager.Instance.ReqUserRegisterHandle);

            MessageHandle.RegisterHandle<ReqDepartmentEdit>(Controller.DepartmentManager.Instance.ReqDepartmentEditHandle);
        }

        

        //员工管理 
        //部门管理
        //职位管理
        //角色管理
        //权限管理
    }
}