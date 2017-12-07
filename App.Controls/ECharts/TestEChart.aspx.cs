using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.Controls.ECharts
{
    /// <summary>
    /// EChart控件测试。
    /// </summary>
    /// <remarks>
    /// 善无法直接在EChart内部放置xml子标签，如： &lt;YAxis&gl;
    /// </remarks>
    public partial class TestEChart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ShowChart1();
                ShowChart2();
            }
        }

        void ShowChart1()
        {
            var data = new ArrayList();
            data.Add(new { Product = "衬衫", Sale = 5, Inc=20 });
            data.Add(new { Product = "羊毛衫", Sale = 50, Inc = 60 });
            data.Add(new { Product = "裤子", Sale = 15, Inc = 80 });
            data.Add(new { Product = "高跟鞋", Sale = 25, Inc = 120 });
            data.Add(new { Product = "袜子", Sale = 20, Inc = 300 });

            var series = new List<Serie>();
            series.Add(new Serie { Name = "销量", DataField = "Sale", Type = SerieType.Bar });
            Chart1.Show(data, "Product", series, "销售情况", null);
        }


        void ShowChart2()
        {
            var data = new ArrayList();
            data.Add(new { Product = "衬衫", Sale = 5, Inc = 20 });
            data.Add(new { Product = "羊毛衫", Sale = 50, Inc = 60 });
            data.Add(new { Product = "裤子", Sale = 15, Inc = 80 });
            data.Add(new { Product = "高跟鞋", Sale = 25, Inc = 120 });
            data.Add(new { Product = "袜子", Sale = 20, Inc = 300 });

            var series = new List<Serie>();
            series.Add(new Serie { Name = "销量", DataField = "Sale", Type = SerieType.Bar });
            series.Add(new Serie { Name = "增速", DataField = "Inc", Type = SerieType.Line });
            var yAxis = new List<YAxis>();
            yAxis.Add(new YAxis() { Name = "销量" });
            yAxis.Add(new YAxis() { Name = "增速" });
            Chart2.Show(data, "Product", series, "销售情况", yAxis);
        }
    }
}