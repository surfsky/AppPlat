using App.Components;
using App.Controls;
using App.DAL;
using FineUI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.Admins
{
    /// <summary>
    /// 简单的数据库表数据浏览页面（未完成）
    /// </summary>
    public partial class DbGrid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Grid1.NewUrlTmpl = "~/admins/DbForm.aspx?mode=new&table={0}";
            Grid1.EditUrlTmpl = "~/admins/DbForm.aspx?mode=edit&table={0}&id={1}";  //....
            Grid1.AllowNew = Common.CheckPower(PowerType.Admin);
            Grid1.AllowEdit = Common.CheckPower(PowerType.Admin);
            Grid1.AllowDelete = Common.CheckPower(PowerType.Admin);
            if (!IsPostBack)
            {
                BindGrid();
            }


        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        //------------------------------------------
        // grid
        //------------------------------------------
        // 绑定数据
        private void BindGrid()
        {
            string table = Request.QueryString["table"];
            if (string.IsNullOrEmpty(table)) return;
            string sql = string.Format("select * from {0}", table);
            var dt = AppContext.Current.ExecuteSelectSql(sql);
            GridPro.BindGrid(Grid1, dt);
        }

        // 删除
        protected void Grid1_Delete(object sender, List<int> ids)
        {
        }

        // 关闭
        protected void Grid1_WindowClose(object sender, EventArgs e)
        {
        }
    }
}