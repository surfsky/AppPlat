=====================================
任务调度引擎
=====================================
功能
	调度表达式参照 Quartz cron
		- https://yq.aliyun.com/articles/62723#_Toc465868115
		- 顺序调整为：年 月 日 时 分 周
		- 每个部分可用逗号分隔
		- 只保留 * 符号
	含依赖逻辑
	含成功后重试逻辑（下次运行间隔）
	含失败后重试逻辑（重试间隔、重试次数限制）
	可以用配置文件，也可以直接完全用代码创建 TaskConfig 对象来运行任务。
	Task 含 Data 属性，作为任务调用参数。如测试网站可访问性任务的URL，运行脚本任务的脚本路径等。

历史
	Date        Author        Description
	----        ------        ------------
	2017-11-28  Surfsky       Init


=====================================
部署方法
=====================================
拷贝文件
	App.Schedule.exe
	App.Schedule.exe.config
	Schedule.config
	Log4Net.dll
	Newstonsoft.Json.dll

实现任务逻辑
	（1）引用App.Schedule.dll
    （2）实现接口 ITaskRunner，实现任务处理逻辑
		public class MyTask : ITaskRunner
		{
			public bool Run(DateTime dt)
			{
				return true;
			}
		}
	（2）或继承Task也行（已经实现ITaskRunner接口），重载其虚拟方法
		public class MyTask : Task
		{
			public overrid bool Run(DateTime dt)
			{
				return true;
			}
		}
			

修改配置
	Schedule.config

运行
	App.Schedule.exe


=====================================
Schedule.config 示例
=====================================
~code
{
  "Sleep": 200,                                           // 任务引擎每次循环休息的毫秒数
  "LogDt": "2017-11-28 19:12:41",					      // 最后记录的时间
  "Tasks": [										      // 
    {												      // 
      "Name": "任务",								      // 任务名称
      "Enable": true,								      // 是否有效
      "Schedule": "* * * 1,2 0 *",					      // 调度表达式，格式为：年月日时分周，此表达式为每天的1、2点运行
      "Status": "Success",							      // 最后运行状态
      "LastRunDt": "2017-11-28 19:12:17",			      // 最后运行时间
      "Success": "0000-00-00 00:00:10 0/9",			      // 成功后间隔10秒钟再次运行
      "Failure": "0000-00-00 00:00:02 3/9",			      // 失败后间隔2秒钟再次运行，已失败3次，最多失败10次
      "Runner": "App.Schedule.RandomTask, App.Schedule",  // ITaskRunner 类型名，运行器的逻辑实现
	  "Data": "http://www.baidu.com",                     // 附加参数，供Runner运行时作为参数传入
      "Dependency": [
        {
          "Name": "子任务1",
          "Enable": true,
          "Schedule": "* * * * * *",
          "Status": "Success",
          "LastRunDt": "2017-11-28 19:12:17",
          "Success": "0000-00-00 00:00:10 0/9",
          "Failure": "0000-00-00 00:00:02 0/9",
          "Runner": "App.Schedule.RandomTask, App.Schedule, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
          "Dependency": []
        },
        {
          "Name": "子任务2",
          "Enable": true,
          "Schedule": "* * * * * *",
          "Status": "Success",
          "LastRunDt": "2017-11-28 19:12:17",
          "Success": "0000-00-00 00:00:10 0/9",
          "Failure": "0000-00-00 00:00:02 0/9",
          "Runner": "App.Schedule.RandomTask, App.Schedule, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
          "Dependency": []
        }
      ]
    },
    {
      "Name": "Dummy",
      "Enable": false,
      "Schedule": "* * * * * *",
      "Status": "Success",
      "LastRunDt": "2017-11-28 19:12:37",
      "Success": "0000-00-00 00:01:00 0/9",
      "Failure": "0000-00-00 00:00:20 0/9",
      "Runner": "App.Schedule.DummyTask, App.Schedule, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
      "Dependency": []
    }
  ]
}

