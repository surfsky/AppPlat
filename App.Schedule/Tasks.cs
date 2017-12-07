using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;

namespace App.Schedule
{
    /// <summary>示例任务，休眠1秒，返回true</summary>
    public class DummyTask : ITaskRunner
    {
        public bool Run(DateTime dt, string data)
        {
            System.Threading.Thread.Sleep(1000);
            return true;
        }
    }

    /// <summary>随机成功任务。可用于模拟测试任务依赖逻辑。</summary>
    public class RandomTask : ITaskRunner
    {
        Random _random = new Random();
        public bool Run(DateTime dt, string data)
        {
            return _random.Next(0, 100) >= 50;
        }
    }

}
