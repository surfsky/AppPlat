using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUI;
using System.Data;
using Newtonsoft.Json.Linq;
using AspNet = System.Web.UI.WebControls;
using App.DAL;
using App.Components;
using System.Collections;
using EntityFramework.Extensions;

namespace App.Admins
{
    /// <summary>
    /// 角色权限管理。
    /// - 本页面仅管理员可用。
    /// - 无授权再管理功能（如：A授权B，B授权C，不予实现）。
    /// - 全选功能在服务器端无法实现，已改为在客户端实现（2017-08 SURFSKY）
    /// - 角色清单和权限清单来自枚举，不再从数据库中取（2017-11 SURFSKY)
    /// </summary>
    [Auth(PowerType.RolePowerEdit)]
    public partial class RolePowers : PageBase
    {
        private Dictionary<PowerType, bool> _powers = new Dictionary<PowerType, bool>();


        //---------------------------------------------------
        // Init
        //---------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            Grid1.PageSize = SiteConfig.PageSize;
            BindGrid1();
            Grid1.SelectedRowIndex = 0;
            Grid2.PageSize = SiteConfig.PageSize;
            BindGrid2();
        }

        private void BindGrid1()
        {
            var roles = DAL.User.AllRoles;
            //var q = Role.Set.SortBy(Grid1.SortField, Grid1.SortDirection);
            Grid1.DataSource = roles;
            Grid1.DataBind();
        }


        private void BindGrid2()
        {
            int roleId = GridHelper.GetSelectedId(Grid1);
            if (roleId == -1)
            {
                Grid2.DataSource = null;
                Grid2.DataBind();
            }
            else
            {
                // 当前角色拥有的权限
                RoleType role = (RoleType)(roleId);
                _powers.Clear();
                foreach (var item in RolePower.Set.Where(t => t.Role == role))
                {
                    if (!_powers.ContainsKey(item.Power))
                        _powers.Add(item.Power, true);
                }

                // 权限分组展示
                Grid2.DataSource = typeof(PowerType).GetEnumGroups().Select(t => new { Group = t }).ToList();
                Grid2.DataBind();
            }
        }


        //---------------------------------------------------
        // Grid1（角色表格）
        //---------------------------------------------------
        protected void Grid1_RowClick(object sender, FineUI.GridRowClickEventArgs e)
        {
            BindGrid2();
        }

        protected void btnEditRole_Click(object sender, EventArgs e)
        {
            this.Window1.Title = "编辑角色列表";
            this.Window1.IFrameUrl = "Roles.aspx";
            this.Window1.Hidden = false;
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            LoadData();
        }


        //---------------------------------------------------
        // Grid2（权限表格）
        //---------------------------------------------------
        // 编辑权限列表
        protected void btnEditPower_Click(object sender, EventArgs e)
        {
            this.Window1.Title = "编辑权限列表";
            this.Window1.IFrameUrl = "Powers.aspx";
            this.Window1.Hidden = false;
        }

        // 用CheckBoxList展现权限列表
        protected void Grid2_RowDataBound(object sender, FineUI.GridRowEventArgs e)
        {
            AspNet.CheckBoxList cbl = (AspNet.CheckBoxList)Grid2.Rows[e.RowIndex].FindControl("ddlPowers");
            string group = e.DataItem.GetPropertyValue("Group").ToString();
            var items = typeof(PowerType).ToList(group);
            foreach (var power in items)
            {
                AspNet.ListItem item = new AspNet.ListItem();
                item.Value = power.ID.ToString();
                item.Text = power.Name;
                item.Attributes["data-qtip"] = power.Name;
                item.Selected = _powers.ContainsKey((PowerType)(power.Value));
                cbl.Items.Add(item);
            }
        }

        // 保存权限
        protected void btnGroupUpdate_Click(object sender, EventArgs e)
        {
            // 角色
            int roleId = GridHelper.GetSelectedId(Grid1);
            if (roleId == -1)
                return;

            // 新的权限列表
            var role = (RoleType)roleId;
            var powers = new List<PowerType>();
            for (int i = 0; i < Grid2.Rows.Count; i++)
            {
                AspNet.CheckBoxList ddlPowers = (AspNet.CheckBoxList)Grid2.Rows[i].FindControl("ddlPowers");
                foreach (AspNet.ListItem item in ddlPowers.Items)
                    if (item.Selected)
                        powers.Add((PowerType)(Convert.ToInt32(item.Value)));
            }

            // 更新权限信息
            DAL.User.SetRolePowers(role, powers);
            this.lblInfo.Text = string.Format("已保存 {0:HH:mm:ss}", DateTime.Now);
        }


    }
}
