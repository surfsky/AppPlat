using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using App.Components;
using App.WeiXin;

namespace App.Test
{
    /// <summary>
    /// 创建微信菜单
    /// </summary>
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WechatMenu menu = new WechatMenu();
            //root.button.Add(new Button("健身房", WechatButtonType.view, "http://122.228.187.249/WeiXin/Stores.aspx");
            menu.button.Add(new WechatButton("首页",   WechatButtonType.view, "http://www.weibofit.com/WeiXin/WeiboFit/Index.aspx?type=Index"));
            menu.button.Add(new WechatButton("课程",   WechatButtonType.view, "http://www.weibofit.com/WeiXin/WeiboFit/FitClasses.aspx?type=FitClasses"));
            menu.button.Add(new WechatButton("健身卡", WechatButtonType.view, "http://www.weibofit.com/WeiXin/WeiboFit/FitCards.aspx?type=FitCards"));
            menu.button.Add(new WechatButton("我的",   WechatButtonType.view, "http://www.weibofit.com/WeiXin/WeiboFit/AuthPage.aspx"));
            Response.Write(WechatHelper.SetMenu(menu));
        }

    }

    

}