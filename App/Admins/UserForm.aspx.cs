using System;
using System.Data.Entity;
using System.Linq;
using FineUI;
using App.DAL;
using App.Components;
using App.Controls;
using System.Collections;
using System.Collections.Generic;
using App;
using System.Web;

namespace App.Admins
{
    /// <summary>
    /// 基础用户信息编辑窗口
    /// </summary>
    [Auth(PowerType.UserView)]
    public partial class UserForm : FormPage<User>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitForm(this.SimpleForm1, PowerType.UserView, PowerType.UserEdit, PowerType.UserEdit, toolbar1);
            if (!IsPostBack)
            {
                var roles = Common.LoginUser.Name == "admin" ? DAL.User.AllRoles : Common.LoginUser.Roles.GetEnumInfos();
                UI.BindDDLTree(ddlDept, DAL.Dept.All, "--选择部门--", null, null);
                UI.BindCBL(cblTitle, DAL.Title.GetAll(), "Name", "ID", null);
                UI.BindCBL(cblRole, roles, "Name", "ID", null);
                UI.SetUploaderText(uploader, SiteConfig.SizeFaceImage);
                ShowForm();
            }
        }


        //----------------------------------------------------
        // 重载方法
        //----------------------------------------------------
        // 获取实体数据（要取关联表数据）
        public override User GetData(int id)
        {
            return DAL.User.GetDetail(id);
        }

        // 新建
        public override void NewData()
        {
            this.tbName.Text = "";
            this.tbRealName.Text = "";
            this.tbNickName.Text = "";
            this.tbEmail.Text = "";
            this.tbMobile.Text = "";
            this.tbPhone.Text = "";
            this.tbQQ.Text = "";
            this.tbWechat.Text = "";
            this.tbRemark.Text = "";
            this.cbEnabled.Checked = true;
            this.tbName.Readonly = false;
            this.imgPhoto.ImageUrl = SiteConfig.DefaultUserImage;
            this.ddlDept.SelectedValue = "";
            this.lblRegistDt.Text = "";
            this.tbIdCard.Text = "";
            this.dpBirthday.Text = "";
            this.tbSpecialty.Text = "";
            this.cblTitle.SelectedIndexArray = new int[]{};
        }

        // 编辑
        public override void ShowData(User item)
        {
            // 编辑权限控制
            if (item.Name == "admin" && AuthHelper.GetIdentityName() != "admin")
            {
                Alert.Show("你无权查看和编辑超级管理员！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }
            if (this.Mode == PageMode.View)
                this.uploader.Hidden = true;

            // 实体数据
            this.tbName.Text = item.Name;
            this.tbRealName.Text = item.RealName;
            this.tbNickName.Text = item.NickName;
            this.tbEmail.Text = item.Email;
            this.tbMobile.Text = item.Mobile;
            this.tbPhone.Text = item.Phone;
            this.tbQQ.Text = item.QQ;
            this.tbWechat.Text = item.Wechat;
            this.tbRemark.Text = item.Remark;
            this.cbEnabled.Checked = item.InUsed;
            this.ddlGender.SelectedValue = item.Gender;
            this.tbName.Readonly = true;
            this.imgPhoto.ImageUrl = item.Photo.IsNullOrEmpty() ? SiteConfig.DefaultUserImage : item.Photo;
            this.lblRegistDt.Text = item.CreateDt.ToString();
            this.uploader.ButtonIconUrl = item.Photo.IsNullOrEmpty() ? SiteConfig.DefaultUserImage : item.Photo;
            this.tbIdCard.Text = item.IdentityCard.ToText();
            this.dpBirthday.SelectedDate = item.Birthday;
            this.tbSpecialty.Text = item.Specialty;


            // 部门、职务、角色
            int?     deptId = item.DeptID;
            string[] titles = (item.Titles == null) ? new string[] { } : item.Titles.Select(t => t.ID.ToString()).ToArray();
            string[] roles  = item.Roles.Select(t => ((int)t).ToString()).ToArray();
            this.ddlDept.SelectedValue = deptId.ToString();
            this.cblTitle.SelectedValueArray = titles;
            this.cblRole.SelectedValueArray = roles;
        }

        // 采集表单数据
        public override void CollectData(ref User item)
        {
            item.Name = tbName.Text.Trim();
            item.RealName = tbRealName.Text.Trim();
            item.NickName = tbNickName.Text.Trim();
            item.Gender = ddlGender.SelectedValue;
            item.Email = tbEmail.Text.Trim();
            item.Mobile = tbMobile.Text.Trim();
            item.Phone = tbPhone.Text.Trim();
            item.QQ = tbQQ.Text.Trim();
            item.Wechat = tbWechat.Text.Trim();
            item.Remark = tbRemark.Text.Trim();
            item.InUsed = cbEnabled.Checked;
            item.Photo = imgPhoto.ImageUrl;
            item.CreateDt = DateTime.Now;
            item.IdentityCard = this.tbIdCard.Text;
            item.Birthday = this.dpBirthday.SelectedDate;
            item.Specialty = this.tbSpecialty.Text;

            // 如果是新用户，设置个默认密码
            if (Mode == PageMode.New)
                item.Password = PasswordHelper.CreateDbPassword(SiteConfig.DefaultPassword);

            // 部门、角色、职务
            item.DeptID = UI.GetDDLValue(this.ddlDept);
            item.Titles = DAL.Title.GetTitles(UI.GetCBLIntValue(this.cblTitle).ToList());
            item.Roles = UI.GetCBLValue(this.cblRole).CastEnum<RoleType>();
        }


        // 保存数据前判断(若为false则阻止保存)
        public override bool CheckData(User item)
        {   
            if (this.Mode == PageMode.New)
            {
                User user = DAL.User.Get(name:item.Name);
                if (user != null)
                {
                    Alert.Show("用户名 " + item.Name + " 已被注册使用。");
                    return false;
                }
                user = DAL.User.Get(mobile:item.Mobile);
                if (user != null)
                {
                    Alert.Show("手机号 " + item.Mobile + " 已被注册使用，请更换手机号。");
                    return false;
                }
            }
            return true;
        }

        // 图片上传
        protected void uploader_FileSelected(object sender, EventArgs e)
        {
            string imageUrl = UI.UploadFile(uploader, "Users", SiteConfig.SizeFaceImage);
            UI.SetImage(this.imgPhoto, imageUrl, true);
        }

    }
}
