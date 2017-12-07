using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Newtonsoft.Json.Linq;
using FineUI;
using System.Linq;
using System.Data.Entity;
using App.DAL;
using App.Components;
using System.Reflection;

namespace App
{
    /// <summary>
    /// 管理后台主窗口
    /// </summary>
    [Auth(PowerType.Backend, CheckLogin=true)]
    public partial class Main : PageBase
    {
        //--------------------------------------------------
        // Init
        //--------------------------------------------------
        // 加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitHelp();
                this.treeMenu.Nodes.Clear();
                if (Common.LoginUser != null)
                    BuildTree(Common.LoginUser.Menus, null, treeMenu.Nodes);

                this.txtTitle.Text = SiteConfig.SiteTitle;
                this.Title = SiteConfig.SiteTitle;
                this.lblVersion.Text = ReflectionHelper.AssemblyVersion.ToString();
                this.txtUser.Text = string.Format("<span class='label'>欢迎 </span><span>{0}</span>", AuthHelper.GetIdentityName());
                this.txtOnlineUserCount.Text = string.Format("在线人数: {0}", Online.GetOnlineCount());
            }
        }

        // 注销
        protected void btnExit_Click(object sender, EventArgs e)
        {
            DbUser.Logout(true);
        }


        // 用定时器保持客户端连接，且定时获取一些业务变更消息，如新订单、在线用户数等。
        // fineui有内置机制，如果属性变更了，只发变更的这部分属性到客户端。
        // 本方案成本有点大，每次都要重建页面，回发viewstate。可作为低压力方案
        // 建议的方案是单独写接口，不要在页面中实现。
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            this.txtOnlineUserCount.Text = string.Format("在线人数: {0}", Online.GetOnlineCount());
            this.Timer1.Enabled = false;
        }


        //--------------------------------------------------
        // 初始化页面
        //--------------------------------------------------
        // 工具栏上的帮助菜单
        private void InitHelp()
        {
            if (SiteConfig.HelpList == null || SiteConfig.HelpList.Trim() == "")
                return;
            JArray ja = JArray.Parse(SiteConfig.HelpList);
            foreach (JObject jo in ja)
            {
                MenuButton menuItem = new MenuButton();
                menuItem.EnablePostBack = false;
                menuItem.Text = jo.Value<string>("Text");
                menuItem.Icon = IconHelper.String2Icon(jo.Value<string>("Icon"), true);
                menuItem.OnClientClick = String.Format("addMainTab('{0}','{1}','{2}')", jo.Value<string>("ID"), ResolveUrl(jo.Value<string>("URL")), jo.Value<string>("Text"));
                btnHelp.Menu.Items.Add(menuItem);
            }
        }


        /// <summary>递归生成菜单树</summary>
        void BuildTree(List<DAL.Menu> menus, DAL.Menu parentMenu, FineUI.TreeNodeCollection nodes)
        {
            foreach (var menu in menus.Where(m => m.Parent == parentMenu).Where(t => t.Visible==true))
            {
                FineUI.TreeNode node = new FineUI.TreeNode();
                nodes.Add(node);
                node.Text = menu.Name;
                node.IconUrl = menu.ImageUrl;
                node.Expanded = menu.IsOpen && !menu.IsTreeLeaf;
                if (!String.IsNullOrEmpty(menu.NavigateUrl))
                    node.NavigateUrl = ResolveUrl(menu.NavigateUrl);

                if (menu.IsTreeLeaf)
                    node.Leaf = true;
                else
                    BuildTree(menus, menu, node.Nodes);
            }
        }

    }
}
