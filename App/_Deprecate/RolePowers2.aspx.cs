using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
//using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUI;
using System.Data;
using Newtonsoft.Json.Linq;
//using AspNet = System.Web.UI.WebControls;
using App.DAL;
using App.Components;

namespace App.Admins
{
    /// <summary>
    /// 角色权限管理
    /// </summary>
    /// <remarks>
    /// 经试验，f:CheckBoxList嵌套在grid会无法点击、无法动态刷新
    /// 总之种种问题，难怪用asp:CheckBoxList替代
    /// 本文件保留，待以后更好的解决方案
    /// </remarks>
    [ViewPower("CoreRolePowerView")]
    public partial class RolePowers2 : PageBase
    {
        private Dictionary<string, bool> _currentRolePowers = new Dictionary<string, bool>();


        //---------------------------------------------------
        // Init
        //---------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            Common.SetButtonByPower(this.btnEditRole, "CoreRoleEdit");
            Common.SetButtonByPower(this.btnEditPower, "CorePowerEdit");
            if (!IsPostBack)
                LoadData();
        }

        private void LoadData()
        {
            Grid1.PageSize = DbConfig.PageSize;
            BindGrid1();
            Grid1.SelectedRowIndex = 0;
            Grid2.PageSize = DbConfig.PageSize;
            BindGrid2();
        }

        private void BindGrid1()
        {
            var q = Common.Db.Roles.SortBy(Grid1.SortField, Grid1.SortDirection);
            Grid1.DataSource = q;
            Grid1.DataBind();
        }


        private void BindGrid2()
        {
            int roleId = GridHelper.GetSelectedRowKeyID(Grid1);
            if (roleId == -1)
            {
                Grid2.DataSource = null;
                Grid2.DataBind();
            }
            else
            {
                // 当前选中角色拥有的权限列表
                _currentRolePowers.Clear();
                Role role = Common.Db.Roles.Include(t => t.Powers).Where(t => t.ID == roleId).FirstOrDefault();

                // 将用户拥有的权限保存到权限字典中去
                foreach (var power in role.Powers)
                {
                    string powerName = power.Name;
                    if (!_currentRolePowers.ContainsKey(powerName))
                        _currentRolePowers.Add(powerName, true);
                }

                // 所有权限（根据类别分组）
                var q = Common.Db.Powers
                    .GroupBy(p => p.GroupName).OrderBy(g => g.Key)
                    .Select(g => new { GroupName = g.Key, Powers = g });
                Grid2.DataSource =  q;
                Grid2.DataBind();
            }
        }


        //---------------------------------------------------
        // Grid1
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
        // Grid2
        //---------------------------------------------------
        protected void btnEditPower_Click(object sender, EventArgs e)
        {
            this.Window1.Title = "编辑权限列表";
            this.Window1.IFrameUrl = "Powers.aspx";
            this.Window1.Hidden = false;
        }

        // 用CheckBoxList展现权限列表
        protected void Grid2_RowDataBound(object sender, FineUI.GridRowEventArgs e)
        {
            var cbl = (FineUI.CheckBoxList)Grid2.Rows[e.RowIndex].FindControl("ddlPowers");
            cbl.Items.Clear();
            IGrouping<string, Power> powers = e.DataItem.GetType().GetProperty("Powers").GetValue(e.DataItem) as IGrouping<string, Power>;
            foreach (Power power in powers.ToList())
            {
                var item = new CheckItem()
                {
                    Value = power.ID.ToString(),
                    Text = power.Title,
                    Selected = _currentRolePowers.ContainsKey(power.Name)
                    //item.Attributes["data-qtip"] = power.Name;
                };
                cbl.Items.Add(item);
            }
        }

        protected void btnGroupUpdate_Click(object sender, EventArgs e)
        {
            // 角色
            int roleId = GridHelper.GetSelectedRowKeyID(Grid1);
            if (roleId == -1)
                return;

            // 新的权限列表
            List<int> newPowerIDs = new List<int>();
            List<string> newPowerNames = new List<string>();
            for (int i = 0; i < Grid2.Rows.Count; i++)
            {
                var ddlPowers = (CheckBoxList)Grid2.Rows[i].FindControl("ddlPowers");
                foreach (var item in ddlPowers.Items)
                {
                    if (item.Selected)
                    {
                        newPowerIDs.Add(Convert.ToInt32(item.Value));
                        newPowerNames.Add(item.Text);
                    }
                }
            }

            // 更新权限信息
            Role role = Common.Db.Roles.Include(r => r.Powers).Where(r => r.ID == roleId).FirstOrDefault();
            role.Powers.ReplaceAttach(newPowerIDs.ToArray());
            Common.Db.SaveChanges();

            // TODO: 用非阻碍式提示替代
            Alert.ShowInTop("选中角色的权限更新成功！"); 
        }

        // 全选全不选
        protected void btnSelectAll_Click(object sender, EventArgs e)
        {
            SetAllPowerCheck(true);
        }
        protected void btnUnSelectAll_Click(object sender, EventArgs e)
        {
            SetAllPowerCheck(false);
        }
        private void SetAllPowerCheck(bool selected)
        {
            for (int i = 0; i < Grid2.Rows.Count; i++)
            {
                var ddlPowers = (CheckBoxList)Grid2.Rows[i].FindControl("ddlPowers");
                foreach (var item in ddlPowers.Items)
                    item.Selected = selected;
            }
        }

    }
}
