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

namespace App.Admins
{
    /// <summary>
    /// 职务编辑窗口
    /// </summary>
    public partial class TitleForm : FormPage<Title>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitForm(this.SimpleForm1, PowerType.TitleView, PowerType.TitleEdit, PowerType.TitleEdit);
            if (!IsPostBack)
                ShowForm();
        }

        public override void NewData()
        {
            tbxName.Text = "";
            tbxRemark.Text = "";
        }

        public override void ShowData(Title item)
        {
            tbxName.Text = item.Name;
            tbxRemark.Text = item.Remark;
        }

        public override void CollectData(ref Title item)
        {
            item.Name = tbxName.Text.Trim();
            item.Remark = tbxRemark.Text.Trim();
        }
    }
}
