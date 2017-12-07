using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUI;
using System.Linq;
using System.Data.Entity;
using EntityFramework.Extensions;
using App.DAL;
using App.Components;

namespace App.Admins
{
    [Auth(PowerType.UserView)]
    public partial class Users : PageBase
    {
        //--------------------------------------------------
        // Init
        //--------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Grid1.ViewUrlTmpl      = "~/admins/UserForm.aspx?mode=view&id={0}";
            this.Grid1.EditUrlTmpl      = "~/admins/UserForm.aspx?mode=edit&id={0}";
            this.Grid1.NewUrlTmpl       = "~/admins/UserForm.aspx?mode=new";
            this.Grid1.AllowNew         = Common.CheckPower(PowerType.UserEdit);
            this.Grid1.AllowDelete      = Common.CheckPower(PowerType.UserDelete);
            this.Grid1.AllowBatchDelete = Common.CheckPower(PowerType.UserDelete);
            this.Grid1.AllowEdit        = Common.CheckPower(PowerType.UserEdit);
            this.Grid1.InitGrid<User>(BindGrid);
            if (!IsPostBack)
            {
                InitSearchBar();
                UI.SetGridColumnByPower(Grid1, "changePasswordField", PowerType.UserChangePassword);
                this.Grid1.SetSortAndPage<User>(true, true, SiteConfig.PageSize, t=>t.Name);
                BindGrid();
            }
        }

        // 初始化检索工具栏
        void InitSearchBar()
        {
            UI.BindDDLTree(ddlDept, Dept.All, "--全部部门--", null, null);
            UI.BindDDL(ddlTitle, DAL.Title.GetAll(), "Name", "ID", "--全部职务--", null);
            UI.BindDDL(ddlRole, typeof(RoleType).ToList(), "Name", "ID", "--全部角色--", null);
            UI.BindDDLBool(ddlStatus, "启用", "禁用", "--全部状态--", null);
        }

        // 查询及绑定网格
        void BindGrid()
        {
            string name = tbName.Text.Trim();
            int? deptId = UI.GetDDLValue(ddlDept);
            int? titleId = UI.GetDDLValue(ddlTitle);
            RoleType? roleId = UI.GetDDLEnumValue(ddlRole, typeof(RoleType));
            bool? enable = ddlStatus.SelectedItemArray.Length == 0 ? null : new bool?(ddlStatus.SelectedValue == "enabled");
            bool includeAdmin = Common.LoginUser.Name == "admin";

            IQueryable<User> q = DAL.User.Search(name:name, role: roleId, inUsed: enable, deptId: deptId, titleId: titleId, includeAdmin: includeAdmin);
            Grid1.BindGrid(q);
        }



        //--------------------------------------------------
        // 工具栏
        //--------------------------------------------------
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.BindGrid();
        }


        //--------------------------------------------------
        // Grid
        //--------------------------------------------------
        // 行绑定事件（BUG: 若是admin则删除按钮无效，代码都对，但整列都变无效了）
        protected void Grid1_PreRowDataBound(object sender, FineUI.GridPreRowEventArgs e)
        {
            User user = e.DataItem as User;

            // 设置职务列
            var titleField = Grid1.FindColumn("Titles") as FineUI.BoundField;
            if (titleField != null)
            {
                string titles = "";
                foreach (var item in user.Titles)
                    titles += item.Name + ",";
                titles = titles.TrimEnd(',');
                titleField.DataFormatString = " " + titles;
            }

            // 设置角色列
            var roleField = Grid1.FindColumn("Roles") as FineUI.BoundField;
            if (roleField != null)
            {
                string roles = "";
                foreach (var item in user.RolesText)
                    roles += item.GetDescription() + ",";
                roles = roles.TrimEnd(',');
                roleField.DataFormatString = " " + roles;
            }
        }

        // 删除
        protected void Grid1_Delete(object sender, List<int> ids)
        {
            //DAL.User.DeleteUsersPhysical(ids);
            DAL.User.DeleteUsersLogic(ids);
        }

    }
}
