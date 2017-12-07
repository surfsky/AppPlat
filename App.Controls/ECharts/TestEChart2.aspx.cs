using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUI;
using System.Collections;

namespace App.Controls.ECharts
{
    /// <summary>
    /// EChart 嵌在FineUI中。用ContentPanel包裹，无动态效果。
    /// </summary>
    public partial class TestEChart2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ShowChart1();
            }
        }

        void ShowChart1()
        {
            var data = new ArrayList();
            data.Add(new { Product = "衬衫", Sale = 5, Inc = 20 });
            data.Add(new { Product = "羊毛衫", Sale = 50, Inc = 60 });
            data.Add(new { Product = "裤子", Sale = 15, Inc = 80 });
            data.Add(new { Product = "高跟鞋", Sale = 25, Inc = 120 });
            data.Add(new { Product = "袜子", Sale = 20, Inc = 300 });

            var series = new List<Serie>();
            series.Add(new Serie { Name = "销量", DataField = "Sale", Type = SerieType.Bar });
            Chart1.Show(data, "Product", series, "销售情况", null);
        }
    }
}