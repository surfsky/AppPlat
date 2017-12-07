using System;
using System.Diagnostics;
using System.IO;
using System.Web;
using App.DAL;

namespace App.Components
{
    /// <summary>
    /// 网站访问监控模块。
    /// （1）请在Web.config中增加
    /// &lt;modules&gt;
    ///  &lt;add name = "MonitorModule" type="App.Components.MonitorModule" /&gt;
    /// &lt;/modules>
    /// （2）默认显示在调试输出页面。
    /// （3）若有需要，可用文本或数据库保存访问数据，便于性能分析
    /// </summary>
    public class MonitorModule : IHttpModule
    {
        public void Dispose() { /* Not needed */ }

        public void Init(HttpApplication context)
        {
            // 页面请求来到时开启跑表
            context.PreRequestHandlerExecute += delegate (object sender, EventArgs e)
            {
                var watch = new Stopwatch();
                watch.Start();
                HttpContext.Current.Items["MonitorTimer"] = watch;
            };

            // 请求结束后关闭跑表并计算时间
            context.PostRequestHandlerExecute += delegate (object sender, EventArgs e)
            {
                var watch = HttpContext.Current.Items["MonitorTimer"] as Stopwatch;
                watch.Stop();
                var info = new RequestInfo()
                {
                    Url = context.Request.Url.ToString(),
                    RequestDt = DateTime.Now,
                    Seconds = watch.ElapsedMilliseconds / 1000.0,
                    ClientIP = Asp.GetClientIP()
                };

                // 输出日志
                Trace.WriteLine(info.ToString());
                Logger.Info(info.ToString());
            };
        }
    }

    /// <summary>
    /// 请求信息
    /// </summary>
    public class RequestInfo
    {
        public string Url { get; set; }
        public DateTime RequestDt { get; set; }
        public double Seconds { get; set; }
        public string ClientIP { get; set; }

        public override string ToString()
        {
            return string.Format("{0:yyyy-MM-dd HH:mm:ss:fff} 请求 {1}, IP地址 {2}, 耗时 {3:F4} 秒", RequestDt, Url, ClientIP, Seconds);
        }
    }
}
