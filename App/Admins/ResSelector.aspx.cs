using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUI;
using EntityFramework.Extensions;
using App.DAL;
using App.Components;
using System.Drawing;
using System.Drawing.Imaging;

namespace App.Admins
{
    /// <summary>
    /// TODO: 资源选择窗口（card方式）
    /// </summary>
    public partial class ResSelector : PageBase
    {
        // Init
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Grid1.AllowNew = false;
            this.Grid1.AllowEdit = false;
            this.Grid1.AllowDelete = false;
            this.Grid1.AllowBatchDelete = false;
            this.Grid1.InitGrid<Res>(BindGrid, Toolbar1);

            if (!IsPostBack)
            {
                this.Grid1.SetSortAndPage<Res>(true, true, SiteConfig.PageSize);
                BindGrid();
            }
        }

        // 绑定网格
        private void BindGrid()
        {
            string key = Request.QueryString["key"];
            IQueryable<Res> q = Res.Search(key);
            Grid1.BindGrid(q);
        }


        // 查找
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        // 删除事件
        protected void Grid1_Delete(object sender, List<int> ids)
        {
            Res.DeleteBatch(ids);
        }

        // 图片上传
        protected void filePhoto_FileSelected(object sender, EventArgs e)
        {
            // 保存目录和键
            string cate = Request.QueryString["cate"];
            string key = Request.QueryString["key"];

            // 准备目录
            string folder = string.Format("~/Files/{0}/", cate);
            string folderPhysical = Server.MapPath(folder);
            if (!System.IO.Directory.Exists(folderPhysical))
                System.IO.Directory.CreateDirectory(folderPhysical);

            // 上传图片并更新数据
            if (filePhoto.HasFile)
            {
                // 文件名
                string fileName = filePhoto.ShortFileName;
                fileName = fileName.Replace(":", "_").Replace(" ", "_").Replace("\\", "_").Replace("/", "_");
                fileName = string.Format("{0}_{1}", DateTime.Now.ToString("yyyyMMddHHmmssfffffff"), fileName);
                string physicalName = string.Format("{0}\\{1}", folderPhysical, fileName);
                string virtualName = string.Format("{0}/{1}", folder, fileName);

                // 上传新图片
                filePhoto.SaveAs(physicalName);
                DrawHelper.CreateThumbnail(physicalName, physicalName, 500);

                // 数据库记录
                Res.Add(key, virtualName);
                BindGrid();
            }
        }
    }
}
