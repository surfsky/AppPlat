using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;



namespace App.Reports
{
    public partial class RDLCReport : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }


        public void Display(string fileName, DataTable datasource, string width, string height)
        {
            if (!string.IsNullOrEmpty(width))
                this.ReportViewer1.Width = Unit.Parse(width);
            if (!string.IsNullOrEmpty(height))
                this.ReportViewer1.Height = Unit.Parse(height);

            ReportDataSource ds = new ReportDataSource("DataSet1", datasource);
            this.ReportViewer1.LocalReport.ReportPath = fileName;
            this.ReportViewer1.LocalReport.DataSources.Clear();
            this.ReportViewer1.LocalReport.DataSources.Add(ds);
            this.ReportViewer1.LocalReport.Refresh();
        }
    }
}