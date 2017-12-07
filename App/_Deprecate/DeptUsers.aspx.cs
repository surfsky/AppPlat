using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUI;
using App.DAL;

namespace App.Admin
{
    [ViewPower("CoreDeptUserView")]
    public partial class DeptUsers : PageBase
    {

        //------------------------------------------
        // Init
        //------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            Common.SetButtonByPower(btnEditDept, "CoreDeptEdit");
            Common.SetButtonByPower(btnNewUser, "CoreDeptUserNew");
            this.Grid2.ViewUrlTmpl = "~/admin/UserView.aspx?id={0}";
            this.Grid2.AllowDelete = Common.CheckPower("CoreDeptUserDelete", false);
            this.Grid2.AllowBatchDelete = Common.CheckPower("CoreDeptUserDelete", false);
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
            Grid1.DataSource = DbDept.Depts;
            Grid1.DataBind();
        }

        private void BindGrid2()
        {
            int deptID = GridHelper.GetSelectedRowKeyID(Grid1);
            if (deptID == -1)
            {
                Grid2.RecordCount = 0;
                Grid2.DataSource = null;
                Grid2.DataBind();
            }
            else
            {
                // 查询部门用户（并排除admin用户）
                IQueryable<User> q = Common.Db.Users.Include(u => u.Dept)
                    .Where(u => u.Name != "admin")
                    .Where(u => u.Dept.ID == deptID)
                    ;

                // 搜索名称
                string searchText = ttbSearchUser.Text.Trim();
                if (!String.IsNullOrEmpty(searchText))
                    q = q.Where(u => u.Name.Contains(searchText));
                
                // 排列和分页
                Grid2.SortAndPage<User>(q);
            }
        }


        //------------------------------------------
        // Grid1
        //------------------------------------------
        protected void Grid1_RowClick(object sender, FineUI.GridRowClickEventArgs e)
        {
            BindGrid2();
        }

        protected void btnEditDept_Click(object sender, EventArgs e)
        {
            this.Window1.Title = "编辑部门";
            this.Window1.IFrameUrl = "depts.aspx";
            this.Window1.Hidden = false;
        }

        // 窗口1关闭
        protected void Window1_Close(object sender, EventArgs e)
        {
            LoadData();
        }


        //------------------------------------------
        // Grid2
        //------------------------------------------
        // 查找
        protected void ttbSearchUser_TriggerClick(object sender, string e)
        {
            BindGrid2();
        }

        // 将用户从部门中移除
        protected void Grid2_Delete(object sender, List<int> ids)
        {
            DbUser.DeleteUsersDept(ids);
        }

        // 新增部门用户
        protected void btnNewUser_Click(object sender, EventArgs e)
        {
            string addUrl = String.Format("~/admin/DeptUserNew.aspx?id={0}", GridHelper.GetSelectedRowKeyID(Grid1));
            PageContext.RegisterStartupScript(Window1.GetShowReference(addUrl, "添加用户到当前部门"));
        }


    }
}
