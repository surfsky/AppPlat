using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL;

namespace App.Components
{
    /// <summary>
    /// 日志
    /// </summary>
    internal class Logger
    {
        public static log4net.ILog _log;
        static Logger()
        {
            //var file = HttpContext.Current.Server.MapPath("~/Log4Net.config");
            //log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(file));
            log4net.Config.XmlConfigurator.Configure();
            _log = log4net.LogManager.GetLogger("App");
            _log.Info("App logger start");
        }

        // 添加文本日志
        public static void Debug(string format, params object[] args) { _log.DebugFormat(format, args); }
        public static void Info(string format, params object[] args) { _log.InfoFormat(format, args); }
        public static void Warn(string format, params object[] args) { _log.WarnFormat(format, args); }
        public static void Error(string format, params object[] args) { _log.ErrorFormat(format, args); }
        public static void Fatal(string format, params object[] args) { _log.FatalFormat(format, args); }

        /// <summary>添加文本日志</summary>
        public static void Log(LogLevel level, string txt)
        {
            switch (level)
            {
                case LogLevel.Debug: _log.Debug(txt); break;
                case LogLevel.Info: _log.Info(txt); break;
                case LogLevel.Warn: _log.Warn(txt); break;
                case LogLevel.Error: _log.Error(txt); break;
                case LogLevel.Fatal: _log.Fatal(txt); break;
            }
        }

        /// <summary>记录日志到数据库</summary>
        public static void LogToDb(string message, LogLevel level = LogLevel.Info, string operater = "", string from = "Web", string ip = "")
        {
            Log(level, message.GetSummary(50));
            if (operater.IsNullOrEmpty())
                operater = (Common.LoginUser != null) ? Common.LoginUser.NickName : "Unknown";
            var log = new Log
            {
                Lvl = level,
                Operator = operater,
                Message = message,
                Summary = message.GetSummary(50),
                LogDt = DateTime.Now,
                From = from,
                IP = ip.IsNullOrEmpty() ? Asp.GetClientIP() : ip
            };
            log.SaveNew(false);
        }
    }
}