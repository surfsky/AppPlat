using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EntityFramework.Extensions;
using System.Data.Entity;
using App.Components;

namespace App.DAL
{
    /// <summary>
    /// 区域表（全国）
    /// </summary>
    public class Area : DbBase<Area>, ITree, ICloneable
    {
        [UI("名称")]                   public string Name { get; set; }
        [UI("顺序")]                   public int? Seq { get; set; }
        [UI("备注")]                   public string Remark { get; set; }
        [UI("全称")]                   public string FullName { get; set; }        // 全名，如：浙江温州鹿城区。后台编辑时自动生成

        [UI("父区域")]                 public virtual Area Parent { get; set; }
        [UI("子区域集合")]             public virtual ICollection<Area> Children { get; set; }

        [NotMapped] public int TreeLevel { get; set; }          // <summary>菜单在树形结构中的层级（从0开始）</summary>
        [NotMapped] public bool IsTreeLeaf { get; set; }        // <summary>是否叶子节点（默认true）</summary>
        [NotMapped] public bool Enabled { get; set; }           // <summary>是否可用（默认true）,在模拟树的下拉列表中使用</summary>


        // 克隆
        public object Clone()
        {
            Area item = new Area
            {
                ID = ID,
                Name = Name,
                Remark = Remark,
                Seq = Seq,
                FullName = FullName,
                Enabled = Enabled,
                TreeLevel = TreeLevel,
                IsTreeLeaf = IsTreeLeaf
            };
            return item;
        }


        //-----------------------------------------------
        // 公共方法
        //-----------------------------------------------
        // 缓存数据
        private static List<Area> _items;
        public static List<Area> All
        {
            get
            {
                if (_items == null)
                    Reload();
                return _items;
            }
        }

        // 重加载数据
        public static void Reload()
        {
            _items = new List<Area>();
            List<Area> items = Set.OrderBy(d => d.Seq).ToList();
            BuildTree(items, null, 0);
        }

        // 递归处理部门，弄成树状的(items -> _items)
        private static int BuildTree(List<Area> items, Area parentItem, int level)
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
                int childCount = BuildTree(items, item, level);
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
        public new static Area GetDetail(int id)
        {
            return Set.Include(t => t.Parent).Where(t => t.ID == id).FirstOrDefault();
        }


        /// <summary>递归删除部门（及子部门、部门用户）</summary>
        public new static void DeleteRecursive(int id)
        {
            // 删除子节点
            var children = Set.Where(m => m.Parent.ID == id).ToList();
            foreach (var child in children)
                DeleteRecursive(child.ID);

            // 删除自身
            Set.Where(t => t.ID == id).Delete();
        }

    }
}