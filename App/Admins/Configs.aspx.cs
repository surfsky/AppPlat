using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;
using FineUI;
using App.DAL;
using App.Components;

namespace App.Admins
{
    /// <summary>
    /// 系统参数配置页面。
    /// 网站相关的配置信息放在这里管理。该界面基本无需变更。
    /// 三方平台相关的配置信息直接挪到webconfig中，并在Common中创建对应的变量。
    /// </summary>
    [Auth(PowerType.ConfigEdit)]
    public partial class Configs : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UI.SetButtonByPower(btnSave, PowerType.ConfigEdit);
                tbxTitle.Text = SiteConfig.SiteTitle;
                nbxPageSize.Text = SiteConfig.PageSize.ToString();
                tbxHelpList.Text = Common.FormatScript(SiteConfig.HelpList);
                ddlMenuType.SelectedValue = SiteConfig.MenuType;
                ddlTheme.SelectedValue = SiteConfig.Theme;
                tbDefaultPassword.Text = SiteConfig.DefaultPassword;
            }
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            // 帮助字符串
            string helpListStr = tbxHelpList.Text.Trim();
            try
            {
                JArray.Parse(helpListStr);
            }
            catch (Exception)
            {
                tbxHelpList.MarkInvalid("格式不正确，必须是JSON字符串！");
                return;
            }

            // 保存
            SiteConfig.SiteTitle = tbxTitle.Text.Trim();
            SiteConfig.PageSize = Convert.ToInt32(nbxPageSize.Text.Trim());
            SiteConfig.HelpList = helpListStr;
            SiteConfig.MenuType = ddlMenuType.SelectedValue;
            SiteConfig.Theme = ddlTheme.SelectedValue;
            SiteConfig.DefaultPassword = tbDefaultPassword.Text.Trim();
            SiteConfig.Save();
            Alert.ShowInTop("修改系统配置成功（点击确定刷新页面）！", String.Empty, "top.window.location.reload(false);");
        }
    }
}
