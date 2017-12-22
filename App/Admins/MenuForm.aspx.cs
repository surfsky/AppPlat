using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUI;
using App.DAL;
using App.Controls;
using App.Components;

namespace App.Admins
{
    /// <summary>
    /// 菜单编辑页面
    /// </summary>
    public partial class MenuForm : FormPage<DAL.Menu>
    {
        //----------------------------------------------------
        // Init
        //----------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            InitForm(this.SimpleForm1, PowerType.MenuEdit, PowerType.MenuEdit, PowerType.MenuEdit);
            if (!IsPostBack)
                ShowForm();
        }

        // 获取
        public override DAL.Menu GetData(int id)
        {
            return DAL.Menu.GetDetail(id);
        }


        // 新建数据
        public override void NewData()
        {
            tbName.Text = "";
            tbUrl.Text = "";
            tbSeq.Text = "0";
            tbIcon.Text = "";
            tbRemark.Text = "";
            chkOpen.Checked = false;

            // 图标列表, 上级菜单，权限下拉框
            int? parentId = Asp.GetQueryIntValue("parentid");
            BindDDLMenu(parentId, null);
            BindDDLPower(null);
            BindIcons("");
        }

        // 展示数据
        public override void ShowData(DAL.Menu item)
        {
            tbName.Text = item.Name;
            tbUrl.Text = item.NavigateUrl;
            tbSeq.Text = item.Seq.ToString();
            tbIcon.Text = item.ImageUrl;
            tbRemark.Text = item.Remark;
            chkOpen.Checked = item.IsOpen;
            chkVisible.Checked = item.Visible;

            // 图标列表, 上级菜单，权限下拉框
            BindDDLMenu(item.Parent == null ? null : (int?)item.Parent.ID, item.ID);
            BindDDLPower(item.ViewPower == null ? null : (int?)item.ViewPower);
            BindIcons(item.ImageUrl);
        }

        // 收集数据
        public override void CollectData(ref DAL.Menu item)
        {
            item.Name = tbName.Text.Trim();
            item.NavigateUrl = tbUrl.Text.Trim();
            item.Seq = Convert.ToInt32(tbSeq.Text.Trim());
            item.ImageUrl = tbIcon.Text;
            item.Remark = tbRemark.Text.Trim();
            item.IsOpen = chkOpen.Checked;
            item.Visible = chkVisible.Checked;

            // 父菜单，访问权限
            item.Parent = null;
            item.ViewPower = null;
            if (ddlParentMenu.SelectedIndex != -1)
            {
                int parentID = Convert.ToInt32(ddlParentMenu.SelectedValue);
                item.Parent = DAL.Menu.Get(parentID);
            }
            if (ddlViewPower.SelectedIndex != -1)
            {
                var powerId = Convert.ToInt32(ddlViewPower.SelectedValue);
                item.ViewPower = (PowerType)(powerId);
            }
        }

        public override void SaveData(DAL.Menu item)
        {
            item.Save();
            DAL.Menu.Reload();
            Common.RefreshLoginUser();
        }

        //----------------------------------------------------
        // 辅助方法
        //----------------------------------------------------
        // 菜单图标列表
        public void BindIcons(string selectImageUrl="")
        {
            FineUI.RadioButtonList iconList = this.iconList;
            iconList.Items.Clear();
            string[] icons = new string[] { "tag_yellow", "tag_red", "tag_purple", "tag_pink", "tag_orange", "tag_green", "tag_blue", "folder", "page" };
            foreach (string icon in icons)
            {
                string value = String.Format("~/res/icon/{0}.png", icon);
                string text = String.Format("<img style=\"vertical-align:bottom;\" src=\"{0}\" />&nbsp;{1}", ResolveUrl(value), icon);
                iconList.Items.Add(new RadioItem(text, value));
            }
            if (!string.IsNullOrEmpty(selectImageUrl))
                iconList.SelectedValue = selectImageUrl;
        }

        // 绑定到下拉列表（启用模拟树功能和不可选择项功能）
        private void BindDDLMenu(int? selectedId = null, int? disableId = null)
        {
            UI.BindDDLTree(ddlParentMenu, DAL.Menu.All, "--根目录--", selectedId, disableId);
        }

        // 显示权限下拉框(根据当前用户拥有的权限来设置)
        private void BindDDLPower(int? selectedId = null)
        {
            var items = new List<object>();
            foreach (var item in Common.LoginUser.Powers)
            {
                var info = item.GetEnumInfo();
                items.Add(new {ID=info.ID, Name=string.Format("{0}（{1}-{2}）", info.Value, info.Group, info.Name) });
            }
            UI.BindDDL(ddlViewPower, items, "Name", "ID", "--请选择权限--", selectedId);
        }
    }
}
