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
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace App.Controls.ECharts
{   

    /// <summary>
    /// 图表中的系列
    /// </summary>
    public class Serie
    {
        /// <summary>类型：bar|line</summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SerieType Type { get; set; } = SerieType.Line;

        /// <summary>属性名</summary>
        [JsonIgnore]
        public string DataField { get; set; }

        /// <summary>名称</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>标记类型</summary>
        [JsonProperty("symbol")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SerieSymbol Symbol { get; set; } = SerieSymbol.Circle;


        [JsonProperty("showSymbol")]
        public bool ShowSymbol { get; set; } = true;

        [JsonProperty("showAllSymbol")]
        public bool ShowAllSymbol { get; set; } = false;

        [JsonProperty("symbolSize")]
        public int SymbolSize { get; set; } = 6;

        /// <summary>用到的Y轴编号</summary>
        [JsonProperty("yAxisIndex")]
        public int YAxisIndex { get; set; } = 0;

        /// <summary>数据</summary>
        [JsonProperty("data")]
        public List<string> Data { get; set; } = new List<string>();
    }

    /// <summary>
    /// 系列类型
    /// </summary>
    public enum SerieType
    {
        [EnumMember(Value = "line")] Line,
        [EnumMember(Value = "bar")]  Bar,
    }

    /// <summary>
    /// 图标系列标记的类型
    /// </summary>
    public enum SerieSymbol
    {
        [EnumMember(Value = "emptyCircle")] EmptyCircle,
        [EnumMember(Value = "circle")]      Circle,
        [EnumMember(Value = "emptyRect")]   EmptyRect,
        [EnumMember(Value = "rect")]        Rect,
        [EnumMember(Value = "diamond")]     Diamond,
        [EnumMember(Value = "triangle")]    Triangle,

        // 以下三种易混淆或不好看，不用
        //[EnumMember(Value = "roundRect")]   RoundRect,
        //[EnumMember(Value = "pin")]         Pin,
        //[EnumMember(Value = "arrow")]       Arrow
    }

    /// <summary>
    /// 图表中的 Y 轴
    /// </summary>
    public class YAxis
    {
        /// <summary>名称</summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }


    /// <summary>
    /// ECharts 控件（实现线状、柱状图）
    /// 参考：http://echarts.baidu.com/
    /// </summary>
    /// <remarks>
    /// （1）无法实现内嵌js资源，始终报错：找不到 WebResource.axd 404错误
    /// （2）无法在aspx页面中直接写Series、YAxis等属性
    /// </remarks>
    [PersistChildren(true)]
    [XmlInclude(typeof(Serie))]
    [XmlInclude(typeof(YAxis))]
    [Designer(typeof(EChartDesigner))]
    [ToolboxBitmap(typeof(EChart), "App.Controls.ECharts.echart.bmp")]
    public class EChart : WebControl, INamingContainer
    {
        //---------------------------------------------------
        // 属性
        //---------------------------------------------------
        /// <summary>ECharts脚本路径</summary>
        public string ScriptPath { get; set; }

        /// <summary>图表标题</summary>
        public string Title { get; set; }

        /// <summary>X轴名称</summary>
        public string XAxisName { get; set; }


        /// <summary>图表系列</summary>
        [XmlArray]
        public List<Serie> Series { get; set; } = new List<Serie>();

        /// <summary>Y轴</summary>
        [XmlArray]
        public List<YAxis> YAxis { get; set; }

        /// <summary>数据源</summary>
        public IList Data { get; set; }
        public DataTable DataTable { get; set; }


        //---------------------------------------------------
        // 初始化
        //---------------------------------------------------
        // 两个简易的构造函数
        public void InitChart(DataTable data, string xAxisName, List<Serie> series, string title = "", List<YAxis> yAxis = null)
        {
            this.DataTable = data;
            this.XAxisName = xAxisName;
            this.Series = series;
            this.Title = title;
            this.YAxis = yAxis;
        }

        public void Show(IList data, string xAxisName, List<Serie> series, string title = "", List<YAxis> yAxis = null)
        {
            this.Data = data;
            this.XAxisName = xAxisName;
            this.Series = series;
            this.Title = title;
            this.YAxis = yAxis;
        }

        //---------------------------------------------------
        // 事件
        //---------------------------------------------------
        // 页面初始化事件：注册<script>标签
        // 打算用内嵌js资源，但老是找不到 WebResource.axd 404错误。
        // 算了，先用 ScriptUrl 属性，也便于升级。
        // 以后再想办法解决内嵌资源问题
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //string url = String.IsNullOrEmpty(ScriptPath)
            //    ? Page.ClientScript.GetWebResourceUrl(this.GetType(), "App.Controls.ECharts.echarts.min.js")
            //    : this.ResolveClientUrl(ScriptPath)
            //    ;

            if (!String.IsNullOrEmpty(ScriptPath))
            {
                string url = this.ResolveClientUrl(ScriptPath);
                if (!Page.ClientScript.IsClientScriptIncludeRegistered("EChartsInclude"))
                    Page.ClientScript.RegisterClientScriptInclude("EChartsInclude", url);
            }
        }



        // 渲染事件：写echarts初始化脚本
        protected override void Render(HtmlTextWriter writer)
        {
            string option = this.Data != null 
                ? EChartRender.GetOption(Data, XAxisName, Series, Title, YAxis) 
                : EChartRender.GetOption(DataTable, XAxisName, Series, Title, YAxis)
                ;
            string tag = string.Format("<div id='{0}' style='width:{1}; height:{2};' >echarts</div>", this.ClientID, this.Width, this.Height);
            string script = string.Format(@"echarts.init(document.getElementById('{0}')).setOption({1});",
                this.ClientID,
                option
                );

            writer.Write(tag);
            writer.Write("<script type='text/javascript'>" + script + "</script>");
        }
    }

    /// <summary>
    /// EChart 设计器支持
    /// </summary>
    public class EChartDesigner : ControlDesigner
    {
        public override string GetDesignTimeHtml()
        {
            EChart c = (EChart)this.Component;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<div style='width:{0}; height:{1}; border:solid 1px gray; padding:5px;'>{2}</div>", c.Width, c.Height, c.ID);
            return sb.ToString();
        }
    }

}