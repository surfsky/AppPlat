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

namespace App.Admins
{
    /// <summary>
    /// 日志查看窗口（只读）
    /// LogForm.aspx?id=xxxx&mode=view
    /// </summary>
    [Auth(PowerType.ReportView)]
    public partial class LogForm : PageBase
    {
        // 初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            string typeName = "App.DAL.Log"; // Request.QueryString["type"];
            this.SimpleForm1.EntityTypeName = typeName;
            this.SimpleForm1.EntityID = Asp.GetQueryIntValue("id").Value;
            this.SimpleForm1.Mode = PageMode.View;
            this.SimpleForm1.InitForm();
        }
    }
}
