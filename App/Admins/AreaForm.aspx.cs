using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Entity;
using FineUI;
using App.DAL;
using App.Components;
using App.Controls;

namespace App.Admins
{
    /// <summary>
    /// 编辑页面
    /// </summary>
    public partial class AreaForm : FormPage<Area>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitForm(this.SimpleForm1, PowerType.AreaView, PowerType.AreaEdit, PowerType.AreaEdit);
            if (!IsPostBack)
                ShowForm();
        }


        //-----------------------------------------------
        // 数据清空、展示、采集、保存
        //-----------------------------------------------
        // 获取
        public override Area GetData(int id)
        {
            return Area.GetDetail(id);
        }


        // 清空数据
        public override void NewData()
        {
            this.lblId.Text = "-1";
            this.tbName.Text = "";
            this.tbSeq.Text = "0";
            this.tbRemark.Text = "";
            int? parentId = Asp.GetQueryIntValue("parentid");
            BindDDL(parentId, parentId);
        }

        // 加载数据
        public override void ShowData(Area item)
        {
            this.lblId.Text = item.ID.ToString();
            this.tbName.Text = item.Name;
            this.tbSeq.Text = item.Seq.ToString();
            this.tbRemark.Text = item.Remark;
            int? parentId = item.Parent == null ? null : (int?)item.Parent.ID;
            BindDDL(parentId, item.ID);
        }

        // 采集数据
        public override void CollectData(ref Area item)
        {
            Area parent = null;
            if (ddlParent.SelectedIndex != -1)
            {
                int n = int.Parse(this.ddlParent.SelectedValue);
                parent = Area.Get(n);
            }
            item.Name = this.tbName.Text;
            item.Seq = int.Parse(this.tbSeq.Text);
            item.Parent = parent;
            item.Remark = this.tbRemark.Text;
        }

        // 保存完毕后刷新部门数据
        protected override void AfterSaveData(Area item)
        {
            Area.Reload();
        }

        // 绑定下拉列表（启用模拟树功能和不可选择项功能）
        private void BindDDL(int? selectId=null, int? disableId=null)
        {
            UI.BindDDLTree(ddlParent, Area.All, "--根区域--", selectId, disableId);
        }
    }
}
