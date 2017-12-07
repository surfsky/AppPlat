using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EntityFramework.Extensions;
using System.Data.Entity;
using App.Components;

namespace App.DAL
{
    /// <summary>
    /// 系统菜单
    /// </summary>
    public class Menu : EntityBase<Menu>, ITree, ICloneable
    {
        [UI("名称"), Required, StringLength(50)]     public string Name { get; set; }
        [UI("图片URL"), StringLength(200)]           public string ImageUrl { get; set; }
        [UI("导航URL"), StringLength(200)]           public string NavigateUrl { get; set; }
        [UI("备注"), StringLength(500)]              public string Remark { get; set; }
        [UI("顺序"), Required]                       public int Seq { get; set; }
        [UI("是否展开")]                             public bool IsOpen { get; set; } = false;
        [UI("是否可见")]                             public bool Visible { get; set; } = true;
        [UI("父菜单")]                               public int? ParentID { get; set; }


        public virtual Menu Parent { get; set; }
        public virtual ICollection<Menu> Children { get; set; }
        public virtual PowerType? ViewPower {get; set;}

        [NotMapped] public int TreeLevel { get; set; }     // 菜单在树形结构中的层级（从0开始）
        [NotMapped] public bool IsTreeLeaf { get; set; }   // 是否叶子节点（默认true）
        [NotMapped] public bool Enabled { get; set; }      // 是否可用（默认true）,在模拟树的下拉列表中使用


        // 克隆
        public object Clone()
        {
            return new Menu {
                ID = ID,
                Name = Name,
                ImageUrl = ImageUrl,
                NavigateUrl = NavigateUrl,
                Remark = Remark,
                Seq = Seq,
                TreeLevel = TreeLevel,
                Enabled = Enabled,
                IsTreeLeaf = IsTreeLeaf,
                IsOpen = IsOpen,
                Visible = Visible
            };
        }


        //-----------------------------------------------
        // 公共方法
        //-----------------------------------------------
        // 所有菜单
        private static List<Menu> _items;
        public static List<Menu> All
        {
            get
            {
                if (_items == null)
                    Reload();
                return _items;
            }
        }

        // 重新加载
        public static void Reload()
        {
            _items = new List<Menu>();
            var item = Set.OrderBy(m => m.Seq).ToList();
            BuildMenuTree(item, null, 0);
        }

        // 递归处理菜单，弄成树状的
        static int BuildMenuTree(List<Menu> items, Menu parentItem, int level)
        {
            int count = 0;
            foreach (var menu in items.Where(m => m.Parent == parentItem))
            {
                count++;
                _items.Add(menu);
                menu.TreeLevel = level;
                menu.IsTreeLeaf = true;
                menu.Enabled = true;

                level++;
                int childCount = BuildMenuTree(items, menu, level);
                if (childCount != 0)
                    menu.IsTreeLeaf = false;
                level--;
            }
            return count;
        }

        //-----------------------------------------------------
        // 其它方法
        //-----------------------------------------------------
        // 获取菜单详情（包括父节点和访问权限）
        public new static DAL.Menu GetDetail(int id)
        {
            return Set
                .Include(m => m.Parent)
                .Where(m => m.ID == id)
                .FirstOrDefault();
        }


        /// <summary>递归删除菜单（及子菜单）</summary>
        public new static void DeleteRecursive(int id)
        {
            var children = Set.Where(m => m.Parent.ID == id).ToList();
            foreach (var child in children)
                DeleteRecursive(child.ID);
            Set.Where(t => t.ID == id).Delete();
        }

    }
}