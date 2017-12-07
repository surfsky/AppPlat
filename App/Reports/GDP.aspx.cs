using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUI;
using System.Linq;
using System.Data.Entity;
using EntityFramework.Extensions;
using App.DAL;
using App.Controls;
using App.Components;
using App.Controls.ECharts;

namespace App.Reports
{
    [Auth(PowerType.ReportView)]
    public partial class GDP : PageBase
    {
        // Init
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Grid1.NewUrlTmpl = "ReportDataEdit.aspx?type=App.DAL.RptGDP&id=-1";
            this.Grid1.EditUrlTmpl = "ReportDataEdit.aspx?type=App.DAL.RptGDP&id={0}";
            this.Grid1.ViewUrlTmpl = "ReportDataView.aspx?type=App.DAL.RptGDP&id={0}";
            this.Grid1.AllowNew = Common.CheckPower(PowerType.ReportEdit);
            this.Grid1.AllowBatchDelete = Common.CheckPower(PowerType.ReportEdit);
            this.Grid1.AllowDelete = Common.CheckPower(PowerType.ReportEdit);
            this.Grid1.AllowEdit = Common.CheckPower(PowerType.ReportEdit);
            this.Grid1.InitGrid<RptGDP>(this.BindGrid, Toolbar1);

            if (!IsPostBack)
            {
                this.Grid1.SetSortAndPage<RptGDP>(true, true, SiteConfig.PageSize, t => t.Quarter);
                BindGrid();
            }
        }


        // 绑定数据
        void BindGrid()
        {
            var data = GetQuery();
            Grid1.BindGrid(data);
        }
        IQueryable<RptGDP> GetQuery()
        {
            return RptGDP.SearchByQuarter(this.txtFrom.Text, this.txtTo.Text);
        }

        // 搜索
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SwitchView(true, false);
            BindGrid();
        }

        //-------------------------------------------------------
        // 导出
        //-------------------------------------------------------
        // 导出excel
        protected void Grid1_Export(object sender, EventArgs e)
        {
            this.Grid1.ExportExcel(GetQuery().ToList());
        }


        //-------------------------------------------------------
        // 图表
        //-------------------------------------------------------
        // 图表1
        protected void btnChart1_Click(object sender, EventArgs e)
        {
            SwitchView(false, true);
            var data = GetQuery().ToList();
            List<Serie> series = new List<Serie>();
            series.Add(new Serie { Name = "GDP增速", DataField = "GDPInc", Type = SerieType.Line, Symbol = SerieSymbol.EmptyCircle });
            series.Add(new Serie { Name = "一产增速", DataField = "FirstIndustryInc", Type = SerieType.Line, Symbol = SerieSymbol.Circle });
            series.Add(new Serie { Name = "二产增速", DataField = "SecondIndustryInc", Type = SerieType.Line, Symbol = SerieSymbol.EmptyRect });
            series.Add(new Serie { Name = "三产增速", DataField = "ThirdIndustryInc", Type = SerieType.Line, Symbol = SerieSymbol.Rect });
            EChartRender.Render(Chart1.ClientID, data, "Quarter", series, "GDP及三次产业增加值增长对比");
        }

        // 图表2
        protected void btnChart2_Click(object sender, EventArgs e)
        {
            SwitchView(false, true);
            var data = GetQuery().ToList();
            List<Serie> series = new List<Serie>();
            series.Add(new Serie { Name = "温州增速", DataField = "GDPInc", Type = SerieType.Line, Symbol = SerieSymbol.EmptyCircle });
            series.Add(new Serie { Name = "全国增速", DataField = "CountryInc", Type = SerieType.Line, Symbol = SerieSymbol.Circle });
            series.Add(new Serie { Name = "浙江增速", DataField = "ZheJiangInc", Type = SerieType.Line, Symbol = SerieSymbol.EmptyRect });
            EChartRender.Render(Chart1.ClientID, data, "Quarter", series, "温州与全国、全省GDP逐季增长对比");
        }


        // 切换视图
        private void SwitchView(bool showGrid, bool showChart)
        {
            this.Grid1.Hidden = !showGrid;
            this.Chart1.Hidden = !showChart;
        }

    }
}
