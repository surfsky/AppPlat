using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.Controls.MdEditors
{
    /// <summary>
    /// 传统 asp.net 页面方式操作 MdEditor，没问题
    /// </summary>
    public partial class TestMdEditor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSet_Click(object sender, EventArgs e)
        {
            this.edt.Text = "hello " + DateTime.Now.ToString();
        }

        protected void btnGet_Click(object sender, EventArgs e)
        {
            this.lblInfo.Text = this.edt.Text;
        }
    }
}