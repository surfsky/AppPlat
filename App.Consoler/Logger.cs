using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL;
using System.Reflection;
using System.IO;

namespace App.Consoler
{
    /// <summary>
    /// 日志
    /// </summary>
    internal class Logger
    {
        // 日志器
        public static log4net.ILog _log;
        static Logger()
        {
            log4net.Config.XmlConfigurator.Configure();
            _log = log4net.LogManager.GetLogger("Consoler");
            _log.Info("Logger start");
        }

        // 添加文本日志
        public static void Debug(string format, params object[] args) {_log.DebugFormat(format, args);}
        public static void Info(string format,  params object[] args) {_log.InfoFormat(format, args);}
        public static void Warn(string format,  params object[] args) {_log.WarnFormat(format, args);}
        public static void Error(string format, params object[] args) {_log.ErrorFormat(format, args);}
        public static void Fatal(string format, params object[] args) {_log.FatalFormat(format, args);}

        /// <summary>添加文本日志</summary>
        public static void Log(LogLevel level, string txt)
        {
            switch (level)
            {
                case LogLevel.Debug: _log.Debug(txt); break;
                case LogLevel.Info:  _log.Info(txt);  break;
                case LogLevel.Warn:  _log.Warn(txt);  break;
                case LogLevel.Error: _log.Error(txt); break;
                case LogLevel.Fatal: _log.Fatal(txt); break;
            }
        }
    }
}