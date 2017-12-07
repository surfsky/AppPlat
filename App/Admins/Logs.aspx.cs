using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUI;
using EntityFramework.Extensions;
using App.DAL;
using App.Components;

namespace App.Admins
{
    [Auth(PowerType.LogEdit)]
    public partial class Logs : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Grid1.ViewUrlTmpl = "LogForm.aspx?id={0}&mode=view";
            Grid1.ShowViewField = true;
            Grid1.InitGrid<Log>(BindGrid);
            if (!IsPostBack)
            {
                UI.BindDDLEnum(this.ddlSearchLevel, typeof(LogLevel), "--日志级别--");
                UI.SetButtonByPower(btnDelete, PowerType.LogEdit);
                this.Grid1.SetSortAndPage<Log>(true, true, SiteConfig.PageSize, t=> t.LogDt, false);
                BindGrid();
            }
        }

        //-------------------------------------------------
        // Grid
        //-------------------------------------------------
        private void BindGrid()
        {
            string user = tbOperator.Text.Trim();
            string msg = tbxMessage.Text.Trim();
            LogLevel? level = UI.GetDDLEnumValue(this.ddlSearchLevel, typeof(LogLevel));
            DateTime? fromDt = null;
            if (ddlSearchRange.SelectedItemArray.Length > 0)
            {
                DateTime today = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                switch (ddlSearchRange.SelectedValue)
                {
                    case "TODAY":      fromDt = today; break;
                    case "LASTWEEK":   fromDt = today.AddDays(-7); break;
                    case "LASTMONTH":  fromDt = today.AddMonths(-1); break;
                    case "LASTYEAR":   fromDt = today.AddYears(-1); break;
                    default: break;
                }
            }

            IQueryable<Log> q = Log.Search(user, msg, level, fromDt);
            Grid1.BindGrid(q);
        }


        // 行预绑定事件（显示图标列）
        protected void Grid1_PreRowDataBound(object sender, GridPreRowEventArgs e)
        {
            var log = e.DataItem as Log;
            var fld = Grid1.FindColumn("Icon") as FineUI.ImageField;
            fld.DataImageUrlFormatString = GetIconUrl(log.Lvl);
        }

        // 获取日志级别对应的图标
        string GetIconUrl(LogLevel? logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug: return string.Format("~/Res/Icon/{0}.png", FineUI.Icon.Bug);
                case LogLevel.Info:  return string.Format("~/Res/Icon/{0}.png", FineUI.Icon.Comment);
                case LogLevel.Warn:  return string.Format("~/Res/Icon/{0}.png", FineUI.Icon.Information);
                case LogLevel.Error: return string.Format("~/Res/Icon/{0}.png", FineUI.Icon.Exclamation);
                case LogLevel.Fatal: return string.Format("~/Res/Icon/{0}.png", FineUI.Icon.Decline);
                default: return string.Format("~/Res/Icon/{0}.png", FineUI.Icon.Information);
            }
        }

        //-------------------------------------------------
        // 工具栏
        //-------------------------------------------------
        // 查找
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        // 批量删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int n = Log.DeleteBatch();
            BindGrid();
            Alert.ShowInTop(string.Format("成功删除{0}条记录", n));
        }

    }
}
