using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Components;
using EntityFramework.Extensions;

namespace App.DAL
{
    /// <summary>
    /// 系统配置表。
    /// 系统所有的状态信息、枚举信息都可在本表维护。可供用户在后台查看、修改。
    /// 系统运行类的参数信息直接保存在Web.Config文件内，仅程序员可修改。
    /// </summary>
    public class Config : EntityBase<Config>
    {
        [UI("类别")]     public string Category { get; set; }
        [UI("键")]       public string Key { get; set; }
        [UI("值")]       public string Value { get; set; }
        [UI("标题")]     public string Title { get; set; }

        //-------------------------------------------
        // 方法
        //-------------------------------------------
        public static IQueryable<Config> Search(string catetory, string key="")
        {
            IQueryable<Config> q = Set;
            if (!catetory.IsNullOrEmpty())  q = q.Where(t => t.Category == catetory);
            if (!key.IsNullOrEmpty())       q = q.Where(t => t.Key == key);
            return q;
        }

        /// <summary>获取配置值</summary>
        public static string GetValue(string category, string key)
        {
            var config = Config.Search(category, key).FirstOrDefault();
            return (config == null) ? "" : config.Value;
        }

        /// <summary>设置配置值</summary>
        public static void SetValue(string category, string key, string value)
        {
            Config config = Config.Search(category, key).FirstOrDefault();
            if (config == null)
                new Config() { Key = key, Value = value, Category = category }.Save();
            else
            {
                config.Value = value;
                config.SetModified();
                config.Save();
            }
        }

    }
}