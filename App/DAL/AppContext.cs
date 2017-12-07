using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Diagnostics;
using System.Reflection;
using App.Components;

namespace App.DAL
{
    /// <summary>
    /// EntityFramework 数据库上下文
    /// </summary>
    public class AppContext : DbContext
    {
        //------------------------------------------------------
        // 实体集合
        //------------------------------------------------------
        // 基础库表
        public DbSet<Config>     Configs { get; set; }
        public DbSet<Area>       Areas { get; set; }
        public DbSet<Dept>       Depts { get; set; }
        public DbSet<User>       Users { get; set; }
        public DbSet<RolePower>  RolePowers { get; set; }
        public DbSet<Title>      Titles { get; set; }
        public DbSet<Online>     Onlines { get; set; }
        public DbSet<Log>        Logs { get; set; }
        public DbSet<Menu>       Menus { get; set; }
        public DbSet<Res>        Res { get; set; }
        public DbSet<Article>    Articles { get; set; }
        public DbSet<Invite>     Invites { get; set; }
        public DbSet<VerifyCode> VerifyCodes { get; set; }
        public DbSet<Message>    Message { get; set; }

        // 财务相关
        public DbSet<Account>    Accounts { get; set; }
        public DbSet<AccountLog> AccountLogs { get; set; }


        // 报表相关（供参考）
        public DbSet<RptGDP>          RptGDPs { get; set; }

        // 商店相关
        public DbSet<Store>           Stores { get; set; }
        public DbSet<Product>         Products { get; set; }
        public DbSet<Order>           Orders { get; set; }
        public DbSet<OrderItem>       OrderItems { get; set; }


        //------------------------------------------------------
        // 当前数据库上下文
        //------------------------------------------------------
        /// <summary>
        /// 当前数据库上下文。
        /// （1）若为HTTP环境，则使用HttpContext来保存AppContext实例，针对每个请求创建一个。
        /// （2）若不是HTTP环境，动态创建一个，循环使用。
        /// </summary>
        private static AppContext _context;
        public static AppContext Current
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    if (_context == null) _context = new AppContext();
                    return _context;
                }
                else
                    return Asp.GetContextData("__DbContext", () => new AppContext()) as AppContext;
            }
        }

        /// <summary>
        /// 释放数据库上下文
        /// </summary>
        public static void Release()
        {
            if (HttpContext.Current == null)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
            else
                HttpContext.Current.Items["__DbContext"] = null;
        }


        //------------------------------------------------------
        // 成员方法
        //------------------------------------------------------
        // 构造函数。数据库连接字符串为db；打印数据库操作SQL到console。
        public AppContext() : base("db")
        {
            this.Database.Log = (t) => Trace.WriteLine(t);
        }


        // 创建数据库时进行设置
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 模型变更会自动去修改数据库
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppContext, AppMigrationConfiguration>());  // 自动更新数据库
            //Database.SetInitializer<AppContext>(new AppDatabaseInitializer()); // 模型一变更就重建数据库

            // TODO: 表间的关联关系（以下代码可考虑废除，直接在表内增加XXXXID外键字段即可）
            // Title <-> User
            modelBuilder.Entity<Title>()
                .HasMany(t => t.Users)
                .WithMany(u => u.Titles)
                .Map(x => x.ToTable("TitleUsers")
                .MapLeftKey("TitleID")
                .MapRightKey("UserID"));

            // User <-> Role
            /*
            modelBuilder.Entity<User>()
                .HasMany(r => r.Roles)
                .Map(x => x.ToTable("UserRoles")
                .MapLeftKey("UserID")
                .MapRightKey("RoleID"));
            */
        }

        
    }
}