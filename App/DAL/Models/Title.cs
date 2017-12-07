using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EntityFramework.Extensions;
using App.Components;


namespace App.DAL
{
    /// <summary>
    /// 职称（头衔）表
    /// </summary>
    public class Title : EntityBase<Title>
    {
        [UI("职称（头衔）"), StringLength(50)]  public string Name { get; set; }
        [UI("备注"), StringLength(500)]         public string Remark { get; set; }
        public virtual ICollection<User> Users { get; set; }

        //-----------------------------------------------
        // 公共方法
        //-----------------------------------------------
        // 查找
        public static IQueryable<Title> Search(string name)
        {
            IQueryable<Title> q = Set;
            if (!String.IsNullOrEmpty(name))
                q = q.Where(t => t.Name.Contains(name));
            return q;
        }

        // 根据id列表获取
        public static List<Title> GetTitles(List<int> titleIds)
        {
            return Set.Where(t => titleIds.Contains(t.ID)).ToList();
        }


        /// <summary>删除职位和相关数据（如有该职务的用户）</summary>
        public static void DeleteTitle(int titleID)
        {
            Set.Where(t => t.ID == titleID).ToList().ForEach(t => t.Users = null);
            Set.Where(t => t.ID == titleID).Delete();
        }

        /// <summary>删除职位和相关数据（如有该职务的用户）</summary>
        public static void DeleteBatch(List<int> ids)
        {
            Set.Where(t => ids.Contains(t.ID)).ToList().ForEach(t => t.Users = null);
            Set.Where(t => ids.Contains(t.ID)).Delete();
        }

    }
}