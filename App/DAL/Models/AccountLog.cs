using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EntityFramework.Extensions;
using System.Data.Entity;
using System.ComponentModel;
using App.Components;


namespace App.DAL
{
    /// <summary>
    /// 账目流水类型
    /// </summary>
    public enum AccountLogType : int
    {
        [UI("预存")] Prestore = 0,
        [UI("提现")] Withdraw = 1,
        [UI("消费")] Consume = 2,
        [UI("提成")] Bonus = 3,
    }

    /// <summary>
    /// 用户账户流水
    /// </summary>
    public class AccountLog : EntityBase<AccountLog>
    {
        [UI("类型")]                     public AccountLogType Type { get; set; }
        [UI("创建时间")]                 public DateTime? CreateDt { get; set; }
        [UI("费用")]                     public float? Money { get; set; }
        [UI("用户")]                     public int? UserID { get; set; }
        [UI("订单")]                     public int? OrderID { get; set; }
        [UI("备注")]                     public string Remark { get; set; }
        [UI("帐户")]                     public int? AccountID { get; set; }

        public virtual Account Account { get; set; }


        //-----------------------------------------------
        // 公共方法
        //-----------------------------------------------
        public new static AccountLog GetDetail(int id)
        {
            return Set.Include(t => t.Account).Where(t => t.ID == id).FirstOrDefault();
        }
    }
}