using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestFineUI.Plugins.MdEditors
{
    /// <summary>
    /// 图像和文件上传处理
    /// </summary>
    public class Uploader : IHttpHandler
    {
        public bool IsReusable
        {
            get {return false;}
        }


        public void ProcessRequest(HttpContext context)
        {
            // 获取
            string folder = "./Files";
            var files = context.Request.Files;
            for (int i=0; i<files.Count; i++)
            {
                // 保存文件
                HttpPostedFile file = files[i];
                string fileName = string.Format("{0}-{1}", System.DateTime.Now.ToString("yyyyMMddHHmmssfffffff"), file.FileName);
                string pysicalName = string.Format("{0}\\{1}", context.Server.MapPath(folder), fileName);
                string virtualName = string.Format("{0}/{1}", folder, fileName);
                file.SaveAs(pysicalName);

                // 反馈JSON给客户端
                string json = string.Format("{{\"success\":1, \"message\": \"上传成功\", \"url\": \"{0}\" }}", virtualName);
                context.Response.ContentType = "text/html";
                context.Response.Charset = "utf-8";
                context.Response.Write(json);
            }
        }

    }
}