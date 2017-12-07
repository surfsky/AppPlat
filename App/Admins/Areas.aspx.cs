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
    /// 区域管理主页面
    /// </summary>
    [Auth(PowerType.AreaView)]
    public partial class Areas : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Grid1.NewUrlTmpl = "~/admins/AreaForm.aspx?mode=new&parentid={0}";
            Grid1.EditUrlTmpl = "~/admins/AreaForm.aspx?mode=edit&id={0}";
            Grid1.AllowNew = Common.CheckPower(PowerType.AreaEdit);
            Grid1.AllowEdit = Common.CheckPower(PowerType.AreaEdit);
            Grid1.AllowDelete = Common.CheckPower(PowerType.AreaDelete);
            Grid1.InitGrid<Area>(BindGrid);
            if (!IsPostBack)
            {
                Grid1.SetSortAndPage<Area>(false, false);
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
            Area.Reload();
            Grid1.DataSource = Area.All;
            Grid1.DataBind();
        }

        protected void Grid1_Delete(object sender, List<int> ids)
        {
            Area.DeleteRecursive(ids[0]);
            Area.Reload();
        }
    }
}
