using App.Components;
using App.Controls;
using App.Controls.ECharts;
using App.DAL;
using App.Reports;
using Kingsoc.Web.WebCall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.Admins
{
    /// <summary>
    /// 欢迎页面
    /// - 经试验，直接用EChart控件的话，要放置在ContentPanel里面，也无法自动适配大小，也无法显示动画效果
    /// - 本页面采用Timer来异步加载图表
    ///     FineUI 的对象的是动态创建的，如果直接输出图表脚本，会找不到要渲染的控件
    ///     用Timer也好，有个动态效果，也保证动态创建的对象都已经生成了。
    /// </summary>
    public partial class Welcome : PageBase
    {
        // Init
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.rptNews.DataSource = DAL.Article.GetNews(0, 10);
                this.rptNews.DataBind();
                ShowChart1();
                ShowChart2();
                ShowChart3();
            }
        }

        // 此处用定时器异步加载，不然会脚本冲突，找不到对象。
        protected void timer_Tick(object sender, EventArgs e)
        {
            timer.Enabled = false;
            ShowChart1();
            ShowChart2();
            ShowChart3();
        }

        IQueryable<RptGDP> GetQuery()
        {
            return RptGDP.SearchByQuarter("2017Q1", "2020Q4");
        }

        // 图表1
        protected void ShowChart1()
        {
            var data = GetQuery().ToList();
            List<Serie> series = new List<Serie>();
            series.Add(new Serie { Name = "GDP增速", DataField = "GDPInc", Type = SerieType.Line, Symbol = SerieSymbol.EmptyCircle });
            series.Add(new Serie { Name = "一产增速", DataField = "FirstIndustryInc", Type = SerieType.Line, Symbol = SerieSymbol.Circle });
            series.Add(new Serie { Name = "二产增速", DataField = "SecondIndustryInc", Type = SerieType.Line, Symbol = SerieSymbol.EmptyRect });
            series.Add(new Serie { Name = "三产增速", DataField = "ThirdIndustryInc", Type = SerieType.Line, Symbol = SerieSymbol.Rect });
            EChartRender.Render(Chart1.ClientID, data, "Quarter", series, "GDP及三次产业增加值增长对比");
        }


        // 图表2
        protected void ShowChart2()
        {
            var data = GetQuery().ToList();
            List<Serie> series = new List<Serie>();
            series.Add(new Serie { Name = "GDP增速", DataField = "GDPInc", Type = SerieType.Line, Symbol = SerieSymbol.EmptyCircle });
            series.Add(new Serie { Name = "一产增速", DataField = "FirstIndustryInc", Type = SerieType.Line, Symbol = SerieSymbol.Circle });
            series.Add(new Serie { Name = "二产增速", DataField = "SecondIndustryInc", Type = SerieType.Line, Symbol = SerieSymbol.EmptyRect });
            series.Add(new Serie { Name = "三产增速", DataField = "ThirdIndustryInc", Type = SerieType.Line, Symbol = SerieSymbol.Rect });
            EChartRender.Render(Chart2.ClientID, data, "Quarter", series, "GDP及三次产业增加值增长对比");
        }

        // 图表3
        protected void ShowChart3()
        {
            var data = GetQuery().ToList();
            List<Serie> series = new List<Serie>();
            series.Add(new Serie { Name = "GDP增速", DataField = "GDPInc", Type = SerieType.Line, Symbol = SerieSymbol.EmptyCircle });
            series.Add(new Serie { Name = "一产增速", DataField = "FirstIndustryInc", Type = SerieType.Line, Symbol = SerieSymbol.Circle });
            series.Add(new Serie { Name = "二产增速", DataField = "SecondIndustryInc", Type = SerieType.Line, Symbol = SerieSymbol.EmptyRect });
            series.Add(new Serie { Name = "三产增速", DataField = "ThirdIndustryInc", Type = SerieType.Line, Symbol = SerieSymbol.Rect });
            EChartRender.Render(Chart3.ClientID, data, "Quarter", series, "GDP及三次产业增加值增长对比");
        }

    }
}
