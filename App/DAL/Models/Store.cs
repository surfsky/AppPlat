using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.Entity;
using App.Components;

namespace App.DAL
{
    /// <summary>
    /// 门店、商店
    /// </summary>
    public class Store : EntityBase<Store>
    {
        [UI("城市")]                public int? CityID { get; set; }
        [UI("名称")]                public string Name { get; set; }
        [UI("地址")]                public string Addr { get; set; }
        [UI("GPS(x,y)")]            public string GPS { get; set; }
        [UI("电话")]                public string Tel { get; set; }
        [UI("封面图片")]            public string CoverImage { get; set; } = "~/Res/images/nopicture.jpg";//默认图片
        [UI("门禁MAC地址")]         public string DoorMac { get; set; }

        public virtual Area City { get; set; }

        //-----------------------------------------------
        // 公共方法
        //-----------------------------------------------
        // 查找
        public static IQueryable<Store> Search(string name="", int? cityId=null)
        {
            IQueryable<Store> q = Set.Include(t => t.City);
            if (!String.IsNullOrEmpty(name))  q = q.Where(t => t.Name.Contains(name));
            if (cityId != null)               q = q.Where(t => t.CityID == cityId);
            return q;
        }
    }
}