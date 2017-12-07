using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using FineUI;
using App.DAL;


namespace App.Admins
{
    [Auth(PowerType.OnlineEdit)]
    public partial class Onlines : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Grid1.InitGrid<Online>(BindGrid);
            if (!IsPostBack)
            {
                this.Grid1.SetSortAndPage<Online>(true, true, SiteConfig.PageSize, t=>t.UpdateDt);
                BindGrid();
            }
        }

        // 搜索用户名 & 最近2小时内登录的用户
        private void BindGrid()
        {
            string name = ttbSearchMessage.Text.Trim();
            IQueryable<Online> q = Online.Search(name);
            Grid1.BindGrid(q);
        }



        // 搜索
        protected void ttbSearchMessage_TriggerClick(object sender, string e)
        {
            BindGrid();
        }
    }
}
