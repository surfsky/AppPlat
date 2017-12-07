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
// 各种辅助任务
//---------------------------------------------------------
namespace App.Tasks
{
    /// <summary>清除日志。每月1日运行</summary>
    /// <remarks>log4net的日志不用写代码清理，可在配置中设置 MaxSizeRollBackups 参数</remarks>
    public class ClearLogTask : ITaskRunner
    {
        public bool Run(DateTime dt, string data)
        {
            Log.DeleteBatch(1);
            return true;
        }
    }

    /// <summary>
    /// 连接测试任务
    /// </summary>
    public class ConnectTask : ITaskRunner
    {
        public bool Run(DateTime dt, string data)
        {
            try
            {
                HttpHelper.Get(data);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
