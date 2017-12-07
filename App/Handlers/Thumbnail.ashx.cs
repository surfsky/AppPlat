using App.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace App.Handlers
{
    /// <summary>
    /// 缩略图页面处理器
    /// http://localhost:5625/handlers/Thumbnail.ashx?url=~/res/images/defaultuser.png&w=10&h=10 
    /// http://localhost:5625/handlers/Thumbnail.ashx?url=./res/images/defaultuser.png&w=10&h=10 
    /// http://localhost:5625/handlers/Thumbnail.ashx?url=https://ss0.bdstatic.com/5aV1bjqh_Q23odCf/static/superman/img/logo/bd_logo1_31bdc765.png&w=10&h=10 
    /// </summary>
    public class Thumbnail : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string url = Asp.GetQueryString("url");
            int w = Asp.GetQueryIntValue("w") ?? -1;
            int h = Asp.GetQueryIntValue("h") ?? -1;
            Image img = HttpHelper.GetThumbnail(url, w, h);
            HttpHelper.SetCache(context, 600);
            HttpHelper.WriteImage(img, "");
        }
    }
}