using App.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using Kingsoc.Web.WebCall;
using System.Net;

namespace App.DAL
{
    /// <summary>
    /// 通用接口
    /// </summary>
    public class DbCommon
    {
        /// <summary>获取缩略图</summary>
        /// <example>
        /// http://localhost:5625/WebCall.App.DAL.DbCommon.axd/GetThumbnail?url=~/res/images/defaultuser.pngw=10h=10 
        /// http://localhost:5625/WebCall.App.DAL.DbCommon.axd/GetThumbnail?url=./res/images/defaultuser.pngw=10h=10 
        /// http://localhost:5625/WebCall.App.DAL.DbCommon.axd/GetThumbnail?url=https://ss0.bdstatic.com/5aV1bjqh_Q23odCf/static/superman/img/logo/bd_logo1_31bdc765.pngw=10h=10 
        /// </example>
        [WebCall(Description="获取缩略图", Type=ResponseDataType.Image, CacheDuration = 5)]
        public static Image GetThumbnail(string url, int w, int h=-1)
        {
            Image img;
            if (url.StartsWith("~/") || url.StartsWith(".") || url.StartsWith("/"))
                img = Image.FromFile(Common.Server.MapPath(url));
            else if (IsLocalFile(url))
                img = Image.FromFile(Common.Server.MapPath(url));
            else
                img = HttpHelper.GetNetworkImage(url);
            return DrawHelper.CreateThumbnail(img, w, h);
        }

        /// <summary>是否是本网站文件</summary>
        public static bool IsLocalFile(string url)
        {
            url = Common.ResolveUrl(url);
            Uri uri = new Uri(url);
            return uri.Host.ToLower() == Common.Host.ToLower();
        }
    }
}