using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using FineUI;
using System.Linq;
using App.DAL;
using Kingsoc.Web.WebCall;

namespace App.Admins
{
    /// <summary>
    /// 用户修改自己的信息（密码）
    /// </summary>
    public partial class Profiles : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        // 保存密码
        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            // 检查密码一致性
            string oldPass = tbxOldPassword.Text.Trim();
            string newPass = tbxNewPassword.Text.Trim();
            string confirmNewPass = tbxConfirmNewPassword.Text.Trim();
            if (newPass != confirmNewPass)
            {
                tbxConfirmNewPassword.MarkInvalid("确认密码和新密码不一致！");
                return;
            }

            User user = Common.LoginUser;
            var result = DbUser.EditUserPassword(user.ID, oldPass, newPass);
            if (result.Info == "旧密码不正确")
                tbxOldPassword.MarkInvalid("当前密码不正确！");
            else
                Alert.ShowInTop(result.Info);
        }


    }
}
