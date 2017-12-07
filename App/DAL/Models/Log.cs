using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EntityFramework.Extensions;
using App.Components;

namespace App.DAL
{
    /// <summary>
    /// 日志级别
    /// </summary>
    public enum LogLevel
    {
        [UI("调试")]  Debug = 0,
        [UI("提示")]  Info = 1,
        [UI("警告")]  Warn = 2,
        [UI("错误")]  Error = 3,
        [UI("致命")]  Fatal = 4,
    }

    /// <summary>
    /// 日志
    /// </summary>
    public class Log : DbBase<Log>
    {
        [UI("级别")]                               public LogLevel? Lvl { get; set; }
        [UI("操作者")]                             public string Operator { get; set; }
        [UI("摘要")]                               public string Summary { get; set; }
        [UI("信息", Editor = EditorType.TextArea)] public string Message { get; set; }
        [UI("IP地址")]                             public string IP { get; set; }
        [UI("来自")]                               public string From { get; set; }
        [UI("记录时间")]                           public DateTime? LogDt { get; set; }


        //-----------------------------------------------
        // 公共方法
        //-----------------------------------------------
        // 查找
        public static IQueryable<Log> Search(string user, string msg, LogLevel? level=null, DateTime? fromDt = null)
        {
            IQueryable<Log> q = Set;
            if (!string.IsNullOrEmpty(user))   q = q.Where(l => l.Operator.Contains(user));
            if (!string.IsNullOrEmpty(msg))    q = q.Where(l => l.Message.Contains(msg));
            if (level != null)                 q = q.Where(l => l.Lvl == level);
            if (fromDt != null)                q = q.Where(t => t.LogDt >= fromDt);
            return q;
        }

        /// <summary>删除n个月前的数据</summary>
        public static int DeleteBatch(int months = 1)
        {
            var lastMonth = DateTime.Now.AddMonths(-months);
            int n = Set.Where(t => t.LogDt <= lastMonth).Delete();
            return n;
        }

    }
}