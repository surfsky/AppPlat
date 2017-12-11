using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using App.Components;
using System.IO;
using App.DAL;
using App.WeiXin;
using System.Xml.Serialization;
using App.Schedule;

//---------------------------------------------------------
// 各种定时后台任务。由App.Consoler.exe运行，配置写在Schedule.config中
//---------------------------------------------------------
namespace App.Jobs
{
    /// <summary>清除日志。每月1日运行</summary>
    /// <remarks>log4net的日志不用写代码清理，可在配置中设置 MaxSizeRollBackups 参数</remarks>
    public class ClearLogJob : IJobRunner
    {
        public bool Run(DateTime dt, string data)
        {
            Log.DeleteBatch(1);
            return true;
        }
    }

    //---------------------------------------------------------
    // 各种统计类任务
    //---------------------------------------------------------
    /// <summary>每日报表任务（处理昨天的数据），每天1点运行</summary>
    public class DayReportJob : IJobRunner
    {
        public bool Run(DateTime dt, string data)
        {
            var startDt = dt.Date.AddDays(-1);
            var endDt = dt.Date;
            return true;
        }
    }

    /// <summary>每月报表任务（处理上个月的数据）</summary>
    public class MonthReportJob : IJobRunner
    {
        public bool Run(DateTime dt, string data)
        {
            var now = DateTime.Now;
            var pre = now.AddMonths(-1);
            var startDt = new DateTime(pre.Year, pre.Month, 1);
            var endDt = new DateTime(now.Year, now.Month, 1);
            return true;
        }
    }

    /// <summary>每周报表任务（处理上周的数据）</summary>
    public class WeekReportJob : IJobRunner
    {
        public bool Run(DateTime dt, string data)
        {
            var now = DateTime.Now;
            var startDt = DateTimeHelper.GetWeekdayDt(now.AddDays(-7), DayOfWeek.Monday);
            var endDt = DateTimeHelper.GetWeekdayDt(now, DayOfWeek.Monday);
            return true;
        }
    }
}
