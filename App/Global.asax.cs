using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.IO;
using System.Data.Entity;
using App.DAL;
using App.Components;
using System.Web.Routing;
using System.Diagnostics;
using App.Schedule;
using System.Threading;

namespace App
{
    public class Global : System.Web.HttpApplication
    {
        // 网站启动（若长时间未访问网站会释放进程池并关闭，等待下次请求时才开启，在本方法内无法保证后台线程运行的可靠性）
        protected void Application_Start(object sender, EventArgs e)
        {
            Logger.Info("==== Application_Start ====");
            Database.SetInitializer(new AppDatabaseInitializer());
        }

        //
        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }
        
        protected virtual void Application_EndRequest()
        {
            AppContext.Release();
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            AuthHelper.LoadPrincipal();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            string txt = string.Format("Application_Error: {0}", e);
            Logger.LogToDb(txt, LogLevel.Error);
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            Logger.Info("Application_End");
        }
    }
}