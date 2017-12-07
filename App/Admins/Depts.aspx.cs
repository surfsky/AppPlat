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

namespace App.Admins
{
    /// <summary>
    /// 部门管理主页面
    /// </summary>
    [Auth(PowerType.DeptView)]
    public partial class Depts : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Grid1.NewUrlTmpl = "~/admins/DeptForm.aspx?mode=new&parentid={0}";
            Grid1.EditUrlTmpl = "~/admins/DeptForm.aspx?mode=edit&id={0}";
            Grid1.AllowNew = Common.CheckPower(PowerType.DeptEdit);
            Grid1.AllowEdit = Common.CheckPower(PowerType.DeptEdit);
            Grid1.AllowDelete = Common.CheckPower(PowerType.DeptDelete);
            Grid1.InitGrid<Dept>(BindGrid);
            if (!IsPostBack)
            {
                Grid1.SetSortAndPage<Dept>(false, false);
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
        private void BindGrid()
        {
            Dept.Reload();
            Grid1.DataSource = Dept.All;
            Grid1.DataBind();
        }

        protected void Grid1_Delete(object sender, List<int> ids)
        {
            Dept.DeleteRecursive(ids[0]);
        }
    }
}
