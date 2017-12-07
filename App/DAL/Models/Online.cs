using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using App.Components;


namespace App.DAL
{
    /// <summary>
    /// 在线信息
    /// </summary>
    public class Online : DbBase<Online>
    {
        [UI("最后访问的IP地址")]   public string IP { get; set; }
        [UI("用户最后登录时间")]   public DateTime? LoginDt { get; set; }
        [UI("最后操作更新时间")]   public DateTime? UpdateDt { get; set; }
        [UI("用户")]               public int? UserID { get; set; }

        public virtual User User { get; set; }

        //-----------------------------------------------
        // 公共方法
        //-----------------------------------------------
        // 搜索在线用户
        public static IQueryable<Online> Search(string name)
        {
            DateTime lastDt = DateTime.Now.AddHours(-2);
            IQueryable<Online> q = Set.Include(o => o.User);
            if (!String.IsNullOrEmpty(name))
                q = q.Where(o => o.User.Name.Contains(name));
            q = q.Where(o => o.UpdateDt > lastDt);
            return q;
        }

        // 注册在线用户（由于日志过于频繁且低价值，不记录到日志中去了）
        public static void RegisterOnlineUser(int userId)
        {
            var now = DateTime.Now;
            Online online = Set.Where(o => o.User.ID == userId).FirstOrDefault();
            if (online != null)
            {
                online.UserID = userId;
                online.IP = Asp.GetClientIP();
                online.LoginDt = now;
                online.UpdateDt = now;
                online.Save(false);
            }
            else
            {
                online = new Online();
                online.UserID = userId;
                online.IP = Asp.GetClientIP();
                online.LoginDt = now;
                online.UpdateDt = now;
                online.SaveNew(false);
            }

            // 记录本次更新时间
            HttpContext.Current.Session[Common.SESSION_ONLINE_UPDATE_TIME] = now;
        }

        // 更新在线用户（刷新在线表-指定用户的最后更新时间）
        public static void UpdateOnlineUser(string username)
        {
            DateTime now = DateTime.Now;
            object lastUpdateDt = HttpContext.Current.Session[Common.SESSION_ONLINE_UPDATE_TIME];
            if (lastUpdateDt == null || (Convert.ToDateTime(lastUpdateDt).Subtract(now).TotalMinutes > 5))
            {
                // 记录本次更新时间；更新到数据库（若不存在则插入）
                HttpContext.Current.Session[Common.SESSION_ONLINE_UPDATE_TIME] = now;
                Online online = Set.Where(o => o.User.Name == username).FirstOrDefault();
                if (online != null)
                {
                    online.UpdateDt = now;
                    online.Save(false);
                }
            }
        }

        /// <summary>获取在线人数（15分钟内活动的用户），加了缓存</summary>
        public static int GetOnlineCount()
        {
            return (int)Asp.GetCacheData(
                "OnlineCount",
                DateTime.Now.AddMinutes(5),
                () => {
                    DateTime lastDt = DateTime.Now.AddMinutes(-15);
                    return Set.Where(o => o.UpdateDt > lastDt).Count();
                });
        }
    }
}