using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUI;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using App.DAL;
using App.Components;

namespace App.Admin
{
    /// <summary>
    /// 新增部门用户
    /// 部门id由querystring传入
    /// </summary>
    [ViewPower("CoreDeptUserNew")]
    public partial class DeptUserNew : PageBase
    {
        //---------------------------------------------------
        // Init
        //---------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Grid1.PageSize = DbConfig.PageSize;
            this.Grid1.InitGrid<User>((Action)BindGrid, null, "Name", true, true);
            if (!IsPostBack)
            {
                btnClose.OnClientClick = ActiveWindow.GetHideReference();
                int id = Common.GetQueryIntValue("id");
                Dept current = Common.Db.Depts.Find(id);
                if (current == null)
                {
                    Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                    return;
                }
                BindGrid();
            }
        }

        // 查询
        void BindGrid()
        {
            IQueryable<User> q = Common.Db.Users.Include(u => u.Dept);
            string searchText = ttbSearchMessage.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
                q = q.Where(u => u.Name.Contains(searchText) || u.RealName.Contains(searchText));
            q = q.Where(u => u.Name != "admin");  // 不能是admin
            q = q.Where(u => u.Dept == null);     // 用户必须没有部门
            Grid1.SortAndPage(q);
        }

        // 搜索
        protected void ttbSearchMessage_TriggerClick(object sender, string e)
        {
            BindGrid();
        }

        // 保存并关闭
        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            List<int> ids = GridHelper.GetSelectedRowKeyIDs(Grid1);
            int deptID = Common.GetQueryIntValue("id");
            Dept dept = EFHelper.GetAttach<Dept>(deptID);
            Common.Db.Users.Where(u => ids.Contains(u.ID))
                .ToList()
                .ForEach(u => u.Dept = dept);
            Common.Db.SaveChanges();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

    }
}
