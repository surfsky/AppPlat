using System;
using System.Web.UI;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using FineUI;
using App.DAL;
using App.Components;
using System.Web.SessionState;

namespace App
{
    /// <summary>访问鉴权</summary>
    /// <example>
    /// [AccessPower(PowerType.UserView, PowerType.UserEdit, PowerType.UserNew)]
    /// public class UserPage : Page {...}
    /// </example>
    public class AuthAttribute : Attribute
    {
        public bool CheckLogin { get; set; } = false;
        public RoleType? ViewRole { get; set; }
        public PowerType? ViewPower { get; set; }
        public PowerType? EditPower { get; set; }
        public PowerType? NewPower  { get; set; }
        public AuthAttribute() { }
        public AuthAttribute(PowerType viewPower)
        {
            this.ViewPower = viewPower;
        }
    }

    /// <summary>
    /// 页面访问模式
    /// </summary>
    public enum PageMode
    {
        View,
        New,
        Edit
    }

    //-----------------------------------------------------
    // HandlerBase
    //-----------------------------------------------------
    /// <summary>
    /// 处理器基类，集成了以下功能（1）访问权限（2）在线用户
    /// </summary>
    public class HandlerBase : IHttpHandler, IRequiresSessionState
    {
        /// <summary>本页面的浏览权限，空字符串表示本页面不受权限控制。该值来自 ViewPowerAttribute</summary>
        /// <see cref="AuthAttribute"/>
        protected AuthAttribute Auth
        {
            get { return ReflectionHelper.GetAttribute<AuthAttribute>(this.GetType());}
        }

        public bool IsReusable { get { return true; } }
        public void ProcessRequest(HttpContext context)
        {
            // 此用户是否有访问此页面的权限
            if (!Common.CheckPower(this.Auth?.ViewPower))
            {
                context.Response.StatusCode = 501;
                context.Response.End();
                return;
            }

            // 在线用户
            if (context.User != null)
                Online.UpdateOnlineUser(context.User.Identity.Name);
            Process(context);
        }

        // 请在子类中override实现
        public virtual void Process(HttpContext context)
        {
            throw new NotImplementedException("");
        }
    }


    //-----------------------------------------------------
    // PageBase
    //-----------------------------------------------------
    /// <summary>
    /// 页面基类，集成了以下功能（1）访问权限（2）在线用户（3）主题（4）标题。
    /// 页面访问权限可直接写Attribute: [Auth(PowerType.UserEdit)]
    /// </summary>
    public class PageBase : Page
    {
        /// <summary>本页面的访问权限。</summary>
        /// <see cref="AuthAttribute"/>
        protected AuthAttribute Auth
        {
            get {return ReflectionHelper.GetAttribute<AuthAttribute>(this.GetType()); }
        }

        /// <summary>页面模式（从ViewState或RequestString中获取）</summary>
        public PageMode Mode
        {
            get
            {
                if (ViewState["PageMode"] != null)
                    return (PageMode)(Enum.Parse(typeof(PageMode), ViewState["PageMode"].ToString()));
                else if (Request.QueryString["mode"] != null)
                {
                    string mode = Request.QueryString["mode"].ToLower();
                    if (mode == "view") return PageMode.View;
                    if (mode == "new")  return PageMode.New;
                    if (mode == "edit") return PageMode.Edit;
                }
                return PageMode.Edit;
            }
            set
            {
                ViewState["PageMode"] = value.ToString();
            }
        }

        // 初始化
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // 检测权限（编辑权限可降权为阅读权限）
            if (this.Mode == PageMode.Edit)
                if (!Common.CheckPower(this.Auth?.EditPower))
                    this.Mode = PageMode.View;
            if (this.Mode == PageMode.View)
                if (!Common.CheckPower(this.Auth?.ViewPower))
                    return;
            if (this.Mode == PageMode.New)
                if (!Common.CheckPower(this.Auth?.NewPower))
                    return;
            if (this.Auth != null)
            {
                if (this.Auth.CheckLogin && !Common.CheckLogin())
                    return;
                if (this.Auth.ViewRole != null && !Common.CheckRole(this.Auth.ViewRole))
                    return;
            }

            // 在线用户数、页面标题、主题
            if (this.User != null)
                Online.UpdateOnlineUser(User.Identity.Name);
            Page.Title = SiteConfig.SiteTitle;
            if (PageManager.Instance != null && SiteConfig.Theme != null)
                PageManager.Instance.Theme = (Theme)Enum.Parse(typeof(Theme), SiteConfig.Theme, true);
        }
         
    }
}
