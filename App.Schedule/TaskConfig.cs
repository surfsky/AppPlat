using System;
using System.Collections.Generic;
using System.IO;
using App.Components;

namespace App.Schedule
{
    /// <summary>
    /// 任务配置。
    /// </summary>
    public class TaskConfig
    {
        /// <summary>每次循环休眠毫秒数</summary>
        public int Sleep { get; set; }
        public DateTime LogDt { get; set; }
        public List<Task> Tasks { get; set; }

        // 单例
        static string _configFile;

        public TaskConfig() { }
        public TaskConfig(string configFile)
        {
            _configFile = configFile;
            var cfg = Load(configFile);
            this.Sleep = cfg.Sleep;
            this.LogDt = cfg.LogDt;
            this.Tasks = cfg.Tasks;
        }

        // 加载
        static TaskConfig Load(string filePath)
        {
            if (!File.Exists(filePath))
                Create().Save();
            return SerializeHelper.LoadJson(filePath, typeof(TaskConfig)) as TaskConfig;
        }

        // 保存
        public void Save()
        {
            if (!string.IsNullOrEmpty(_configFile))
               SerializeHelper.SaveJson(_configFile, this);
        }

        // 创建示例数据
        static TaskConfig Create()
        {
            TaskConfig cfg = new TaskConfig();
            cfg.Sleep = 400;
            cfg.LogDt = DateTime.Now;
            cfg.Tasks = new List<Task>
            {
                new Task()
            };
            return cfg;
        }
    }
}
