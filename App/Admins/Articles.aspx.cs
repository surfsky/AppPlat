using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUI;
using EntityFramework.Extensions;
using App;
using App.DAL;
using App.Components;

namespace App.Admins
{
    /// <summary>
    /// 文章列表。
    /// </summary>
    [Auth(PowerType.ArticleView)]
    public partial class Articles : PageBase
    {
        // Init
        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnNew.Hidden = !Common.CheckPower(PowerType.ArticleEdit);
            this.Grid1.NewUrlTmpl = "~/admins/ArticleForm.aspx?mode=new";
            this.Grid1.EditUrlTmpl = "~/admins/ArticleForm.aspx?mode=edit&id={0}";
            this.Grid1.ViewUrlTmpl = "~/admins/Article.aspx?id={0}";
            this.Grid1.AllowNew = false;
            this.Grid1.AllowDelete = Common.CheckPower(PowerType.ArticleDelete);
            this.Grid1.AllowBatchDelete = Common.CheckPower(PowerType.ArticleDelete);
            this.Grid1.AllowEdit = Common.CheckPower(PowerType.ArticleEdit);
            this.Grid1.InitGrid<DAL.Article>(BindGrid);
            if (!IsPostBack)
            {
                UI.BindDDLEnum(ddlType, typeof(ArticleType), "--全部类别--", selectedId: null);
                this.Grid1.SetSortAndPage<DAL.Article>(true, true, SiteConfig.PageSize, t=>t.PostDt, false);
                BindGrid();
            }
        }


        // 绑定网格
        private void BindGrid()
        {
            string author = tbAuthor.Text.Trim();
            string title = tbTitle.Text.Trim();
            var startDt = dpStart.SelectedDate;
            var endDt = dpEnd.SelectedDate;
            ArticleType? type = (ArticleType?)UI.GetDDLValue(ddlType);

            IQueryable<DAL.Article> q = DAL.Article.Search(type, author, title, startDt, endDt);
            Grid1.BindGrid(q);
        }


        //--------------------------------------------------
        // 工具栏
        //--------------------------------------------------
        // 查找
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        // 新增（事实上是新增后编辑）
        protected void btnNew_Click(object sender, EventArgs e)
        {
            var news = DAL.Article.Add();
            string url = string.Format(Grid1.EditUrlTmpl, news.ID);
            PageContext.RegisterStartupScript(this.Grid1.Win.GetShowReference(url));
            PageContext.RegisterStartupScript(this.Grid1.Win.GetMaximizeReference());
        }

        //--------------------------------------------------
        // Grid
        //--------------------------------------------------
        // 行绑定事件（BUG: 若是admin则删除按钮无效，代码都对，但整列都变无效了）
        protected void Grid1_PreRowDataBound(object sender, FineUI.GridPreRowEventArgs e)
        {
            var item = e.DataItem as DAL.Article;

            // 设置职务列
            var field = Grid1.FindColumn("Type") as FineUI.BoundField;
            field.DataFormatString = item.Type.GetDescription();
        }

        // 删除
        protected void Grid1_Delete(object sender, List<int> ids)
        {
            DAL.Article.DeleteBatch(ids, typeof(Article));
        }


    }
}
