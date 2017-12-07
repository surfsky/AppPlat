using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using App.HttpApi;
using EntityFramework;
using EntityFramework.Extensions;
using App.Components;
using App;
using Newtonsoft.Json;

namespace App.DAL
{
    /// <summary>
    /// 基础用户表（登录+通用信息+会员信息）
    /// </summary>
    public class User : EntityBase<User>, ILogicDelete
    {
        //------------------------------------------------------
        // 属性
        //------------------------------------------------------
        // 通用属性
        [UI("是否在用")]                         public bool InUsed { get; set; } = true;
        [UI("账户名（唯一）"), StringLength(50)] public string Name { get; set; }
        [UI("EMail")]                            public string Email { get; set; }
        [UI("密码（已加密）")]                   public string Password { get; set; }
        [UI("性别"), StringLength(10)]           public string Gender { get; set; }
        [UI("昵称"), StringLength(120)]          public string NickName { get; set; }
        [UI("实名"), StringLength(100)]          public string RealName { get; set; }
        [UI("头像")]                             public string Photo { get; set; }
        [UI("脸部照片")]                         public string Face { get; set; }
        [UI("QQ"), StringLength(50)]             public string QQ { get; set; }
        [UI("微信"), StringLength(50)]           public string Wechat { get; set; }
        [UI("电话"), StringLength(50)]           public string Phone { get; set; }
        [UI("手机（唯一）"), StringLength(50)]   public string Mobile { get; set; }
        [UI("地址"), StringLength(500)]          public string Address { get; set; }
        [UI("身份证"), StringLength(50)]         public string IdentityCard { get; set; }
        [UI("特长")]                             public string Specialty { get; set; }
        [UI("备注")]                             public string Remark { get; set; }
        [UI("生日")]                             public DateTime? Birthday { get; set; }
        [UI("创建日期")]                         public DateTime? CreateDt { get; set; }
        [UI("最后登录日期")]                     public DateTime? LastLoginDt { get; set; }
        [UI("用户拥有的角色")]                   public string RolesText { get; set; }="";        // 格式如：" 1, 2, 4,"
        [UI("微信OpenID")]                       public string WechatOpenId { get; set; }


        // 员工信息
        [UI("就职日期")]                         public DateTime? TakeOfficeDt { get; set; }
        [UI("部门")]                             public int? DeptID { get; set; }

        // 导航属性
        public virtual Dept Dept { get; set; }
        public virtual List<Title> Titles { get; set; }

        //
        [NotMapped, JsonIgnore]
        public string MobileMask {
            get
            {
                int len = Mobile.ToText().Length;
                if (len < 7) return "";
                else return Mobile.Substring(0, 3) + "****" + Mobile.Substring(len - 4, 4);
            }
        }

        [NotMapped, UI("年龄")]
        public int Age
        {
            get
            {
                if (Birthday != null)
                {
                    DateTime now = DateTime.Today;
                    int age = now.Year - Birthday.Value.Year;
                    if (now.AddYears(-age) < Birthday)
                        age--;
                    return age;
                }
                else
                    return 0;
            }
        }

