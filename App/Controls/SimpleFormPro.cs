using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FineUI;
using System.Web.UI;
using App.DAL;
using App.Components;
using System.Reflection;

namespace App.Controls
{
    /// <summary>
    /// 优化的SimpleForm，可以自动生成实体表单。
    /// </summary>
    public class SimpleFormPro : FineUI.SimpleForm
    {
        // 属性
        public string EntityTypeName { get; set; }         // 实体类型名称
        public int    EntityID { get; set; }               // 实体id
        public bool   ShowCloseButton { get; set; } = true;// 是否显示关闭按钮
        public bool   ShowIdLabel { get; set; } = true;    // 是否显示ID标签行
        public PageMode Mode { get; set; }                 // 模式

        // 事件
        public event EventHandler<string> PreSave;         // 保存前事件

        // 私有
        protected FineUI.Label  lblId;                     // ID
        protected FineUI.Button btnClose;                  // 关闭按钮
        protected FineUI.Button btnSaveClose;              // 保存后关闭按钮
        protected FineUI.Button btnSaveNew;                // 保存并新增按钮
        protected Dictionary<string, FineUI.Field> map;    // 字段控件映射字典
        protected Type entityType;                         // 实体类型


        /*
        <f:SimpleForm ID = "SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px"  Title="SimpleForm" LabelWidth="200">
            <Items>
                <f:Label runat = "server" ID ="lblId" Label="ID" Hidden="false" />
            </Items>
        </f:SimpleForm>
        */
        // Init
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.BoxFlex = 1;
            this.ShowBorder = false;
            this.ShowHeader = false;
            this.BodyPadding = "10px";
            this.LabelWidth = 200;
        }


        //-----------------------------------------------
        // 页面事件
        //-----------------------------------------------
        // 初始化
        public void InitForm()
        {
            if (string.IsNullOrEmpty(EntityTypeName))
                return;
            this.entityType = Assembly.GetExecutingAssembly().GetType(EntityTypeName);

            // 构建表单
            InitToolbar();
            InitItemID();
            this.map = FormHelper.BuildForm(this, new UISetting(this.entityType), false, this.Mode==PageMode.View);
            if (this.Mode == PageMode.View)
            {
                this.btnSaveClose.Visible = false;
                this.btnSaveNew.Visible = false;
                FormHelper.SetFormEditable(this, false);
            }

            // 初始化
            if (!(HttpContext.Current.Handler as Page).IsPostBack)
            {
                this.lblId.Text = this.EntityID.ToString();
                this.btnSaveNew.Visible = (this.EntityID == -1);
                if (this.EntityID != -1)
                    ShowData(this.EntityID);
            }
        }

        /*
        <Toolbars>
            <f:Toolbar ID = "Toolbar1" runat="server">
                <Items>
                    <f:Button runat = "server" ID="btnClose" Icon="SystemClose" Text="关闭" OnClick="btnClose_Click"  />
                    <f:ToolbarSeparator runat = "server" ID="ToolbarSeparator2" />
                    <f:Button runat = "server" ID="btnSaveClose" ValidateForms="SimpleForm1" Icon="SystemSaveClose" OnClick="btnSaveClose_Click" Text="保存并关闭" />
                    <f:Button runat = "server" ID="btnSaveNew" ValidateForms="SimpleForm1" Icon="SystemSaveNew" OnClick="btnSaveNew_Click" Text="保存并新增" />
                </Items>
            </f:Toolbar>
        </Toolbars>
        */
        void InitToolbar()
        {
            // 关闭按钮
            btnClose = new Button() { Icon = Icon.SystemClose, Text = "关闭" };
            btnClose.Click += (s, e) =>
            {
                PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            };

            // 保存后关闭按钮
            btnSaveClose = new Button() { Icon = Icon.SystemSaveClose, Text = "保存后关闭", ValidateForms = new string[] { this.ID } };
            btnSaveClose.Click += (s, e) =>
            {
                SaveData();
                PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            };

            // 保存并新增按钮
            btnSaveNew = new Button() { Icon = Icon.SystemSaveClose, Text = "保存并新增", ValidateForms=new string[] { this.ID} };
            btnSaveNew.Click += (s, e) =>
            {
                SaveData();
                ClearData();
            };

            // 添加到第一个工具栏左侧
            if (this.Toolbars.Count == 0)
                this.Toolbars.Add(new Toolbar());
            var toolbar = this.Toolbars[0];
            toolbar.Items.Insert(0, new ToolbarSeparator());
            toolbar.Items.Insert(0, btnSaveNew);
            toolbar.Items.Insert(0, btnSaveClose);
            if (ShowCloseButton)
                toolbar.Items.Insert(0, btnClose);
        }

        /*
        <Items>
            <f:Label runat = "server" ID ="lblId" Label="ID" Hidden="false" />
        </Items>
        */
        void InitItemID()
        {
            lblId = new Label() { Label = "ID", Hidden= !ShowIdLabel };
            this.Items.Add(lblId);
        }



        //-----------------------------------------------
        // 数据清空、展示、采集、保存
        //-----------------------------------------------
        // 清空数据
        void ClearData()
        {
            this.lblId.Text = "-1";
            foreach (var key in this.map.Keys)
            {
                FormHelper.SetEditorValue(map[key], "");
            }
        }

        // 加载数据
        void ShowData(int id)
        {
            object o = AppContext.Current.Set(entityType).Find(id);
            FormHelper.ShowFormData(this.map, o);
        }


        // 保存数据
        void SaveData()
        {
            int id = int.Parse(this.lblId.Text);
            if (id == -1)
            {
                // 新增
                var item = AppContext.Current.Set(entityType).Create();
                FormHelper.CollectData(this.map, ref item);
                if (PreSave != null) PreSave(item, "New");
                AppContext.Current.Set(entityType).Add(item);
                AppContext.Current.SaveChanges();
                this.lblId.Text = item.GetPropertyValue("ID").ToString();
            }
            else
            {
                // 更新
                var item = AppContext.Current.Set(entityType).Find(id);
                FormHelper.CollectData(this.map, ref item);
                if (PreSave != null) PreSave(item, "Update");
                AppContext.Current.SaveChanges();
            }
        }
    }
}