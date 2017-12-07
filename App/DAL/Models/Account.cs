using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EntityFramework.Extensions;
using System.Data.Entity;
using App.Components;

namespace App.DAL
{
    /// <summary>
    /// 用户账户表
    /// </summary>
    public class Account : DbBase<Account>
    {
        [UI("余额")]                     public string Balance { get; set; }
        [UI("总收入")]                   public float? TotalIncome { get; set; } = 0;
        [UI("总支出")]                   public float? TotalOutcome { get; set; } = 0;
        [UI("用户ID")]                   public int? UserID { get; set; }

        public virtual User User { get; set; }
        public virtual List<AccountLog> Logs { get; set; }

        //-----------------------------------------------
        // 公共方法
        //-----------------------------------------------
        public new static Account GetDetail(int id)
        {
            return Set.Include(t => t.User).Include(t =>t.Logs).Where(t => t.ID == id).FirstOrDefault();
        }

        /// <summary>根据用户ID获取（或新建一个）</summary>
        public static Account GetOrCreate(int userId)
        {
            var item = GetByUserId(userId);
            if (item == null)
            {
                item = new Account();
                item.UserID = userId;
                item.SaveNew();
            }
            return item;
        }

        public static Account GetByUserId(int userId)
        {
            return Account.Set.Where(t => t.UserID == userId).FirstOrDefault();
        }
    }
}