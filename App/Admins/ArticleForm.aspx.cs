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
    /// 文章编辑窗口
    /// </summary>
    public partial class ArticleForm : FormPage<DAL.Article>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitForm(this.SimpleForm1, PowerType.ArticleView, PowerType.ArticleEdit, PowerType.ArticleEdit);
            if (!IsPostBack)
                ShowForm();
        }

        // 
        private void InitControls(DAL.Article item)
        {
            int? type = (item == null || item.Type == null) ? null : (int?)item.Type;
            UI.BindDDLEnum(ddlType, typeof(ArticleType), "--类型--", selectedId: null);
            ddlType.SelectedValue = type.ToText();
        }

        public override void NewData()
        {
            tbTitle.Text = "";
            tbAuthor.Text = "";
            tbContent.Text = "";
            lblPostDt.Text = "";
            tbVisitCnt.Text = "0";
            InitControls(null);

            this.Region2.Hidden = true;
            this.Region2.Width = 0;
            this.Region1.Width = 1000;
        }

        public override void ShowData(DAL.Article item)
        {
            tbTitle.Text = item.Title;
            tbAuthor.Text = item.Author;
            tbContent.Text = item.Body;
            lblPostDt.Text = item.PostDt.ToText();
            tbVisitCnt.Text = item.VisitCnt.ToText();
            InitControls(item);

            this.Region2.Hidden = false;
            this.Panel2.IFrameUrl = string.Format("~/Admins/Resources.aspx?cate=Article&key={0}", item.ResKey);
        }

        public override void CollectData(ref DAL.Article item)
        {
            item.Title = tbTitle.Text.Trim();
            item.Author = tbAuthor.Text.Trim();
            item.Body = tbContent.Text.Trim();
            item.PostDt = DateTime.Now;
            item.VisitCnt = int.Parse(tbVisitCnt.Text);
            item.Type = ddlType.SelectedItemArray.Length != 0 ? (ArticleType?)(int.Parse(ddlType.SelectedValue)) : null;
            item.SetSummary();
        }
    }
}
