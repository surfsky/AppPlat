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
    /// 数据编辑窗口（仅admin可访问，或快速建模时使用）
    /// DataForm.aspx?type=xxx&id=xxxx&mode=view
    /// </summary>
    [Auth(PowerType.Admin)]
    public partial class DataForm : PageBase
    {
        // 初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            string typeName = Request.QueryString["type"];
            this.SimpleForm1.EntityTypeName = Asp.GetQueryString("type");
            this.SimpleForm1.EntityID = Asp.GetQueryIntValue("id").Value;
            this.SimpleForm1.Mode = this.Mode;
            this.SimpleForm1.InitForm();
        }
    }
}
