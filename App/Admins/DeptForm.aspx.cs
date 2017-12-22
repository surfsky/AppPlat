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
    /// 部门编辑页面
    /// </summary>
    public partial class DeptForm : FormPage<Dept>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitForm(this.SimpleForm1, PowerType.DeptView, PowerType.DeptEdit, PowerType.DeptEdit);
            if (!IsPostBack)
                ShowForm();
        }


        //-----------------------------------------------
        // 数据清空、展示、采集、保存
        //-----------------------------------------------
        // 获取
        public override Dept GetData(int id)
        {
            return Dept.GetDetail(id);
        }


        // 清空数据
        public override void NewData()
        {
            this.lblId.Text = "-1";
            this.tbName.Text = "";
            this.tbSeq.Text = "0";
            this.tbRemark.Text = "";
            int? parentId = Asp.GetQueryIntValue("parentid");
            BindDDLDept(parentId, parentId);
        }

        // 加载数据
        public override void ShowData(Dept item)
        {
            this.lblId.Text = item.ID.ToString();
            this.tbName.Text = item.Name;
            this.tbSeq.Text = item.Seq.ToString();
            this.tbRemark.Text = item.Remark;
            int? parentId = item.Parent == null ? null : (int?)item.Parent.ID;
            BindDDLDept(parentId, item.ID);
        }

        // 采集数据
        public override void CollectData(ref Dept item)
        {
            Dept parent = null;
            if (ddlParent.SelectedIndex != -1)
            {
                int n = int.Parse(this.ddlParent.SelectedValue);
                parent = Dept.Get(n);
            }
            item.Name = this.tbName.Text;
            item.Seq = int.Parse(this.tbSeq.Text);
            item.Parent = parent;
            item.Remark = this.tbRemark.Text;
        }

        // 保存完毕后刷新部门数据
        public override void SaveData(Dept item)
        {
            item.Save();
            Dept.Reload();
        }

        // 绑定下拉列表（启用模拟树功能和不可选择项功能）
        private void BindDDLDept(int? selectId=null, int? disableId=null)
        {
            UI.BindDDLTree(ddlParent, Dept.All, "--根部门--", selectId, disableId);
        }
    }
}
