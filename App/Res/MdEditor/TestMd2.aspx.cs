using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestFineUI.Plugins.MdEditor
{
    public partial class TestMd2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                this.edtArea.InnerText = "## hello world";
        }

        protected void btnSet_Click(object sender, EventArgs e)
        {
            this.edtArea.InnerText = "## This is title";
        }

        protected void btnGet_Click(object sender, EventArgs e)
        {
            Response.Write(edtArea.InnerText);
        }
    }
}