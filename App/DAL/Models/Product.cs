using EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using App.Components;

namespace App.DAL
{
    /// <summary>
    /// 商品类别（更复杂的话，用商品目录表替代）
    /// </summary>
    public enum ProductType : int
    {
        [UI("计天卡", "DoorCard")] DayCard = 0,
        [UI("计次卡", "DoorCard")] TimesCard = 1,
        [UI("私教卡", "Card")] PrivateCard = 2,
        [UI("食品",   "Normal")] Food = 10,
        [UI("装备",   "Normal")] Equipment = 11,
        [UI("其它")]   Other = 99
    }

    /// <summary>
    /// 商品。
    /// 简化的商品模型。不同规格包装色彩的商品算不同商品，要记录多条。
    /// </summary>
    public class Product : EntityBase<Product>
    {
        // 基础属性
        [UI("创建日期")]     public DateTime? CreateDt { get; set; }
        [UI("商品类型")]     public ProductType? Type { get; set; }
        [UI("是否上架")]     public bool? OnShelf { get; set; } = true;
        [UI("名称")]         public string Name { get; set; }
        [UI("描述")]         public string Description { get; set; }
        [UI("条码")]         public string BarCode { get; set; }
        [UI("数量")]         public int? Amount { get; set; } = 0;        // 个数、月数或次数
        [UI("图片")]         public string CoverImage { get; set; }
        [UI("限售商品")]     public bool? IsLimit { get; set; }=false;  // 2017-10 新增。限售商品只能由营业员在前台售出，客户不能在app、微信直接购买

        //
        [UI("门店")]         public int? StoreID { get; set; }            // 门店零售商品
        [UI("拥有人")]       public int? OwnerID { get; set; }            // 私教开的课程

        // 销售及评分
        [UI("销售数")]       public int? SaleCnt { get; set; } = 0;
        [UI("好评数")]       public int? PositiveCnt { get; set; } = 0;

        // 规格价格库存
        [UI("价格")]         public double? Price { get; set; } = 0;
        [UI("原价")]         public double? RawPrice { get; set; } = 0;
        [UI("最低价")]       public double? LowestPrice { get; set; } = 0;
        [UI("库存")]         public int? StockCnt { get; set; } = 0;
        [UI("折扣"), NotMapped]
        public string Discount
        {
            get
            {
                if (RawPrice == null || RawPrice == 0.0) return "";
                else return string.Format("{0:0.0}折", Price.Value * 10.0 / RawPrice.Value);
            }
        }

        // 导航属性
        public virtual Store Store { get; set; }
        public virtual User Owner { get; set; }

        [NotMapped]
        public string Summary
        {
            get
            {
                string ownerText = (Owner != null) ? ", " + Owner.NickName : "";
                string limitText = (IsLimit.HasValue && IsLimit.Value) ? ", 限售" : "";
                string onshelfText = (OnShelf.HasValue && OnShelf.Value) ? "" : ", 已下架";
                return string.Format("{0} ({1}{2}{3}{4})", Name, Type.GetDescription(), onshelfText, limitText, ownerText);
            }
        }


        //-----------------------------------------------
        // 公共方法
        //-----------------------------------------------
        /// <summary>下架</summary>
        public static void OffShelf(List<int> ids)
        {
            Set.Where(t => ids.Contains(t.ID)).Update(t => new Product { OnShelf = false });
        }

        /// <summary>根据条码获取商品</summary>
        public static Product GetByCode(string barCode)
        {
            if (barCode.IsNullOrEmpty()) return null;
            else                         return Set.Where(t => t.BarCode.Contains(barCode)).FirstOrDefault();
        }

        // 查找
        public static IQueryable<Product> Search(string name="", ProductType? type=null, bool? onShelf=null, int? storeId=null, int? ownerUID=null, bool? isLimit=null)
        {
            IQueryable<Product> q = Set;
            if (!String.IsNullOrEmpty(name))    q = q.Where(t => t.Name.Contains(name));
            if (type != null)                   q = q.Where(t => t.Type == type);
            if (onShelf != null)                q = q.Where(t => t.OnShelf == onShelf);
            if (storeId != null)                q = q.Where(t => t.StoreID == storeId);
            if (ownerUID != null)               q = q.Where(t => t.OwnerID == ownerUID);
            if (isLimit != null)                q = q.Where(t => t.IsLimit == isLimit);
            return q;
        }


        /// <summary>获取所有卡</summary>
        public static List<Product> GetCards(bool? onShelf = null)
        {
            IQueryable<Product> q = Set.Where(t => t.Type == ProductType.DayCard || t.Type == ProductType.TimesCard || t.Type == ProductType.PrivateCard);
            if (onShelf != null) q = q.Where(t => t.OnShelf == onShelf || t.OnShelf == null);
            q = q.OrderBy(t => t.Type).ThenBy(t => t.Name);
            return q.ToList();
        }

        /// <summary>查找客户端可用的卡列表（非限售、上架、卡类商品）</summary>
        public static List<Product> GetClientCards()
        {
            return Set
                .Where(t => t.Type == ProductType.DayCard || t.Type == ProductType.TimesCard)
                .Where(t => t.OnShelf == true || t.OnShelf == null)
                .Where(s => s.IsLimit == false || s.IsLimit == null)
                .OrderBy(t => t.Type).ThenBy(t => t.Name)
                .ToList()
                ;
        }

        /// <summary>增加商品销售数目</summary>
        public static void AddSaleCnt(int productId, int? cnt)
        {
            var product = Product.Get(productId);
            if (product != null)
            {
                product.SaleCnt += cnt;
                product.Save(false);
            }
        }
    }
}