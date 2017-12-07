using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUI;
using App.DAL;
using App.Components;

namespace App.Admin
{
    /// <summary>
    /// 职务用户管理页面
    /// </summary>
    [ViewPower("CoreTitleUserView")]
    public partial class TitleUsers : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Common.SetButtonByPower(btnEditTitle, "CoreTitleEdit");
            Common.SetButtonByPower(btnNewUser, "CoreTitleUserNew");
            this.Grid2.ViewUrlTmpl = "~/admin/UserView.aspx?id={0}";
            this.Grid2.AllowDelete = Common.CheckPower("CoreTitleUserDelete", false);
            this.Grid2.AllowBatchDelete = Common.CheckPower("CoreTitleUserDelete", false);
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
            var q = Common.Db.Titles.SortBy("Name", "DESC");
            Grid1.DataSource = q;
            Grid1.DataBind();
        }

        private void BindGrid2()
        {
            int titleID = GridHelper.GetSelectedRowKeyID(Grid1);
            if (titleID == -1)
            {
                Grid2.RecordCount = 0;
                Grid2.DataSource = null;
                Grid2.DataBind();
            }
            else
            {
                IQueryable<User> q = Common.Db.Users;
                q = q.Where(u => u.Name != "admin");
                q = q.Where(u => u.Titles.Any(r => r.ID == titleID));
                string searchText = ttbSearchUser.Text.Trim();
                if (!String.IsNullOrEmpty(searchText))
                    q = q.Where(u => u.Name.Contains(searchText));
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


        protected void btnEditTitle_Click(object sender, EventArgs e)
        {
            this.Window1.Title = "编辑职称";
            this.Window1.IFrameUrl = "Titles.aspx";
            this.Window1.Hidden = false;
        }

        //
        protected void Window1_Close(object sender, EventArgs e)
        {
            LoadData();
        }


        //--------------------------------------------------
        // Grid2
        //--------------------------------------------------
        protected void ttbSearchUser_TriggerClick(object sender, string e)
        {
            BindGrid2();
        }

        // 删除用户职务
        protected void Grid2_Delete(object sender, List<int> ids)
        {
            int titleID = GridHelper.GetSelectedRowKeyID(Grid1);
            List<int> userIDs = GridHelper.GetSelectedRowKeyIDs(Grid2);
            DbUser.DeleteUsersTitle(titleID, userIDs);
        }

        // 新增用户
        protected void btnNewUser_Click(object sender, EventArgs e)
        {
            int titleID = GridHelper.GetSelectedRowKeyID(Grid1);
            string addUrl = String.Format("~/admin/TitleUserNew.aspx?id={0}", titleID);
            PageContext.RegisterStartupScript(Window1.GetShowReference(addUrl, "添加用户到当前职称"));
        }

    }
}
