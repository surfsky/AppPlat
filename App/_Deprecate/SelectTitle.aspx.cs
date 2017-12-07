using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUI;
using System.Transactions;
using System.Text;

namespace App.Admin
{
    /// <summary>
    /// 职位选择窗口
    /// 输入参数：ids
    /// </summary>
    [ViewPower("CoreTitleView")]
    public partial class SelectTitle : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnClose.OnClientClick = ActiveWindow.GetHideReference();
                string ids = Request.QueryString["ids"];
                ShowTitles();
                cblJobTitle.SelectedValueArray = ids.Split(',');
            }
        }

        // 显示职务列表
        private void ShowTitles()
        {
            cblJobTitle.DataTextField = "Name";
            cblJobTitle.DataValueField = "ID";
            cblJobTitle.DataSource = Common.Db.Titles;
            cblJobTitle.DataBind();
        }

        // 保存并将选择数据传递给父窗口
        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            string titleValues = String.Join(",", cblJobTitle.SelectedItemArray.Select(c => c.Value));
            string titleTexts = String.Join(",", cblJobTitle.SelectedItemArray.Select(c => c.Text));
            PageContext.RegisterStartupScript(
                  ActiveWindow.GetWriteBackValueReference(titleValues, titleTexts)
                + ActiveWindow.GetHideReference()
                );
        }
    }
}
