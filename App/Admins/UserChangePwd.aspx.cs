using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity;
using System.Linq;
using FineUI;
using App.DAL;
using App.Components;

namespace App.Admins
{
    /// <summary>
    /// 管理员修改用户密码
    /// </summary>
    [Auth(PowerType.UserChangePassword)]
    public partial class UserChangePwd : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnClose.OnClientClick = ActiveWindow.GetHideReference();
                LoadData();
            }
        }

        // 读取用户信息
        private void LoadData()
        {
            int id = Asp.GetQueryIntValue("id").Value;
            User user = DAL.User.Get(id);
            if (user == null)
            {
                Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }
            if (user.Name == "admin" && AuthHelper.GetIdentityName() != "admin")
            {
                Alert.Show("你无权编辑超级管理员！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }
            labUserName.Text = user.Name;
            labUserRealName.Text = user.RealName;
        }

        // 保存并关闭
        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            int id = Asp.GetQueryIntValue("id").Value;
            User item = DAL.User.Get(id);
            DAL.User.SetPassword(item, tbxPassword.Text.Trim());
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
    }
}
