using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace App.Components
{
    /// <summary>
    /// ASP.NET 网页相关辅助方法
    /// </summary>
    public class Asp
    {
        // 在页面头部注册CSS
        public static void RegistCSS(string url)
        {
            HtmlLink css = new HtmlLink();
            css.Href = url;
            css.Attributes.Add("rel", "stylesheet");
            css.Attributes.Add("type", "text/css");
            (HttpContext.Current.Handler as Page).Header.Controls.Add(css);
        }

        // 在页面头部注册脚本
        public static void RegistScript(string url)
        {
            HtmlGenericControl script = new HtmlGenericControl("script");
            script.Attributes.Add("type", "text/javascript");
            script.Attributes.Add("src", url);
            (HttpContext.Current.Handler as Page).Header.Controls.Add(script);
        }


        /// <summary>
        /// 创建POST表单并跳转页面
        /// </summary>
        public static void CreateFormAndPost(Page page, string url, Dictionary<string, string> data)
        {
            // 构建表单
            string formID = "PostForm";
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"<form id=""{0}"" name=""{0}"" action=""{1}"" method=""POST"">", formID, url);
            foreach (var item in data)
                sb.AppendFormat(@"<input type=""hidden"" name=""{0}"" value='{1}'>", item.Key, item.Value);
            sb.Append("</form>");

            // 创建js执行Form
            sb.Append(@"<script type=""text/javascript"">");
            sb.AppendFormat("var postForm = document.{0};", formID);
            sb.Append("postForm.submit();");
            sb.Append("</script>");
            page.Controls.Add(new LiteralControl(sb.ToString()));
        }

        //-------------------------------------
        // HttpContext
        //-------------------------------------
        public static HttpServerUtility Server { get { return HttpContext.Current.Server; } }
        public static HttpRequest Request { get { return HttpContext.Current.Request; } }
        public static HttpResponse Response { get { return HttpContext.Current.Response; } }
        public static HttpSessionState Session { get { return HttpContext.Current.Session; } }
        public static HttpApplicationState Application { get { return HttpContext.Current.Application; } }
        public static Page Page { get { return HttpContext.Current.Handler as Page; } }


        /// <summary>获取主机根路径</summary>
        public static string Host
        {
            get
            {
                Uri url = HttpContext.Current.Request.Url;
                return url.Port == 80
                    ? string.Format("{0}://{1}", url.Scheme, url.Host)
                    : string.Format("{0}://{1}:{2}", url.Scheme, url.Host, url.Port)
                    ;
            }
        }


        /// <summary>获取客户端真实IP</summary>
        public static string GetClientIP()
        {
            HttpRequest request = HttpContext.Current.Request;
            return (request.ServerVariables["HTTP_VIA"] != null)
                ? request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString()   // 使用代理，尝试去找原始地址
                : request.ServerVariables["REMOTE_ADDR"].ToString()            // 
                ;
            //return request.UserHostAddress;
        }

        // Session 相关
        public static void SetSession(string name, object value)
        {
            SetSession(name, value, 20);
        }

        public static void SetSession(string name, object value, int expireMinutes)
        {
            HttpContext.Current.Session[name] = value;
            HttpContext.Current.Session.Timeout = expireMinutes;
        }

        public static object GetSession(string name)
        {
            return HttpContext.Current.Session[name];
        }


        //-------------------------------------------
        // Url 转换辅助函数
        //-------------------------------------------
        /// <summary>
        /// 将 URL 转化为从根目录开始的路径。如:
        /// （1）../default.aspx 转化为 /application1/default.aspx
        /// （2）~/default.aspx 转化为 /application1/default.aspx
        /// </summary>
        public static string ResolveUrl(string relativeUrl)
        {
            return new Control().ResolveUrl(relativeUrl);
        }

        /// <summary>
        /// 将 URL 转化为相对于浏览器当前路径的相对路径。
        /// 如浏览器当前为 /pages/test.aspx，则
        /// （1）/pages/default.aspx 转化为 default.aspx
        /// （2）~/default.aspx      转化为 ../default.aspx
        /// </summary>
        public static string ResolveClientUrl(string relativeUrl)
        {
            return new Control().ResolveClientUrl(relativeUrl);
        }



        //-------------------------------------
        // QueryString
        //-------------------------------------
        /// <summary>获取查询字符串</summary>
        public static string GetQueryString(string queryKey)
        {
            return HttpContext.Current.Request.QueryString[queryKey];
        }

        /// <summary>获取查询字符串中的整型参数值</summary>
        public static int? GetQueryIntValue(string queryKey)
        {
            int result = -1;
            string str = HttpContext.Current.Request.QueryString[queryKey];
            if (!string.IsNullOrEmpty(str) && Int32.TryParse(str, out result))
                return result;
            return null;
        }

        /// <summary>获取查询字符串中的boolean参数值</summary>
        public static bool? GetQueryBoolValue(string queryKey)
        {
            bool result = false;
            string str = HttpContext.Current.Request.QueryString[queryKey];
            if (!string.IsNullOrEmpty(str) && Boolean.TryParse(str, out result))
                return result;
            return null;
        }

        /// <summary>获取查询字符串中的枚举参数值</summary>
        public static T? GetQueryEnumValue<T>(string queryKey) where T : struct
        {
            string str = HttpContext.Current.Request.QueryString[queryKey];
            if (!string.IsNullOrEmpty(str))
            {
                Enum.TryParse<T>(str, true, out T result);
                return result;
            }
            return null;
        }


        //------------------------------------------------------------
        // 环境数据获取方法：Cache & Session & HttpContext & Application
        // 若使用泛型方法存储简单值的数据，as 转化会有报错，故还是用非泛型方法，更通用一些。
        // 泛型方法以后再想办法
        //------------------------------------------------------------
        /// <summary>获取Session数据（会话期有效）</summary>
        public static object GetSessionData(string key, Func<object> creator = null)
        {
            if (HttpContext.Current.Session == null)
                return null;

            if (creator != null && HttpContext.Current.Session[key] == null)
                HttpContext.Current.Session[key] = creator();
            return HttpContext.Current.Session[key];
        }

        /// <summary>获取上下文数据（在每次请求中有效）</summary>
        public static object GetContextData(string key, Func<object> creator = null)
        {
            if (creator != null && !HttpContext.Current.Items.Contains(key))
                HttpContext.Current.Items[key] = creator();
            return HttpContext.Current.Items[key];
        }

        /// <summary>获取 Application 数据（网站开启一直有效）</summary>
        public static object GetApplicationData(string key, Func<object> creator = null)
        {
            if (creator == null && !Application.AllKeys.Contains(key))
                Application[key] = creator();
            return Application[key];
        }

        /// <summary>获取缓存对象（缓存有有效期，一旦失效，自动获取）</summary>
        public static object GetCacheData(string key, DateTime expiredTime, Func<object> creator = null)
        {
            if (creator != null && HttpContext.Current.Cache[key] == null)
            {
                var o = creator();
                if (o != null)
                    HttpContext.Current.Cache.Insert(key, o, null, expiredTime, System.Web.Caching.Cache.NoSlidingExpiration);  // Cache.Insert若存在会覆盖的
            }
            return HttpContext.Current.Cache[key];
        }


        /*
        // 获取缓存对象（泛型版本）
        // return session == null ? default(T) : (T)session;
        public static T GetCacheData<T>(string key, DateTime expiredTime, Func<T> creator) where T : class
        {
            if (HttpContext.Current.Cache[key] == null)
            {
                T o = creator();
                if (o != null)
                    HttpContext.Current.Cache.Insert(key, o, null, expiredTime, System.Web.Caching.Cache.NoSlidingExpiration);
            }
            return HttpContext.Current.Cache[key] as T;
        }
        */
    }
}