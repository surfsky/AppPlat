using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUI;
using EntityFramework.Extensions;
using App.DAL;
using App.Components;

namespace App.Admin
{
    /// <summary>
    /// 角色用户管理界面
    /// </summary>
    [ViewPower("CoreRoleUserView")]
    public partial class RoleUsers : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Common.SetButtonByPower(btnEditRole, "CoreRoleEdit");
            Common.SetButtonByPower(btnNewUser, "CoreRoleUserNew");
            this.Grid2.ViewUrlTmpl = "~/admin/UserView.aspx?id={0}";
            this.Grid2.AllowDelete = Common.CheckPower("CoreRoleUserDelete", false);
            this.Grid2.AllowBatchDelete = Common.CheckPower("CoreRoleUserDelete", false);
            this.Grid2.PageSize = DbConfig.PageSize;
            this.Grid2.ShowViewField = true;
            this.Grid2.ShowDeleteField = true;
            this.Grid2.InitGrid<User>((Action)BindGrid2, null, "Name");

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            BindGrid1();
            Grid1.SelectedRowIndex = 0;
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
            int roleID = GridHelper.GetSelectedRowKeyID(Grid1);
            if (roleID == -1)
            {
                Grid2.RecordCount = 0;
                Grid2.DataSource = null;
                Grid2.DataBind();
            }
            else
            {
                // 查询用于该角色的用户（并排除admin用户）
                IQueryable<User> q = Common.Db.Users.Include(u => u.Roles)
                    .Where(u => u.Name != "admin")
                    .Where(u => u.Roles.Any(r => r.ID == roleID))
                    ;

                // 搜索
                string searchText = ttbSearchUser.Text.Trim();
                if (!String.IsNullOrEmpty(searchText))
                    q = q.Where(u => u.Name.Contains(searchText));

                // 排序分页
                Grid2.SortAndPage<User>(q);
            }
        }


        //--------------------------------------------------
        // Grid1
        //--------------------------------------------------
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


        //--------------------------------------------------
        // Grid2
        //--------------------------------------------------
        // 查找
        protected void ttbSearchUser_TriggerClick(object sender, string e)
        {
            BindGrid2();
        }

        // 将用户从角色中剔除
        protected void Grid2_Delete(object sender, List<int> ids)
        {
            int roleID = GridHelper.GetSelectedRowKeyID(Grid1);
            DbUser.DeleteUsersRole(ids, roleID);
        }



        // 新增用户
        protected void btnNewUser_Click(object sender, EventArgs e)
        {
            int roleID = GridHelper.GetSelectedRowKeyID(Grid1);
            string addUrl = String.Format("~/admin/RoleUserNew.aspx?id={0}", roleID);
            PageContext.RegisterStartupScript(Window1.GetShowReference(addUrl, "添加用户到当前角色"));
        }



    }
}
