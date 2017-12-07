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
    /// 职务列表管理页面
    /// </summary>
    [Auth(PowerType.TitleView)]
    public partial class Titles : PageBase
    {
        // Init
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Grid1.NewUrlTmpl = "~/admins/TitleForm.aspx?mode=new";
            this.Grid1.EditUrlTmpl = "~/admins/TitleForm.aspx?mode=edit&id={0}";
            this.Grid1.AllowNew = Common.CheckPower(PowerType.TitleEdit);
            this.Grid1.AllowDelete = Common.CheckPower(PowerType.TitleDelete);
            this.Grid1.AllowBatchDelete = Common.CheckPower(PowerType.TitleDelete);
            this.Grid1.AllowEdit = Common.CheckPower(PowerType.TitleEdit);
            this.Grid1.InitGrid<Title>(BindGrid);
            if (!IsPostBack)
            {
                this.Grid1.SetSortAndPage<Title>(true, true, SiteConfig.PageSize, t=>t.Name);
                BindGrid();
            }
        }

        // 绑定网格
        private void BindGrid()
        {
            string name = ttbSearchMessage.Text.Trim();
            IQueryable<Title> q = DAL.Title.Search(name);
            Grid1.BindGrid(q);
        }


        // 关闭本窗口
        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        // 查找
        protected void ttbSearchMessage_TriggerClick(object sender, string e)
        {
            BindGrid();
        }

        // 删除事件
        protected void Grid1_Delete(object sender, List<int> ids)
        {
            DAL.Title.DeleteBatch(ids);
        }


    }
}
