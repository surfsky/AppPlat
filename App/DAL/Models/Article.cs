using App.Components;
using EntityFramework.Extensions;
using App.HttpApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace App.DAL
{
    /// <summary>
    /// 内容类别
    /// </summary>
    public enum ArticleType : int
    {
        [UI("普通")]  Normal = 0,
        [UI("新闻")]  News = 1,
        [UI("帮助")]  Help = 2
    }

    /// <summary>
    /// 文章。简单的内容管理系统，带图片。
    /// </summary>
    public class Article : EntityBase<Article>
    {
        [UI("类别")]                                 public ArticleType? Type { get; set; }
        [UI("标题")]                                 public string Title { get; set; }
        [UI("内容", Editor = EditorType.HtmlEditor)] public string Body { get; set; }
        [UI("作者")]                                 public string Author { get; set; }
        [UI("纯文本摘要")]                           public string Summary { get; set; }
        [UI("发表时间")]                             public DateTime PostDt { get; set; }
        [UI("查看次数")]                             public int?   VisitCnt { get; set; } = 0;


        //-----------------------------------------------
        // 公共方法
        //-----------------------------------------------
        // 设置摘要
        public void SetSummary()
        {
            string txt = this.Body.ClearTag();
            this.Summary = txt.IsNullOrEmpty() ? "" : txt.Substring(0, Math.Min(100, txt.Length));
        }

        // 新增新闻
        public static Article Add()
        {
            var news = new Article() { PostDt = DateTime.Now };
            news.Save();
            return news;
        }

        // 查询
        public static IQueryable<DAL.Article> Search(ArticleType? type, string author, string title, DateTime? startDt, DateTime? endDt)
        {
            IQueryable<DAL.Article> q = Set;
            if (type != null)                  q = q.Where(t => t.Type == type);
            if (!String.IsNullOrEmpty(author)) q = q.Where(t => t.Author.Contains(author));
            if (!String.IsNullOrEmpty(title))  q = q.Where(t => t.Title.Contains(title));
            if (startDt != null)               q = q.GreaterEqual(t => t.PostDt, startDt.Value);
            if (endDt != null)                 q = q.LessEqual(t => t.PostDt, endDt.Value);
            return q;
        }

        // 查询并分页
        public static List<Article> Search(ArticleType? type, string author, string title, DateTime? startDt, DateTime? endDt, int pageIndex = 0, int pageSize = 10)
        {
            var q = Search(type, author, title, startDt, endDt);
            q.SortAndPage("PostDt", "DESC", pageIndex, pageSize);
            return q.ToList();
        }

        // 
        public static List<Article> GetNews(int pageIndex, int pageSize = 10)
        {
            return Search(ArticleType.News, null, null, null, null, pageIndex, pageSize);
            //return Content.Set.Where(t => t.Type==ContentType.News).SortAndPage("PostDt", "DESC", pageIndex, pageSize).ToList();
        }
    }
}