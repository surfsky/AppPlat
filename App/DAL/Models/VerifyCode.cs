using App.Components;
using EntityFramework.Extensions;
using Kingsoc.Web.WebCall;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace App.DAL
{
    /// <summary>
    /// 短信验证码
    /// </summary>
    public class VerifyCode : DbBase<VerifyCode>
    {
        [UI("验证码")]                          public string Code { get; set; }
        [UI("手机号")]                          public string Mobile { get; set; }
        [UI("创建时间")]                        public DateTime CreateDt { get; set; }
        [UI("过期时间")]                        public DateTime ExpireDt { get; set; }
        [UI("哪里请求的验证码")]                public string Source { get; set; }

        //-----------------------------------------------
        // 公共方法
        //-----------------------------------------------
        /// <summary>查询</summary>
        public static IQueryable<VerifyCode> Search(string mobile, DateTime? startDt=null, DateTime? endDt=null)
        {
            IQueryable<VerifyCode> q = Set;
            if (!String.IsNullOrEmpty(mobile)) q = q.Where(t => t.Mobile.Contains(mobile));
            if (startDt != null)               q = q.Where(t => t.CreateDt >= startDt);
            if (endDt != null)                 q = q.Where(t => t.CreateDt <= endDt);
            return q;
        }

        /// <summary>批量删除</summary>
        public static void DeleteBatch(int months)
        {
            var date = DateTime.Now.AddMonths(-months);
            Set.Where(t => t.CreateDt<date).Delete();
        }
        /// <summary>
        ///  获取验证码
        /// </summary> 
        public static VerifyCode GetDetail(string mobile)
        {
            return Search(mobile, DateTime.Now.AddMinutes(-10)).OrderByDescending(s => s.ExpireDt).FirstOrDefault();
        }

    }
}