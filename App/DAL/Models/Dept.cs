using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using EntityFramework.Extensions;
using App.Components;

namespace App.DAL
{
    /// <summary>
    /// 部门表
    /// </summary>
    public class Dept : EntityBase<Dept>, ITree, ICloneable
    {
        [UI("名称")]          public string Name { get; set; }
        [UI("排序")]          public int Seq { get; set; }
        [UI("备注")]          public string Remark { get; set; }
        [UI("父部门")]        public int? ParentID { get; set; }

        public virtual Dept Parent { get; set; }
        public virtual ICollection<Dept> Children { get; set; }
        public virtual ICollection<User> Users { get; set; }

        [NotMapped]                           public bool Enabled { get; set; }           // <summary>是否可用（默认true）,在模拟树的下拉列表中使用</summary>
        [NotMapped]                           public int TreeLevel { get; set; }          // <summary>菜单在树形结构中的层级（从0开始）</summary>
        [NotMapped]                           public bool IsTreeLeaf { get; set; }        // <summary>是否叶子节点（默认true）</summary>

        // 克隆
        public object Clone()
        {
            Dept item = new Dept
            {
                ID = ID,
                Name = Name,
                Remark = Remark,
                Seq = Seq,
                TreeLevel = TreeLevel,
                Enabled = Enabled,
                IsTreeLeaf = IsTreeLeaf
            };
            return item;
        }


        //-----------------------------------------------
        // 公共方法
        //-----------------------------------------------
        // 缓存数据
        private static List<Dept> _items;
        public static List<Dept> All
        {
            get
            {
                if (_items == null)
                    Reload();
                return _items;
            }
        }

        // 重构建缓存数据
        public static void Reload()
        {
            _items = new List<Dept>();
            var items = Set.OrderBy(d => d.Seq).ToList();
            BuildDeptTree(items, null, 0);
        }

        // 递归处理，弄成树状的
        private static int BuildDeptTree(List<Dept> items, Dept parentItem, int level)
        {
            int count = 0;
            foreach (var item in items.Where(d => d.Parent == parentItem))
            {
                count++;
                _items.Add(item);
                item.TreeLevel = level;
                item.IsTreeLeaf = true;
                item.Enabled = true;

                level++;
                // 如果这个节点下没有子节点，则这是个终结节点
                int childCount = BuildDeptTree(items, item, level);
                if (childCount != 0)
                    item.IsTreeLeaf = false;
                level--;
            }
            return count;
        }

        //-----------------------------------------------------
        // 其它方法
        //-----------------------------------------------------
        /// <summary>获取详情（包括父节点）</summary>
        public new static Dept GetDetail(int id)
        {
            return Set.Include(t => t.Parent).Where(t => t.ID == id).FirstOrDefault();
        }


        /// <summary>递归删除（及子部门、部门用户）</summary>
        public new static void DeleteRecursive(int id)
        {
            // 删除附属表数据
            Set.Include(t => t.Users).Where(t => t.ID == id).ToList().ForEach(t => t.Users = null);
            Db.SaveChanges();

            // 删除子部门
            var children = Set.Where(m => m.Parent.ID == id).ToList();
            foreach (var child in children)
                DeleteRecursive(child.ID);

            // 删除自身
            Set.Where(t => t.ID == id).Delete();
        }

    }
}