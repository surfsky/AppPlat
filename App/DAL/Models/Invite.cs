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
    /// 邀请来源
    /// </summary>
    public enum InviteSource : int
    {
        [UI("地推")]  Offline = 0,
        [UI("网站")]  Web = 1,
        [UI("微信")]  WeiXin = 2,
        [UI("App")]   App  = 3
    }

    /// <summary>
    /// 邀请状态
    /// </summary>
    public enum InviteStatus : int
    {
        [UI("新推荐")]  New = 0,
        [UI("已取消")]  Cancel = 1,
        [UI("已拜访")]  Visit = 2,
        [UI("已注册")]  Regist = 3
    }

    /// <summary>
    /// 邀请信息表
    /// (1）该表可用于推广营销登记用，用于推广结算
    ///（2）推荐时先查找该用户手机是否已经注册；再查找本表看该手机是否已经被别人推荐了；实在没有才新增一条记录。
    /// </summary>
    public class Invite : EntityBase<Invite>
    {
        [UI("来源")]                           public InviteSource? Source { get; set; }
        [UI("状态")]                           public InviteStatus? Sts { get; set; }
        [UI("邀请人")]                         public int? InviterID { get; set; }
        [UI("受邀者账户")]                     public int? InviteeID { get; set; }
        [UI("受邀者手机")]                     public string InviteeMobile { get; set; }
        [UI("创建日期")]                       public DateTime? CreateDt { get; set; }
        [UI("电访日期")]                       public DateTime? CallDt {get;set;}
        [UI("预约日期")]                       public DateTime? AppointmentDt { get; set; }
        [UI("实际拜访日期")]                   public DateTime? VisitDt { get; set; }
        [UI("新用户注册日期")]                 public DateTime? RegistDt { get; set; }
        [UI("提醒日期")]                       public DateTime? NotifyDt {get; set;}
        [UI("备注")]                           public string Remark {get; set;}
        

        public virtual User Inviter { get; set; }
        public virtual User Invitee { get; set; }

        //-----------------------------------------------
        // 公共方法
        //-----------------------------------------------
        public new static Invite GetDetail(int id)
        {
            return Set.Include(t => t.Inviter).Include(t => t.Invitee).Where(t => t.ID == id).FirstOrDefault();
        }


        // 查询
        public static IQueryable<Invite> Search(
            int? inviterID=null, int? inviteeID = null, 
            DateTime? appointmentStartDt = null, DateTime? appointmentEndDt = null,
            DateTime? createStartDt = null, DateTime? createEndDt = null,
            InviteSource? source = null, InviteStatus? sts = null, bool? hasNotify = null
            )
        {
            IQueryable<Invite> q = Set.Include(t => t.Inviter).Include(t => t.Invitee);
            if (inviterID != null)             q = q.Where(t => t.InviterID == inviterID);
            if (inviteeID != null)             q = q.Where(t => t.InviteeID == inviteeID);
            if (createStartDt != null)         q = q.Where(t => t.CreateDt >= createStartDt);
            if (createEndDt != null)           q = q.Where(t => t.CreateDt <= createEndDt);
            if (appointmentStartDt != null)    q = q.Where(t => t.AppointmentDt >= appointmentStartDt);
            if (appointmentEndDt != null)      q = q.Where(t => t.AppointmentDt <= appointmentEndDt);
            if (source != null)                q = q.Where(t => t.Source == source);
            if (sts != null)                   q = q.Where(t => t.Sts == sts);
            if (hasNotify != null)
            {
                if (hasNotify.Value) q = q.Where(t => t.NotifyDt != null);
                else                 q = q.Where(t => t.NotifyDt == null);
            }
            return q;
        } 
    }
}