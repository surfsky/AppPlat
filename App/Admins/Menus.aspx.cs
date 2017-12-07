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
    /// 菜单管理主界面
    /// </summary>
    [Auth(PowerType.MenuEdit)]
    public partial class Menus : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Grid1.NewUrlTmpl  = "~/admins/MenuForm.aspx?mode=new&parentid={0}";
            Grid1.EditUrlTmpl = "~/admins/MenuForm.aspx?mode=edit&id={0}";
            Grid1.AllowNew = Common.CheckPower(PowerType.MenuEdit);
            Grid1.AllowEdit = Common.CheckPower(PowerType.MenuEdit);
            Grid1.AllowDelete = Common.CheckPower(PowerType.MenuEdit);
            Grid1.InitGrid<DAL.Menu>(BindGrid);
            if (!IsPostBack)
            {
                Grid1.SetSortAndPage<DAL.Menu>(false, false);
                BindGrid();
            }
        }

        private void BindGrid()
        {
            Grid1.DataSource = DAL.Menu.All;
            Grid1.DataBind();
        }

        protected void Grid1_Delete(object sender, List<int> e)
        {
            DAL.Menu.DeleteRecursive(e[0]);
            DAL.Menu.Reload();
        }
    }
}
