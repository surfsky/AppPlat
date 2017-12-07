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
    /// 订单状态
    /// </summary>
    public enum OrderStatus : int
    {
        [UI("草稿")]   Draft = 0,
        [UI("新建")]   New = 1,
        [UI("已支付")] Pay = 2,
        [UI("已取消")] Cancel = 3,
        [UI("已完成")] Complete = 4
    }

    /// <summary>
    /// 订单支付方式
    /// </summary>
    public enum OrderPayMode : int
    {
        [UI("现金")]       Cash = 0,
        [UI("支付宝")]     Alipay = 1,
        [UI("微信支付")]   WechatPay = 2,
        [UI("银联支付")]   ChinaUnionPay = 3,
        [UI("用户卡")]     UserCard = 4
    }

    /// <summary>
    /// 订单。
    /// 订单详情参看 OrderItem 表。
    /// 如果是简单的单商品订单系统，直接在summary上填写商品信息即可。
    /// </summary>
    public class Order : EntityBase<Order>
    {
        [UI("状态")]                   public OrderStatus? Sts { get; set; }
        [UI("用户")]                   public int? UserID { get; set; }
        [UI("创建时间")]               public DateTime? CreateDt { get; set; }
        [UI("订单流水号")]             public string SerialNo { get; set; }    // 微信支付生成预支付订单号，需要传入本系统订单号。 

        // 商品相关
        [UI("首商品")]                 public int? FirstProductID { get; set; } // 主商品（出于便捷考虑，此处填写订单的首条商品记录）
        [UI("概述")]                   public string Summary { get; set; }      // 概述，在订单生成时自动插入。格式如：xxxx 等n件商品
        [UI("商品数目")]               public int? ProductCnt { get; set; }=0;
        [UI("总费用")]                 public double? TotalMoney { get; set; }=0;

        // 支付相关
        [UI("支付时间")]               public DateTime? PayDt { get; set; }
        [UI("支付方式")]               public OrderPayMode? PayMode { get; set; } 
        [UI("实付费用")]               public double? PayMoney { get; set; }=0;
        [UI("支付卡")]                 public int? PayCardID { get; set; }

        // 导航属性
        public virtual User User { get; set; }
        public virtual Product FirstProduct { get; set; }

        //-----------------------------------------------
        // 公共方法
        //-----------------------------------------------
        /// <summary>生成订单流水号（22位）</summary>
        public static string BuildSerialNo()
        {
            return string.Format("{0:yyyyMMddHHmmssfffffff}{1}", DateTime.Now, new Random().Next(10));
        }

        // 根据SerialNo 订单流水号获取订单
        public static Order Get(string serialNo)
        {
            return Set.Include(s => s.FirstProduct).Include(s => s.User).Where(s => s.SerialNo == serialNo).FirstOrDefault();
        }
       

        // 查找
        public static IQueryable<Order> Search(
            OrderStatus? sts=null, string userName="", 
            DateTime? startDt=null, DateTime? endDt=null, 
            int? userId=null, string userMobile="", string serialNo=""
            )
        {
            IQueryable<Order> q = Set.Include(t => t.FirstProduct).Include(t => t.User);
            if (!String.IsNullOrEmpty(userName)) q = q.Where(t => t.User.Name.Contains(userName));
            if (!String.IsNullOrEmpty(userMobile)) q = q.Where(t => t.User.Mobile.Contains(userMobile));
            if (!String.IsNullOrEmpty(serialNo)) q = q.Where(t => t.SerialNo.Contains(serialNo));
            if (sts != null)                     q = q.Where(t => t.Sts == sts);
            if (startDt != null)                 q = q.Where(t => t.CreateDt >= startDt);
            if (endDt != null)                   q = q.Where(t => t.CreateDt <= endDt);
            if (userId != null && userId != -1)  q = q.Where(t => t.UserID == userId);

            return q;
        }

        //-----------------------------------------------
        // 订单处理流程
        // 若流程复杂化的话，创建OrderLog表来记录操作步骤
        //-----------------------------------------------
        /// <summary>获取订单详单</summary>
        public List<OrderItem> GetItems()
        {
            return OrderItem.Search(this.ID).ToList();
        }

        /// <summary>创建订单</summary>
        public static Order Create(int userId)
        {
            var order = new Order();
            order.Sts = OrderStatus.New;
            order.UserID = userId; 
            order.CreateDt = DateTime.Now;
            order.SerialNo = BuildSerialNo();
            order.Save();
            return order;
        }

        /// <summary>创建产品订单</summary>
        public static Order Create(int userId, int productId)
        {
            var order = Order.Create(userId);
            var product = Product.Get(productId);
            order.AddItem(product.Name, product.ID, product.Price, 1, product.Price);
            return order;
        }

        /// <summary>添加项目</summary>
        public void AddItem(string title, int? productId, double? price, int? cnt, double? money)
        {
            // 增加商品项目
            var oi = new OrderItem();
            oi.OrderID = this.ID;
            oi.Title = title;
            oi.ProductID = productId;
            oi.Price = price;
            oi.Cnt = cnt;
            oi.Money = money;
            oi.Save(false);

            // 修改商品销售数
            if (productId != null)
                Product.AddSaleCnt(productId.Value, cnt);

            // 更新订单信息
            this.ProductCnt++;
            if (productId != null)
            {
                if (this.FirstProductID == null)
                    this.FirstProductID = productId;
                this.FirstProduct = Product.Get(productId.Value);
            }
            var firstProductName = (this.FirstProduct == null) ? title : this.FirstProduct.Name;
            this.TotalMoney += money;
            this.PayMoney += money;
            this.Summary = string.Format("{0} 等{1}件商品", firstProductName, this.ProductCnt);
            this.Save(false);
        }



        /// <summary>支付</summary>
        public void Pay(OrderPayMode paymode, double? money, int? payCardId)
        {
            this.PayMode = paymode;
            this.PayDt = DateTime.Now;
            this.PayMoney = money;
            this.PayCardID = payCardId;
            this.Sts = OrderStatus.Pay;
            this.Save(false);
        }

        /// <summary>完成订单</summary>
        public void Complete()
        {
            this.Sts = OrderStatus.Complete;
            this.Save();
        }

        /// <summary>取消订单</summary>
        public void Cancel()
        {
            this.Sts = OrderStatus.Cancel;
            this.PayDt = DateTime.Now;
            this.Save();
        }

        /// <summary>批量取消订单</summary>
        public static void CancelBatch(List<int> ids)
        {
            Search(ids).ToList().ForEach(t => t.Cancel());
        }
    }
}