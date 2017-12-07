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
    /// 用EChartRender 渲染FineUI Label控件。
    /// 直接渲染会找不到，要延缓加载
    /// </summary>
    public partial class TestEChart3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //ShowChart1();  // 直接渲染失败，会找不到动态创建的控件
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
            series.Add(new Serie { Name = "销量", DataField = "Sale", Type = SerieType.Bar, Symbol = SerieSymbol.Circle });
            EChartRender.Render(this.Chart1.ClientID, data, "Product", series, "销售情况", null);
        }

        // 用定时器延缓加载成功
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            this.Timer1.Enabled = false;
            ShowChart1();
        }
    }
}