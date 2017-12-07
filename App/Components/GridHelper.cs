using FineUI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL;
using App.Components;
using System.Data.Entity.Core.Objects;

namespace App.Components
{
    /// <summary>
    /// FineUI Grid 辅助操作类
    /// </summary>
    public static class GridHelper
    {
        //-----------------------------------------
        // 网格分页和排序
        //-----------------------------------------
        // 排序分页后显示
        public static void BindGrid<T>(this Grid grid, IQueryable<T> query)
        {
            IQueryable<T> q = SortAndPage(query, grid);
            grid.DataSource = q;
            grid.DataBind();
        }

        public static IQueryable<T> SortAndPage<T>(this IQueryable<T> query, Grid grid)
        {
            grid.RecordCount = query.Count();
            grid.PageIndex = MathHelper.Limit(grid.PageIndex, 0, grid.PageCount - 1);
            var q = query.SortAndPage(grid.SortField, grid.SortDirection, grid.PageIndex, grid.PageSize);
            return q;
        }



        //-----------------------------------------
        // ID 相关
        //-----------------------------------------
        /// <summary>获取选择行主键 ID</summary>
        public static int GetSelectedId(Grid grid)
        {
            int id = -1;
            int rowIndex = grid.SelectedRowIndex;
            if (rowIndex >= 0)
                id = Convert.ToInt32(grid.DataKeys[rowIndex][0]);
            return id;
        }

        /// <summary>获取选择行主键 ID 列表</summary>
        public static List<int> GetSelectedIds(Grid grid)
        {
            List<int> ids = new List<int>();
            foreach (int rowIndex in grid.SelectedRowIndexArray)
                ids.Add(Convert.ToInt32(grid.DataKeys[rowIndex][0]));
            return ids;
        }


        //-----------------------------------------
        // 选择行ID 与隐藏域
        //-----------------------------------------
        /// <summary>将表格当前页面选中行对应的数据保存到隐藏字段中（有空简化下）</summary>
        public static void SaveSelectedIds(Grid grid, FineUI.HiddenField hiddenField)
        {
            List<int> ids = GetIdsFromHidden(hiddenField);
            List<int> selectedRows = new List<int>();
            if (grid.SelectedRowIndexArray != null && grid.SelectedRowIndexArray.Length > 0)
                selectedRows = new List<int>(grid.SelectedRowIndexArray);

            if (grid.IsDatabasePaging)
            {
                //int count = Math.Min(grid.PageSize, (grid.RecordCount - grid.PageIndex * grid.PageSize));
                int count = MathHelper.Min(grid.PageSize, (grid.RecordCount - grid.PageIndex * grid.PageSize), grid.DataKeys.Count);
                for (int i = 0; i < count; i++)
                {
                    int id = Convert.ToInt32(grid.DataKeys[i][0]);
                    if (selectedRows.Contains(i))
                    {
                        if (!ids.Contains(id))
                            ids.Add(id);
                    }
                    else
                    {
                        if (ids.Contains(id))
                            ids.Remove(id);
                    }
                }
            }
            else
            {
                int startPageIndex = grid.PageIndex * grid.PageSize;
                int count = MathHelper.Min(startPageIndex + grid.PageSize, grid.RecordCount, grid.DataKeys.Count);
                for (int i = startPageIndex; i < count; i++)
                {
                    int id = Convert.ToInt32(grid.DataKeys[i][0]);
                    if (selectedRows.Contains(i - startPageIndex))
                    {
                        if (!ids.Contains(id))
                            ids.Add(id);
                    }
                    else
                    {
                        if (ids.Contains(id))
                            ids.Remove(id);
                    }
                }
            }

            hiddenField.Text = new JArray(ids).ToString(Newtonsoft.Json.Formatting.None);
        }

        /// <summary>根据隐藏字段的数据更新表格当前页面的选中行</summary>
        public static void ShowSelectedIds(Grid grid, HiddenField hiddenField)
        {
            List<int> ids = GetIdsFromHidden(hiddenField);
            List<int> nextSelectedRowIndexArray = new List<int>();
            if (grid.IsDatabasePaging)
            {
                for (int i = 0, count = Math.Min(grid.PageSize, (grid.RecordCount - grid.PageIndex * grid.PageSize)); i < count; i++)
                {
                    int id = Convert.ToInt32(grid.DataKeys[i][0]);
                    if (ids.Contains(id))
                        nextSelectedRowIndexArray.Add(i);
                }
            }
            else
            {
                int nextStartPageIndex = grid.PageIndex * grid.PageSize;
                for (int i = nextStartPageIndex, count = Math.Min(nextStartPageIndex + grid.PageSize, grid.RecordCount); i < count; i++)
                {
                    int id = Convert.ToInt32(grid.DataKeys[i][0]);
                    if (ids.Contains(id))
                        nextSelectedRowIndexArray.Add(i - nextStartPageIndex);
                }
            }
            grid.SelectedRowIndexArray = nextSelectedRowIndexArray.ToArray();
        }

        /// <summary>从隐藏字段中获取选择的全部ID列表</summary>
        private static List<int> GetIdsFromHidden(FineUI.HiddenField hiddenField)
        {
            JArray idsArray = new JArray();
            string currentIDS = hiddenField.Text.Trim();
            if (!String.IsNullOrEmpty(currentIDS))
                idsArray = JArray.Parse(currentIDS);
            else
                idsArray = new JArray();
            return new List<int>(idsArray.ToObject<int[]>());
        }
    }
}