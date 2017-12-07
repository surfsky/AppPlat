using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUI;
using App.Controls;
using App.DAL;

namespace App.Fits
{
    /// <summary>
    /// TODO: 新用户 & 办卡一体化界面
    /// </summary>
    public partial class UserNew : FormPage<FitUserCard>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitForm(this.SimpleForm1, "FitView", "FitEdit", "FitEdit");
            if (!IsPostBack)
            {
                Common.BindDDL(this.ddlUser, FitCustomer.GetAll(), "User.Name", "UserID", null);
                Common.BindDDL(this.ddlCard, Product.GetAll(), "Name", "ID", null);
                Common.BindDDL(this.ddlCoach, FitCoach.GetAll(), "User.Name", "UserID", null);
                Common.BindRBLEnum(this.rblSts, typeof(FitUserCardStatus), null);
                ShowForm();
            }
        }

        protected override void NewData()
        {
            this.ddlUser.SelectedValue = "";
            this.ddlCard.SelectedValue = "";
            this.ddlCoach.SelectedValue = "";
            this.rblSts.SelectedValue = "";
            this.dpBuyDt.SelectedDate = DateTime.Now;
            this.dpStartDt.SelectedDate = null;
            this.dpExpireDt.SelectedDate = null;
            this.tbTotalCnt.Text = "0";
            this.tbUsedCnt.Text = "0";
            this.tbLeftCnt.Text = "0";

            // 传递进来userid参数
            int? userId = Common.GetQueryIntValue("userId");
            if (userId != null)
            {
                ddlUser.SelectedValue = userId.ToString();
                ddlUser.Enabled = false;
            }
        }

        protected override void ShowData(FitUserCard item)
        {
            this.ddlUser.SelectedValue = item.UserID.ToText();
            this.ddlCard.SelectedValue = item.CardID.ToText();
            this.ddlCoach.SelectedValue = item.CoachID.ToText();
            this.rblSts.SelectedValue = item.Sts.ToIntText();
            this.dpBuyDt.SelectedDate = item.BuyDt;
            this.dpStartDt.SelectedDate = item.StartDt;
            this.dpExpireDt.SelectedDate = item.ExpireDt;
            this.tbTotalCnt.Text = item.TotalCnt.ToText();
            this.tbUsedCnt.Text = item.UsedCnt.ToText();
            this.tbLeftCnt.Text = item.LeftCnt.ToText();
        }

        protected override void CollectData(ref FitUserCard item)
        {
            item.UserID = Common.GetDDLValue(this.ddlUser);
            item.CardID = Common.GetDDLValue(this.ddlCard);
            item.CoachID = Common.GetDDLValue(this.ddlCoach);
            item.Sts = Common.GetRBLEnumValue(rblSts, typeof(FitUserCardStatus));
            item.BuyDt = Common.GetDateTime(this.dpBuyDt);
            item.StartDt = Common.GetDateTime(this.dpStartDt);
            item.ExpireDt = Common.GetDateTime(this.dpExpireDt);
            item.TotalCnt = Common.GetTextBoxIntValue(this.tbTotalCnt);
            item.UsedCnt = Common.GetTextBoxIntValue(this.tbUsedCnt);
        }
    }
}
