using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL;
using System.Web.Security;

namespace App.Handlers
{
    /// <summary>
    /// 注销返回首页
    /// </summary>
    public class Logout : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            DbUser.Logout(true);
        }

        public bool IsReusable
        {
            get {return false;}
        }
    }
}