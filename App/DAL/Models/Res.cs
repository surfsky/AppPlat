using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using App.Components;
using EntityFramework.Extensions;

namespace App.DAL
{
    /// <summary>
    /// 资源表
    /// 可考虑加上缩小图片，视频截图等功能
    /// </summary>
    public class Res : EntityBase<Res>
    {
        [UI("键")]             public string Key { get; set; }
        [UI("文件名")]         public string Name { get; set; }
        [UI("描述")]           public string Description { get; set; }
        [UI("自定义数据")]     public string Tag { get; set; }
        [UI("顺序")]           public int    Seq { get; set; }
        [UI("文件Mime类型")]   public string MimeType { get; set; }
        [UI("是否是图片")]     public bool   IsImage { get; set; }
        [UI("文件大小")]       public long   Size { get; set; }
        [UI("文件相对路径")]   public string Path { get; set; }
        [UI("MD5值")]          public string MD5 { get; set; }
        [UI("文件上传时间")]   public DateTime? UploadDt { get; set; }
        [UI("访问次数")]       public int    VisitCnt { get; set; }


        [NotMapped, UI("完整URL路径", ReadOnly =true)]
        public string Url
        {
            get { return Asp.ResolveClientUrl(this.Path);}
        }

        [NotMapped, UI("物理路径", ReadOnly = true)]
        public string PhysicalPath
        {
            get { return HttpContext.Current.Server.MapPath(this.Path); }
        }

        // 构建资源键值
        public static string BuildKeyName(string prefix, int id)
        {
            return string.Format("{0}-{1}", prefix, id);
        }

        // 覆盖几个属性，避免递归循环
        [NotMapped]
        public new List<Res> Files
        {
            get {return new List<Res>();}
        }
        [NotMapped]
        public new List<Res> Images
        {
            get { return new List<Res>(); }
        }


        //-----------------------------------------------
        // 公共方法
        //-----------------------------------------------
        // 构造函数
        public Res()
        {
            this.UploadDt = DateTime.Now;
        }
        public Res(string key, string virtualPath)
        {
            string physicalPath = HttpContext.Current.Server.MapPath(virtualPath);
            FileInfo fi = new FileInfo(physicalPath);
            this.Key = key;
            this.Name = fi.Name;
            this.Size = fi.Length;
            this.Path = virtualPath;
            this.MimeType = GetMimeType(fi.Name);
            this.IsImage = this.MimeType.Contains("image");
            this.UploadDt = DateTime.Now;
            this.MD5 = EncryptionHelper.GetFileMD5(physicalPath);
            this.VisitCnt = 0;
        }


        // 获取文件mimetype
        public static string GetMimeType(string filePath)
        {
            if (filePath.IsNullOrEmpty()) return "";
            int n = filePath.LastIndexOf('.');
            if (n == -1) return "";
            string ext = filePath.Substring(n).ToLower();
            switch (ext)
            {
                case ".jpg": return "image/jpeg";
                case ".png": return "image/png";
                case ".gif": return "image/gif";
                case ".doc": return "application/msword";
                case ".xls": return "application/vnd.ms-excel";
                case ".ppt": return "application/vnd.ms-powerpoint";
                case ".exe": return "application/octet-stream";
                case ".pdf": return "application/pdf";
                default:     return "";
            }
        }

        // 删除资源文件
        public static void Delete(string key)
        {
            var items = Set.Where(t => t.Key == key).ToList();
            foreach (var item in items)
                File.Delete(item.PhysicalPath);
            Set.Where(t => t.Key == key).Delete();
        }

        // 批量删除资源文件
        public static void DeleteBatch(List<int> ids)
        {
            var items = Set.Where(t => ids.Contains(t.ID)).ToList();
            foreach (var item in items)
                File.Delete(item.PhysicalPath);
            Set.Where(t => ids.Contains(t.ID)).Delete();
        }

        // 批量删除对应键的资源文件
        public static void DeleteBatch(string prefix, List<int> ids)
        {
            foreach (int id in ids)
                Res.Delete(BuildKeyName(prefix, id));
        }

        // 批量删除对应键的资源文件
        public static void DeleteBatch(Type entityType, List<int> ids)
        {
            string prefix = entityType.Name;
            foreach (int id in ids)
                Res.Delete(BuildKeyName(prefix, id));
        }


        // 检索
        public static IQueryable<Res> Search(string key, bool? isImage = null)
        {
            var q = Set.Where(t => t.Key == key);
            if (isImage != null) q = q.Where(t => t.IsImage == isImage);
            return q;
        }

        // 新增
        public static void Add(string key, string virtualName)
        {
            Res res = new Res(key, virtualName);
            res.Save();
        }


    }
}