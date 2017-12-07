using System;
using System.Web;
using System.Web.Security;
using FineUI;
using System.Text;
using System.Linq;
using App;
using App.DAL;
using App.Components;

namespace App
{
    public partial class Default : PageBase
    {
        protected System.Web.UI.HtmlControls.HtmlForm form1;
        protected System.Web.UI.WebControls.Label lblTitle;
        protected System.Web.UI.WebControls.Label lblDomain;
        protected System.Web.UI.WebControls.Label lblICPNumber;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.lblTitle.Text = SiteConfig.SiteTitle;
                this.lblDomain.Text = SiteConfig.SiteDomain;
                this.lblICPNumber.Text = SiteConfig.SiteICP;
                
                if (User.Identity.IsAuthenticated)
                    Response.Redirect(FormsAuthentication.DefaultUrl);
            }
        }
    }
}
