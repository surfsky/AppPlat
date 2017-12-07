using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUI;
using App.DAL;
using App;
using System.Reflection;
using System.Collections;
using App.Components;

namespace App.Reports
{
    /// <summary>
    /// 报表数据查看窗口（只读）
    /// ReportDataView.aspx?type=xxx&id=xxxx&readonly=true
    /// </summary>
    [Auth(PowerType.ReportView)]
    public partial class ReportDataView : PageBase
    {
        //-----------------------------------------------
        // 页面事件
        //-----------------------------------------------
        // 初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            // 限制下本页面的权限，只能编辑报表实体
            string typeName = Request.QueryString["type"];
            if (typeName == null)
                return;
            if (!typeName.StartsWith("App.DAL.Rpt"))
                return;

            //
            this.SimpleForm1.EntityTypeName = typeName;
            this.SimpleForm1.EntityID = Asp.GetQueryIntValue("id").Value;
            this.SimpleForm1.InitForm();
        }
    }
}
