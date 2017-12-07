using App.Components;
using EntityFramework.Extensions;
using Kingsoc.Web.WebCall;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.Entity;


namespace App.DAL
{
    /// <summary>
    /// 消息类别
    /// </summary>
    public enum MessageType : int
    {
        // 网站内部消息
        [UI("系统消息")]         System = 0,
        [UI("顾客留言")]         GuestBook = 1,
        [UI("用户互发消息")]     Interact = 2,

        // 业务消息
        [UI("用户拜访提醒")]     UserVisit = 10,
        [UI("用户课程提醒")]     UserClass = 11,
        [UI("用户卡到期提醒")]   UserCard = 12,

        //
        [UI("其它")]             Others = 99
    }

    /// <summary>
    /// 消息通道
    /// </summary>
    public enum MessageWay
    {
        [UI("网页")]  Web = 0,
        [UI("短信")]  SMS = 1,
        [UI("微信")]  Wechat = 2,
        [UI("推送")]  App = 3,
        [UI("邮件")]  Mail = 4
    }

    /// <summary>
    /// 简短的消息（感觉和文章、文档、公文很相似）
    /// 非即时性的消息。
    /// </summary>
    public class Message : EntityBase<Message>
    {
        [UI("消息类别")]  public MessageType? Type { get; set; }
        [UI("消息通道")]  public MessageWay? Way { get; set; }
        [UI("标题")]      public string Title { get; set; }
        [UI("内容")]      public string Content { get; set; }
        [UI("URL")]       public string   URL { get; set; }
        [UI("From")]      public int?   SenderID { get; set; }
        [UI("To")]        public int?   ReceiverID { get; set; }

        [UI("时间")]      public DateTime CreateDt { get; set; }
        [UI("预约时间")]  public DateTime? AssignDt { get; set; }
        [UI("发送时间")]  public DateTime? SendDt { get; set; }
        [UI("是否成功")]  public bool? IsSuccess { get; set; }

        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }

        //-----------------------------------------------
        // 公共方法
        //-----------------------------------------------
        // 新增
        public static Message Add()
        {
            var item = new Message() { CreateDt = DateTime.Now };
            item.Save();
            return item;
        }

        // 查询
        public static IQueryable<Message> Search(MessageType? type, string title, DateTime? startDt, DateTime? endDt, int? senderID, int? receiverID)
        {
            IQueryable<Message> q = Set.Include(t => t.Sender).Include(t => t.Receiver);
            if (type != null)                  q = q.Where(t => t.Type == type);
            if (!String.IsNullOrEmpty(title))  q = q.Where(t => t.Title.Contains(title));
            if (startDt != null)               q = q.GreaterEqual(t => t.CreateDt, startDt.Value);
            if (endDt != null)                 q = q.LessEqual(t => t.CreateDt, endDt.Value);
            if (senderID != null)              q = q.Where(t => t.SenderID == senderID);
            if (receiverID != null)            q = q.Where(t => t.ReceiverID == receiverID);
            return q;
        }
    }
}