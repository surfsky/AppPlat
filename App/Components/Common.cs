using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Reflection;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.IO;
using System.Configuration;
using System.Drawing;
using App.DAL;
using App.Components;

namespace App
{
    /// <summary>
    /// 公共方法和全局变量常量
    /// 文件过于庞大时请拆分成多个辅助类
    /// 配置信息请放置在 SiteConfig.cs 类中
    /// </summary>
    public partial class Common
    {
        //-------------------------------------
        // 常量
        //-------------------------------------
        public readonly static string SESSION_VERIFYCODE = "session_code";                  // 验证码Session名称
        public readonly static string SESSION_ONLINE_UPDATE_TIME = "OnlineUpdateTime";      // 在线人数最后更新时间
        public readonly static string CHECK_POWER_FAIL_ACTION_MESSAGE = "您无权进行此操作！";
        public readonly static string CHECK_POWER_FAIL_PAGE_MESSAGE = string.Format("您无权访问此页面！请重新<a href='{0}'>登录</a>", FormsAuthentication.LoginUrl);


        /// <summary>Js格式化</summary>
        public static string FormatScript( string source)
        {
            if (source == null) return "";
            JSBeautifyLib.JSBeautify jsb = new JSBeautifyLib.JSBeautify(source, new JSBeautifyLib.JSBeautifyOptions());
            return jsb.GetResult();
        }

        /// <summary>微信程序主目录</summary>
        public static string WechatRoot
        {
            get
            {
                return string.Format("{0}/WeiXin/{1}", Asp.Host, SiteConfig.Site);
            }
        }


        //-----------------------------------------
        // 权限检测相关
        //-----------------------------------------
        /// <summary>当前登录用户</summary>
        public static User LoginUser
        {
            get { return Asp.GetSessionData("LoginUser", () => DAL.User.GetDetail(null, AuthHelper.GetIdentityName())) as User; }
            set { Asp.Session["LoginUser"] = value; }
        }

        /// <summary>刷新登录用户</summary>
        public static void RefreshLoginUser()
        {
            Common.LoginUser = DAL.User.GetDetail(name: AuthHelper.GetIdentityName());
        }

        /// <summary>检查是否登录</summary>
        public static bool CheckLogin()
        {
            return AuthHelper.IsLogin();
            //return (LoginUser != null);
        }

        /// <summary>检查当前用户是否拥有某个角色</summary>
        public static bool CheckRole(RoleType? role)
        {
            if (role == null)                return true;
            if (LoginUser == null)           return false;
            return LoginUser.HasRole(role.Value);
        }

        /// <summary>检查当前用户是否拥有某个权限</summary>
        public static bool CheckPower(PowerType? power)
        {
            if (power == null)               return true;
            if (LoginUser == null)           return false;
            if (LoginUser.Name == "admin")   return true;
            return LoginUser.Powers.Contains(power.Value);
        }

        // 检测页面访问权限
        public static bool CheckPagePower(PowerType power)
        {
            if (!Common.CheckPower(power))
            {
                Asp.Response.Write(Common.CHECK_POWER_FAIL_PAGE_MESSAGE);
                Asp.Response.End();
                return false;
            }
            return true;
        }


        //-----------------------------------------
        // 其它
        //-----------------------------------------
        // 检测图片类型
        public static bool IsImageFile(string fileName)
        {
            fileName = fileName.ToLower();
            int n = fileName.LastIndexOf('.');
            if (n == -1)
                return false;

            string ext = fileName.Substring(n);
            string[] exts = new string[] { ".jpg", ".png", ".gif", ".jpeg", ".bmp" };
            return exts.Contains(ext);
        }


        /// <summary>获取上传文件要保存的路径名</summary>
        public static string GetUploadFilePath(string folderName, string fileName)
        {
            string folder = string.Format("~/Files/{0}", folderName);
            fileName = fileName.Replace(":", "_").Replace(" ", "_").Replace("\\", "_").Replace("/", "_");
            fileName = string.Format("{0}-{1}", DateTime.Now.ToString("yyyyMMddHHmmssfffffff"), fileName);
            return Asp.ResolveUrl(string.Format("{0}/{1}", folder, fileName));
        }

        /// <summary>安全删除文件</summary>
        public static void SafeDeleteFile(string fileUrl)
        {
            try
            {
                if (!fileUrl.Contains("/res/"))
                    System.IO.File.Delete(HttpContext.Current.Server.MapPath(fileUrl));
            }
            catch { }
        }

        /// <summary>拷贝文件。若文件名2未填写，则用guid替代。</summary>
        public static string CopyFile(string url1, string url2 = "")
        {
            string path1 = Asp.Server.MapPath(url1);
            string path2 = Asp.Server.MapPath(url2);
            if (url2.IsNullOrEmpty())
            {
                int n = url1.LastIndexOf("/");
                var path = url1.Substring(0, n);
                var fileInfo = new FileInfo(path1);
                var folder = fileInfo.Directory.FullName;
                var name = Guid.NewGuid().ToString();
                string extension = fileInfo.Extension;
                path2 = string.Format("{0}\\{1}{2}", folder, name, extension);
                url2 = string.Format("{0}/{1}{2}", path, name, extension);
            }
            try
            {
                File.Copy(path1, path2);
            }
            catch { }
            return url2;
        }
    }
}
