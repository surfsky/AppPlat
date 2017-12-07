using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace App.Reports
{
    public partial class Report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string reportFile = Server.MapPath("~/Reports/RDLC/Cross_2x2.rdlc");
                DataTable dt = CreateDemoData();
                this.report1.Display(reportFile, dt, "95%", null); 
            }
        }

        DataTable CreateDemoData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Column1", typeof(string));
            dt.Columns.Add("Column2", typeof(string));
            dt.Columns.Add("Column3", typeof(string));
            dt.Columns.Add("Column4", typeof(string));
            dt.Columns.Add("Value", typeof(int));
            dt.Rows.Add("a", "a1", "d", "d1", 100);
            dt.Rows.Add("a", "a1", "d", "d2", 230);
            dt.Rows.Add("a", "a2", "d", "d3", 10);
            dt.Rows.Add("a", "a2", "d", "d4", 11);
            dt.Rows.Add("b", "b1", "c", "c1", 113);
            dt.Rows.Add("b", "b2", "c", "c2", 13);
            dt.Rows.Add("b", "b3", "c", "c3", 110);
            dt.Rows.Add("b", "b4", "c", "c4", 155);
            return dt;
        }
    }
}