        // 用户拥有的角色
        [NotMapped]
        public List<RoleType> Roles
        {
            get
            {
                var roles = new List<RoleType>();
                if (!RolesText.IsNullOrEmpty())
                    foreach (var txt in RolesText.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        roles.Add((RoleType)(Convert.ToInt32(txt)));
                return roles;
            }
            set
            {
                string txt = "";
                foreach (var role in value)
                    txt += string.Format(" {0},", (int)role);
                this.RolesText = txt;
            }
        }


        // 用户拥有的权限
        private List<PowerType> _powers;
        [NotMapped]
        public List<PowerType> Powers
        {
            set { _powers = value; }
            get
            {
                if (_powers == null)
                    _powers = GetUserPowers(this);
                return _powers;
            }
        }

        // 用户可访问的菜单
        private List<Menu> _menus;
        [NotMapped]
        public List<Menu> Menus
        {
            set { _menus = value; }
            get
            {
                if (_menus == null)
                    _menus = GetAllowMenus(this.Powers);
                return _menus;
            }
        }


        //------------------------------------------------------
        // 查找相关
        //------------------------------------------------------
        // 搜索用户
        public static IQueryable<User> Search(
            string name = "", string mobile = "",
            RoleType? role = null, bool? inUsed = null,
            int? birthMonth = null, int? deptId = null, int? titleId = null, 
            bool includeAdmin = false)
        {
            var r = role==null ? null : GetSearchRoleText(role.Value);
            IQueryable<User> q = Set.Include(u => u.Dept).Include(u => u.Titles);
            if (!includeAdmin)                                               q = q.Where(u => u.Name != "admin");
            if (!String.IsNullOrEmpty(name))                                 q = q.Where(u => u.Name.Contains(name) || u.RealName.Contains(name));
            if (deptId != null)                                              q = q.Where(u => u.Dept.ID == deptId);
            if (titleId != null)                                             q = q.Where(u => u.Titles.Any(t => t.ID == titleId));
            if (role != null)                                                q = q.Where(u => u.RolesText.Contains(r));
            if (inUsed != null)                                              q = q.Where(u => u.InUsed == inUsed.Value);
            if (!mobile.IsNullOrEmpty())                                     q = q.Where(u => u.Mobile == mobile);
            if (birthMonth != null)                                          q = q.Where(u => u.Birthday.Value.Month == birthMonth);
            return q;
        }

        /// <summary>查找雇员</summary>
        public static IQueryable<User> SearchEmployee(string name = "", string mobile = "", RoleType? role = null, bool? inUsed = null, int? deptId = null, int? titleId = null, int? birthMonth = null)
        {
            var r = GetSearchRoleText(RoleType.Customer);
            IQueryable<User> q = Search(name:name, mobile:mobile, role: role, inUsed: inUsed, birthMonth: birthMonth, deptId: deptId, titleId: titleId);
            q = q.Where(t => t.Name != "admin");
            q = q.Where(t => !t.RolesText.Contains(r));  // 非客户都算员工
            return q;
        }

        /// <summary>查找所有顾客</summary>
        public static IQueryable<User> GetCustomers()
        {
            return SearchRole(RoleType.Customer);
        }

        public static IQueryable<User> GetSalesmans()
        {
            return SearchRole(RoleType.Salesman);
        }

        public static IQueryable<User> SearchRole(RoleType role)
        {
            var r = GetSearchRoleText(role);
            IQueryable<User> q = Set.Where(t => t.InUsed == true)
                .Where(t => t.RolesText.Contains(r))
                .Where(t => t.InUsed == true)
                .OrderBy(t => t.NickName)
                ;
            return q;
        }

        /// <summary>构造检索角色字符串（前面一个空格，后面一个逗号）</summary>
        static string GetSearchRoleText(RoleType roleType)
        {
            return string.Format(" {0},", (int)roleType);
        }

        /// <summary>获取用户</summary>
        public static User Get(int? id=null, string name = "", string mobile = "", string openId = "")
        {
            if (id == null && name.IsNullOrEmpty() && mobile.IsNullOrEmpty() && openId.IsNullOrEmpty())
                return null;

            IQueryable<User> q = Set;
            if (id != null)                       q = q.Where(t => t.ID == id);
            if (!name.IsNullOrEmpty())            q = q.Where(t => t.Name == name);
            if (!openId.IsNullOrEmpty())          q = q.Where(t => t.WechatOpenId == openId);
            if (!mobile.IsNullOrEmpty())          q = q.Where(t => t.Mobile == mobile);
            return q.FirstOrDefault();
        }


        /// <summary>获取用户详情</summary>
        public static User GetDetail(int? id=null, string name = "", string mobile = "", string openId = "")
        {
            if (id == null && name.IsNullOrEmpty() && mobile.IsNullOrEmpty() && openId.IsNullOrEmpty())
                return null;

            var q = Set.Include(t => t.Dept).Include(t => t.Titles);
            if (id != null)                       q = q.Where(t => t.ID == id);
            if (!name.IsNullOrEmpty())            q = q.Where(t => t.Name == name);
            if (!openId.IsNullOrEmpty())          q = q.Where(t => t.WechatOpenId == openId);
            if (!mobile.IsNullOrEmpty())          q = q.Where(t => t.Mobile == mobile);
            return q.FirstOrDefault();
        }

        /// <summary>虚拟删除用户（打个不在用标志。）</summary>
        public static void DeleteUsersLogic(List<int> ids)
        {
            Set.Where(t => ids.Contains(t.ID)).Where(t => t.Name != "admin").ToList()
                .ForEach(t => t.InUsed = false);
            Db.SaveChanges();
        }

        //------------------------------------------------------
        // 菜单
        //------------------------------------------------------
        // 用户 & 菜单
        private static List<DAL.Menu> GetAllowMenus(List<PowerType> powers)
        {
            var menus = new List<DAL.Menu>();
            foreach (var menu in DAL.Menu.All)
            {
                if (menu != null)
                    if (menu.ViewPower == null || powers.Contains(menu.ViewPower.Value))
                        menus.Add(menu);
            }
            return menus;
        }

        //------------------------------------------------------
        // 角色、权限
        //------------------------------------------------------
        /// <summary>所有的角色</summary>
        public static List<EnumInfo> AllRoles
        {
            get { return typeof(RoleType).ToList(); }
        }

        /// <summary>所有的权限</summary>
        public static List<EnumInfo> AllPowers
        {
            get { return typeof(PowerType).ToList(); }
        }

        /// <summary>是否拥有某个角色</summary>
        public bool HasRole(RoleType role)
        {
            return this.Roles.Contains(role);
        }

        /// <summary>设置角色拥有的权限列表</summary>
        public static void SetRolePowers(RoleType role, List<PowerType> powers)
        {
            RolePower.Set.Where(t => t.Role == role).Delete();
            foreach (var power in powers)
            {
                var item = new RolePower() { Role = role, Power = power };
                RolePower.Set.Add(item);
            }
            RolePower.Db.SaveChanges();
        }

        // 获取用户角色id列表
        public static List<RoleType> GetUserRoles(int userID)
        {
            var user = User.GetDetail(userID);
            return user.Roles;
        }



        // 获取用户权限（admin拥有所有权限、普通用户根据角色来获取权限）
        public static List<PowerType> GetUserPowers(User user)
        {
            var powers = new List<PowerType>();
            if (user.Name == "admin")
                powers = typeof(PowerType).GetEnums<PowerType>();
            else
            {
                var roles = user.Roles;
                RolePower.Set.Where(t => roles.Contains(t.Role)).ToList().ForEach(t => powers.Add(t.Power));
            }
            return powers;
        }


        //------------------------------------------------------
        // 其它
        //------------------------------------------------------
        // 设置用户密码（不比较旧密码）
        public static void SetPassword(User user, string password)
        {
            if (user != null)
            {
                user.Password = PasswordHelper.CreateDbPassword(password);
                Db.SaveChanges();
            }
        }


    }
}