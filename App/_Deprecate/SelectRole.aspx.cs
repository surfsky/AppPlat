using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUI;
using App.DAL;

namespace App.Admin
{
    /// <summary>
    /// 角色选择窗口
    /// 输入参数：ids
    /// </summary>
    [ViewPower("CoreRoleView")]
    public partial class SelectRole : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnClose.OnClientClick = ActiveWindow.GetHideReference();
                string ids = Request.QueryString["ids"];
                ShowRoles();
                cblRole.SelectedValueArray = ids.Split(',');
            }
        }

        // 显示角色列表(只显示当前用户拥有的角色列表)
        private void ShowRoles()
        {
            cblRole.DataTextField = "Name";
            cblRole.DataValueField = "ID";
            cblRole.DataSource = Common.LoginUser.Roles;
            cblRole.DataBind();
        }

        // 保存并将选择数据传递给父窗口
        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            string roleValues = String.Join(",", cblRole.SelectedItemArray.Select(c => c.Value));
            string roleTexts = String.Join(",", cblRole.SelectedItemArray.Select(c => c.Text));
            PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(roleValues, roleTexts)
                + ActiveWindow.GetHideReference());
        }
    }
}
