using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.Entity;
using System.ComponentModel;
using App.Components;

namespace App.DAL
{
    /// <summary>
    /// 订单详情表
    /// </summary>
    public class OrderItem : EntityBase<OrderItem>
    {
        [UI("订单")]                   public int? OrderID { get; set; }
        [UI("明目")]                   public string Title { get; set; }
        [UI("商品")]                   public int? ProductID { get; set; }
        [UI("单价")]                   public double? Price { get; set; }
        [UI("数量")]                   public int? Cnt { get; set; }
        [UI("金额")]                   public double? Money { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }

        //-----------------------------------------------
        // 公共方法
        //-----------------------------------------------
        // 查找
        public static IQueryable<OrderItem> Search(int? orderId, string title="", int? productId=null)
        {
            IQueryable<OrderItem> q = Set.Include(t => t.Order).Include(t => t.Product);
            if (orderId != null)        q = q.Where(t => t.OrderID == orderId);
            if (productId != null)      q = q.Where(t => t.ProductID == productId);
            if (!title.IsNullOrEmpty()) q = q.Where(t => t.Title.Contains(title));
            return q;
        }
    }
}