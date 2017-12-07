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
    /// 教练
    /// 使用最简单的PageBase改造，不要用FormPage，还要跳转，相当麻烦
    /// 需增加一个链接按钮，跳到教授的课程上。
    /// </summary>
    public partial class CoachForm : FormPage<FitCoach>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitForm(this.SimpleForm1, "FitView", "FitEdit", "FitEdit");
            this.ShowBtnClose = false;
            this.ShowBtnSaveNew = false;
            if (!IsPostBack)
            {
                CreateIfNeed();
                ShowForm();
            }
        }

        // 若有必要创建并重定向
        void CreateIfNeed()
        {
            int userId = Common.GetQueryIntValue("userID");
            if (userId == -1) return;
            FitCoach item = FitCoach.GetByUserId(userId);
            if (item == null)
                item = FitCoach.Add(userId);
            Response.Redirect(string.Format("CoachForm.aspx?id={0}&mode={1}", item.ID, this.Mode));
        }

        // 新建
        protected override void NewData()
        {
            tbClasses.Text = "健身";
            tbPositiveCnt.Text = "0";
            tbMiddleCnt.Text = "0";
            tbNegativeCnt.Text = "0";
            tbPositiveRate.Text = "1.0";
            tbTeachPrice.Text = "0.0";
            tbPositiveRate.Enabled = false;
        }

        // 显示实体
        protected override void ShowData(FitCoach item)
        {
            tbClasses.Text = item.Classes.ToText();
            tbPositiveCnt.Text = item.PositiveCnt.ToText();
            tbMiddleCnt.Text = item.MiddleCnt.ToText();
            tbNegativeCnt.Text = item.NegativeCnt.ToText();
            tbPositiveRate.Text = item.PositiveRate.ToText();
            tbTeachPrice.Text = item.TeachPrice.ToText();
            tbPositiveRate.Enabled = false;
        }

        // 收集表单数据
        protected override void CollectData(ref FitCoach item)
        {
            item.Classes = tbClasses.Text;
            item.PositiveCnt = int.Parse(tbPositiveCnt.Text);
            item.MiddleCnt = int.Parse(tbMiddleCnt.Text);
            item.NegativeCnt = int.Parse(tbNegativeCnt.Text);

            int cnt = item.PositiveCnt + item.MiddleCnt + item.NegativeCnt;
            item.PositiveRate = cnt == 0 ? 0.0 : (item.PositiveCnt*1.0/cnt);
            item.TeachPrice = float.Parse(tbTeachPrice.Text);

            tbPositiveRate.Text = item.PositiveRate.ToText();
        }

        // 保存
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int userId = Common.GetQueryIntValue("userID");
            FitCoach item = FitCoach.GetByUserId(userId);
            CollectData(ref item);
            item.Save();
        }
    }
}
