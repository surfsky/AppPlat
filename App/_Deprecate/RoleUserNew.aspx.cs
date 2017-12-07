using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUI;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using App.DAL;
using App.Components;

namespace App.Admin
{
    [ViewPower("CoreRoleUserNew")]
    public partial class RoleUserNew : PageBase
    {
        //----------------------------------------------------
        // Init
        //----------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Grid1.PageSize = DbConfig.PageSize;
            this.Grid1.InitGrid<User>((Action)BindGrid, null, "Name");
            if (!IsPostBack)
            {
                btnClose.OnClientClick = ActiveWindow.GetHideReference();
                int id = Common.GetQueryIntValue("id");
                Role current = Common.Db.Roles.Find(id);
                if (current == null)
                {
                    Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                    return;
                }
                Grid1.BindGrid<User>();
            }
        }

        // 查询
        void BindGrid()
        {
            IQueryable<User> q = Common.Db.Users;
            string searchText = ttbSearchMessage.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
                q = q.Where(u => u.Name.Contains(searchText) || u.RealName.Contains(searchText));
            q = q.Where(u => u.Name != "admin");

            // 排除已经属于本角色的用户
            int roleID = Common.GetQueryIntValue("id");
            q = q.Where(u => u.Roles.All(r => r.ID != roleID));
            Grid1.SortAndPage(q);
        }


        // 查找
        protected void ttbSearchMessage_TriggerClick(object sender, string e)
        {
            BindGrid();
        }

        // 保存并关闭
        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            int roleID = Common.GetQueryIntValue("id");
            Role role = Common.Db.Roles.Include(r => r.Users)
                .Where(r => r.ID == roleID)
                .FirstOrDefault()
                ;

            // 拥有该角色的用户
            List<int> ids = GridHelper.GetSelectedRowKeyIDs(Grid1);
            foreach (int userID in ids)
            {
                User user = EFHelper.GetAttach<User>(userID);
                role.Users.Add(user);
            }

            // 保存
            Common.Db.SaveChanges();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
    }
}
