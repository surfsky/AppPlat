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
using System.Collections;

namespace App.Admins
{
    /// <summary>
    /// API接口清单
    /// </summary>
    [Auth(PowerType.Admin)]
    public partial class API : PageBase
    {
        //------------------------------------------
        // init
        //------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindGrid();
        }

        private void BindGrid()
        {
            ArrayList arr = new ArrayList
            {
                  new { Name = "App.DAL.DbUser", URL = "../HttpApi.App.DAL.DbUser.axd/api", Description = "用户相关数据接口" }
                 ,new { Name = "App.DAL.DbVerifyCode",  URL = "../HttpApi.App.DAL.DbVerifyCode.axd/api",  Description = "短信验证码接口" }
            };

            Grid1.DataSource = arr;
            Grid1.DataBind();
        }
    }
}
