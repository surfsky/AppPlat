using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUI;
using App.Controls;
using App.DAL;
using App.Components;

namespace App.Admins
{
    /// <summary>
    /// 文章详情
    /// </summary>
    public partial class Article : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id = Asp.GetQueryIntValue("id").Value;
                var item = DAL.Article.Get(id);
                item.VisitCnt += 1;
                item.Save();
                this.lblTitle.Text = item.Title;
                this.lblAuthor.Text = item.Author;
                this.lblPostDt.Text = item.PostDt.ToString("yyyy-MM-dd");
                this.lblVisitCnt.Text = item.VisitCnt.ToText();
                this.lblContent.Text = item.Body;
                this.rptImage.DataSource = item.Images;
                this.rptImage.DataBind();
            }
        }
    }
}
