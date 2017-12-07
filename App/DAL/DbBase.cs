using App.Components;
using EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using Newtonsoft.Json;
using System.Collections;
using System.Text;

namespace App.DAL
{
    /// <summary>
    /// 数据操作基类（用纯静态方法实现）。
    /// 实现了数据访问的一些基础CRUD操作。
    /// 请用子类继承之，并实现扩展逻辑，如Search(), GetDetail(), DeleteBatch(), DeleteRecursive()
    /// </summary>
    /// <example>
    /// var user = User.Get(5);
    /// user.Age = 20;
    /// user.Save();
    /// </example>
    public class DbBase<T> : IID
        where T : class, IID
    {
        /// <summary>
        /// ID字段。如要自定义数据库字段名，请重载并加上[Column("XXXID")]
        /// </summary>
        [UI("ID"), Key] public virtual int ID { get; set;}


        //---------------------------------------------
        // 附属资源
        //---------------------------------------------
        // 资源键值
        [NotMapped, UI("资源键", ShowInForm=false, ShowInGrid=false, ShowInDetail=false)]
        public string ResKey
        {
            get { return Res.BuildKeyName(this.GetType().Name, this.ID);}
        }

        // 资源
        [NotMapped, UI("附件", ShowInForm=false, ShowInGrid = false, ShowInDetail = false)]
        public List<Res> Files
        {
            get
            {
                string key = this.ResKey;
                return Res.Set.Where(t => t.Key == key).OrderBy(t => t.Seq).ToList();
            }
        }
        [NotMapped, UI("附带的图片", ShowInForm=false, ShowInGrid = false, ShowInDetail = false)]
        public List<Res> Images
        {
            get
            {
                string key = this.ResKey;
                return Res.Set.Where(t => t.Key == key && t.IsImage).OrderBy(t => t.Seq).ToList();
            }
        }

        /// <summary>删除附属资源</summary>
        public void DeleteRes()
        {
            Res.Delete(this.ResKey);
        }

        /// <summary>删除附属资源</summary>
        public static void DeleteRes(Type entityType, int entityId)
        {
            string resKey = Res.BuildKeyName(entityType.Name, entityId);
            Res.Delete(resKey);
        }


        //---------------------------------------------
        // 属性
        //---------------------------------------------
        /// <summary>数据上下文。默认采用Common.Db。如需定制，请调用SetDb(),ReleaseDb()方法设置和取消。</summary>
        public static DbContext Db 
        {
            get { return AppContext.Current;}
        }


        /// <summary>获取数据集</summary>
        public static DbSet<T> Set
        {
            get { return Db.Set<T>(); }
        }


        //---------------------------------------------
        // 成员方法
        //---------------------------------------------
        /// <summary>删除</summary>
        public void Delete(bool log=true)
        {
            DeleteRes();
            Log(log, "删除", this.ID, typeof(T), this);
            Set.Where(t => t.ID == this.ID).Delete();
        }

        /// <summary>保存修改</summary>
        public void Save(bool log=true)
        {
            Db.Entry(this).State = EntityState.Modified;
            Db.SaveChanges();
            Log(log, "更新", this.ID, typeof(T), this);
        }

        /// <summary>保存新增数据</summary>
        public void SaveNew(bool log=true)
        {
            Set.Add(this as T);
            Db.SaveChanges();
            Log(log, "新增", this.ID, typeof(T), this);
        }

        /// <summary>设置状态为已修改</summary>
        public void SetModified()
        {
            Db.Entry(this).State = EntityState.Modified;
        }

        //---------------------------------------------
        // 静态方法
        //---------------------------------------------
        /// <summary>获取所有</summary>
        public static IQueryable<T> GetAll()
        {
            return Set;
        }

        /// <summary>获取所有（并排序）</summary>
        /// <remarks>(2017-11-06)无法合并这两个方法, 否则使用时会报错：无法从用法中推断出方法“XXX”的类型参数。请尝试显式指定类型参数。</remarks>
        public static IQueryable<T> GetAll<TKey>(Expression<Func<T, TKey>> keySelector, bool ascend = true)
        {
            return Set.SortBy(keySelector, ascend);
        }

        /// <summary>获取（根据ID）</summary>
        public static T Get(int id)
        {
            return Set.Find(id);
        }

        /// <summary>删除（如要删除对应的附属资源，请填写类型名参数）</summary>
        public static void Delete(int id, Type entityType=null, bool log=true)
        {
            if (entityType != null)
                DeleteRes(entityType, id);
            Set.Where(t => t.ID == id).Delete();
            Log(log, "删除", id, entityType, null);
        }

        /// <summary>批量删除（如要删除对应的附属资源，请填写类型名参数）</summary>
        public static void DeleteBatch(List<int> ids, Type entityType=null, bool log=true)
        {
            if (entityType != null)
                ids.ForEach(id => DeleteRes(entityType, id));
            Set.Where(t => ids.Contains(t.ID)).Delete();
            Log(log, "删除", ids.ToCommaString(), entityType, null);
        }

        /// <summary>查找一批指定ID的数据</summary>
        public static IQueryable<T> Search(List<int> ids)
        {
            return Set.Where(t => ids.Contains(t.ID));
        }

        //---------------------------------------------
        // 几个虚拟方法，如有需要请在子类实现。
        // 经测试，静态方法不支持virtual，所以在子类实现时请加个 new 关键字
        //---------------------------------------------
        /// <summary>获取详情（包括关联表信息）</summary>
        public static T GetDetail(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>逻辑删除</summary>
        public static void DeleteLogic(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>递归删除（包括子表数据）</summary>
        public static void DeleteRecursive(int id)
        {
            throw new NotImplementedException();
        }


        //---------------------------------------------
        // 日志
        //---------------------------------------------
        static void Log(bool log, string action, int id, Type type, object data)
        {
            Log(log, action, id.ToString(), type, data);
        }
        static void Log(bool log, string action, string id, Type type, object data)
        {
            if (!log) return;
            string json = Jsonlizer.ToJson(data, 20, true, true);  // 序列化为json，跳过复杂的属性
            string txt = string.Format("{0}数据，ID={1}, Type={2}, Data={3}", action, id, type, json);
            Logger.LogToDb(txt);
        }

        
    }
}