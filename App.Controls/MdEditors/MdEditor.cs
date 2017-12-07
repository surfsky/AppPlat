using FineUI;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Xml.Serialization;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;

namespace App.Controls
{
    /// <summary>
    ///  Markdown 编辑器控件。该控件是 editor.md 的简单封装。
    /// </summary>
    /// <remarks>
    /// （1）标准aspx页面直接用属性 Text 进行读写
    /// （2）内嵌在 FineUI ContentPanel中时，请用AjaxSetText()方法来刷新其文本。
    /// </remarks>
    [ToolboxBitmap(typeof(MdEditor), "App.Controls.ECharts.echarts.bmp")]
    public class MdEditor : System.Web.UI.HtmlControls.HtmlTextArea
    {
        //---------------------------------------------------
        // 属性
        //---------------------------------------------------
        /// <summary>脚本根路径</summary>
        public string ScriptPath { get; set; }

        /// <summary>图像上传处理地址</summary>
        public string ImageUploadUrl { get; set; }

        public Unit Width { get; set; }
        public Unit Height { get; set; }
        public string Text
        {
            get { return base.Value; }
            set { base.Value = value; }
        }

        //---------------------------------------------------
        // 事件
        //---------------------------------------------------
        // 页面初始化：注册<script>标签
        // 放置多个该控件时，应该做下检测。虽然运行起来没问题。
        protected override void OnInit(EventArgs e)
        {
            string path = this.ResolveClientUrl(ScriptPath).TrimEnd('/');
            string md = string.Format("{0}/editormd.js", path);
            string jquery = string.Format("{0}/jquery.min.js", path);
            string css = string.Format("{0}/css/editormd.css", path);
            RegistCSS(css);
            RegistScript(jquery);
            RegistScript(md);
            base.OnInit(e);
        }

        //-------------------------------------
        // 在页面头部注册脚本
        //-------------------------------------
        static void RegistCSS(string url)
        {
            HtmlLink css = new HtmlLink();
            css.Href = url;
            css.Attributes.Add("rel", "stylesheet");
            css.Attributes.Add("type", "text/css");
            (HttpContext.Current.Handler as Page).Header.Controls.Add(css);
        }

        static void RegistScript(string url)
        {
            HtmlGenericControl script = new HtmlGenericControl("script");
            script.Attributes.Add("type", "text/javascript");
            script.Attributes.Add("src", url);
            (HttpContext.Current.Handler as Page).Header.Controls.Add(script);
        }

        // 渲染
        protected override void Render(HtmlTextWriter writer)
        {
            string wrapper = string.Format("{0}_wrapper", this.ClientID);
            string path = this.ResolveClientUrl(ScriptPath).TrimEnd('/');
            writer.Write(string.Format("<div id='{0}'>", wrapper));
            base.Render(writer);
            writer.Write("</div>");

            string tmpl = @" var editor_%ID% = editormd('%ID%', 
            {
                width: '%WIDTH%',
                height: '%HEIGHT%',
                path: '%PATH%/lib/',
                watch: false,
                tex: true,
                tocm: true,
                emoji: true,
                taskList: true,
                codeFold: true,
                searchReplace: true,
                flowChart: true,
                sequenceDiagram: true,
                htmlDecode: 'style,script,iframe',
                toolbarIcons: function()
                {
                    return [
                          'h1', 'h2', 'h3', '|',
                          'bold', 'del', 'italic', 'quote', '|',
                          'list-ul', 'list-ol', 'hr', 'table', '|',
                          'link', 'image', '||',
                          'watch', 'fullscreen'
                    ]
                },
                imageUpload: true,
                imageFormats: ['jpg', 'jpeg', 'gif', 'png', 'bmp', 'webp'],
                imageUploadURL: '%UPLOADURL%'
            });";
            string script = tmpl.Replace("%ID%", wrapper)
                .Replace("%WIDTH%", this.Width.ToString())
                .Replace("%HEIGHT%", this.Height.ToString())
                .Replace("%PATH%", path)
                .Replace("%UPLOADURL%", this.ImageUploadUrl)
                ;
            writer.Write("<script type='text/javascript'>" + script + "</script>");
        }

        /// <summary>
        /// 设置文本（用Ajax方式，发送刷新脚本到客户端执行。与FineUI混用时请用该方法设置文本值。）
        /// </summary>
        public void AjaxSetText(string text)
        {
            string wrapper = string.Format("{0}_wrapper", this.ClientID);
            string script = string.Format("editor_{0}.setMarkdown({1});", wrapper, JsonConvert.SerializeObject(text));
            FineUI.PageContext.RegisterStartupScript(script);
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "MdSetter", script);
        }
    }
}