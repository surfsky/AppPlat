using App.Components;
using EntityFramework.Extensions;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace App.DAL
{
    /// <summary>
    /// Model类变更后，自动升级数据库
    /// 参考 http://www.tuicool.com/articles/EfIvey
    /// </summary>
    internal sealed class AppMigrationConfiguration : DbMigrationsConfiguration<AppContext>
    {
        public AppMigrationConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "AppPlat";  // EF会根据这个key去检索_MigrationHistory表，来实现模型变更的数据库更新
        }

        // 每次数据库变更后调用
        protected override void Seed(AppContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );

            // 更新状态表数据
            // 将备注信息写入数据库
            //ResetEnumData();
            //context.WriteComments();
        }


        //------------------------------------------------------
        // 将枚举信息写到数据库
        //------------------------------------------------------
        /// <summary>重置非系统配置数据</summary>
        public static void ResetEnumData()
        {
            SiteConfig.ClearEnumData();
            SiteConfig.AddEnum(typeof(AccountLogType));
            SiteConfig.AddEnum(typeof(ArticleType));
            SiteConfig.AddEnum(typeof(ProductType));
            SiteConfig.AddEnum(typeof(OrderStatus));
            SiteConfig.AddEnum(typeof(OrderPayMode));
            SiteConfig.AddEnum(typeof(InviteSource));
            SiteConfig.AddEnum(typeof(InviteStatus));
            SiteConfig.AddEnum(typeof(RoleType));
            SiteConfig.AddEnum(typeof(PowerType));
            SiteConfig.AddEnum(typeof(LogLevel));
        }

        //------------------------------------------------------
        // 将备注信息写到数据库
        //------------------------------------------------------
        /// <summary>
        /// 将备注信息写到数据库表。
        /// 遍历AppContext中所有的DbSet属性，获取泛型元素类别，根据其UIAttribute获取备注信息，填写到数据库。
        /// 局限：
        /// （1）现阶段只支持sqlserver；
        /// （2）无法根据实体类的特性来获取正确的数据库表名、字段名；如[Table][Column]等。
        /// （3）估计要研究 DbModelBuilder 类，或好好看看EF的源码，看到底是怎么映射的。
        /// （4）日后完善
        /// </summary>
        public void WriteComments()
        {
            foreach (var prop in this.GetType().GetProperties())
            {
                Type type = prop.PropertyType;
                if (type.IsGenericType && type.Name.Contains("DbSet"))
                {
                    string table = prop.Name;
                    Type itemType = type.GenericTypeArguments[0]; // 获取泛型参数类
                    WriteComment(table, itemType);
                }
            }
        }

        /// <summary>
        /// 将实体类属性的备注写入数据库。
        /// </summary>
        void WriteComment(string tableName, Type type)
        {
            //DbModelBuilder builder = new DbModelBuilder();
            foreach (var prop in type.GetProperties())
            {
                var desc = prop.GetDescription();
                if (prop.CanWrite && !desc.IsNullOrEmpty())
                {
                    var schema = "dbo";
                    var table = tableName;
                    var column = prop.Name;
                    var sql1 = string.Format(@"
                        EXEC sys.sp_dropextendedproperty 
                            @name='MS_Description', 
                            @level0type=N'SCHEMA',@level0name=N'{0}', 
                            @level1type=N'TABLE', @level1name=N'{1}', 
                            @level2type=N'COLUMN',@level2name=N'{2}'
                        ", schema, table, column
                        );
                    var sql2 = string.Format(@"
                        EXEC sys.sp_addextendedproperty 
                            @name='MS_Description', @value=N'{0}', 
                            @level0type=N'SCHEMA',@level0name=N'{1}', 
                            @level1type=N'TABLE', @level1name=N'{2}', 
                            @level2type=N'COLUMN',@level2name=N'{3}'
                        ", desc, schema, table, column
                        );
                    try { AppContext.Current.Database.ExecuteSqlCommand(sql1); } catch { }
                    try { AppContext.Current.Database.ExecuteSqlCommand(sql2); } catch { }
                }
            }
        }
    }
}
