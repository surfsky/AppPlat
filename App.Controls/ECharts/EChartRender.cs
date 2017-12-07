using FineUI;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace App.Controls.ECharts
{
    /// <summary>
    /// EChart 渲染器
    /// </summary>
    public class EChartRender
    {
        static string _template = @"{
                title: {
                    text: %title%,
                    x: 'center',
                    y: 0
                },
                tooltip: {
                    trigger: 'axis',
                    axisPointer: {
                        type: 'cross',
                        crossStyle: {color: '#999'}
                    }
                },
                toolbox: {
                    feature: {
                        dataView: {show: true, readOnly: false},
                        magicType: {show: true, type: ['line', 'bar', 'stack']},
                        restore: {show: true},
                        saveAsImage: {show: true}
                    }
                },
                legend: {
                    data: %legend%,
                    top: 40
                },
                xAxis: {
                    data: %xAxis%
                },
                yAxis: %yAxis%,
                series: %series%
            }";

        /// <summary>渲染图表Js到客户端</summary>
        /// <param name="clientID">客户端控件ID</param>
        /// <param name="data">数据</param>
        /// <param name="xAxisName">图表x轴要展示的数据名称</param>
        /// <param name="series">图表系列。若为空则根据属性自动构建。</param>
        /// <param name="title">图表标题</param>
        /// <param name="yAxis">图标y轴定义</param>
        public static void Render(string clientID, IList data, string xAxisName, List<Serie> series = null, string title = "", List<YAxis> yAxis = null)
        {
            string option = GetOption(data, xAxisName, series, title, yAxis);
            RegistScript(clientID, option);
        }
        public static void Render(string clientID, DataTable data, string xAxisName, List<Serie> series = null, string title = "", List<YAxis> yAxis = null)
        {
            string option = GetOption(data, xAxisName, series, title, yAxis);
            RegistScript(clientID, option);
        }

        /// <summary>获取图表配置信息</summary>
        public static string GetOption(IList data, string xAxisName, List<Serie> series=null, string title="", List<YAxis> yAxis=null)
        {
            if (data.Count == 0) return "{}";
            if (series == null)  series = BuildSeries(data[0].GetType(), xAxisName);

            // 遍历数据，填充x轴值和系列值
            List<string> xAxis = new List<string>();
            foreach (var item in data)
            {
                xAxis.Add(item.GetPropertyValue(xAxisName).ToText());
                for (int i = 0; i < series.Count; i++)
                    series[i].Data.Add(item.GetPropertyValue(series[i].DataField).ToText());
            }

            return BuildOption(title, xAxis, yAxis, series);
        }

        /// <summary>获取图表配置信息</summary>
        public static string GetOption(DataTable data, string xAxisName, List<Serie> series=null, string title="", List<YAxis> yAxis=null)
        {
            if (data == null || data.Rows.Count == 0) return "{}";
            if (series == null)       series = BuildSeries(data, xAxisName);

            // 遍历数据，填充x轴值和系列值
            List<string> xAxis = new List<string>();
            foreach (DataRow item in data.Rows)
            {
                xAxis.Add(item[xAxisName].ToText());
                for (int i = 0; i < series.Count; i++)
                    series[i].Data.Add(item[series[i].DataField].ToText());
            }
            var option = BuildOption(title, xAxis, yAxis, series);
            return option;
        }

        //---------------------------------------------------
        // 根据表格列构建所有图表系列
        //---------------------------------------------------
        private static List<Serie> BuildSeries(DataTable data, string excludeField)
        {
            int i = 0;
            string[] symbols = Enum.GetNames(typeof(SerieSymbol));
            List<Serie> series = new List<Serie>();
            foreach (DataColumn item in data.Columns)
            {
                if (item.ColumnName != excludeField)
                {
                    i = i % symbols.Length;
                    SerieSymbol symbol = (SerieSymbol)Enum.Parse(typeof(SerieSymbol), symbols[i]);
                    i++;
                    series.Add(new Serie() { Name = item.ColumnName, DataField = item.ColumnName, Type = SerieType.Line, Symbol = symbol });
                }
            }
            return series;
        }

        // 根据对象属性构建所有图表系列
        private static List<Serie> BuildSeries(Type type, string excludeField)
        {
            int i = 0;
            string[] symbols = Enum.GetNames(typeof(SerieSymbol));
            List<Serie> series = new List<Serie>();
            foreach (var item in type.GetProperties())
            {
                if (item.Name != excludeField)
                {
                    i = i % symbols.Length;
                    SerieSymbol symbol = (SerieSymbol)Enum.Parse(typeof(SerieSymbol), symbols[i]);
                    i++;
                    series.Add(new Serie() { Name = item.Name, DataField = item.Name, Type = SerieType.Line, Symbol = symbol });
                }
            }
            return series;
        }

        //---------------------------------------------------
        // 构建脚本
        //---------------------------------------------------
        // 构建图表的 Option 字符串
        static string BuildOption(string title, List<string> xAxis, List<YAxis> yAxis, List<Serie> series)
        {
            string result;
            var legend = series.Select(t => t.Name).ToList();
            result = _template.Replace("%xAxis%", JsonConvert.SerializeObject(xAxis));
            result = result.Replace("%legend%", JsonConvert.SerializeObject(legend));
            result = result.Replace("%series%", JsonConvert.SerializeObject(series));
            result = result.Replace("%title%", JsonConvert.SerializeObject(title));
            if (yAxis == null || yAxis.Count == 0)
                result = result.Replace("%yAxis%", "{}");
            else
                result = result.Replace("%yAxis%", JsonConvert.SerializeObject(yAxis));
            return result;
        }

        // 注册生成图表的脚本到客户端
        private static void RegistScript(string clientID, string option, bool useFineUI=true)
        {
            if (useFineUI)
            {
                var script = string.Format("echarts.init(document.getElementById('{0}')).setOption({1});", clientID, option);
                PageContext.RegisterStartupScript(script);
            }
            else
            {
                var script = string.Format("echarts.init(document.getElementById('{0}')).setOption({1});", clientID, option);
                (HttpContext.Current.Handler as Page).ClientScript.RegisterStartupScript(null, "EChart", script, true);
            }
        }
    }
}