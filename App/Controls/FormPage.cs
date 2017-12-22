using App.Components;
using App.DAL;
using FineUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Controls
{
    /// <summary>
    /// 表单基类。实现实体的查看、编辑、新增逻辑。
    /// 输入参数：
    ///     id                      实体ID
    ///     mode = view/new/edit    查看/新建/编辑
    ///     create = true/false     若不存在时是否创建
    ///     showBtnClose            默认为false
    /// </summary>
    /// <example>
    /// protected void Page_Load(object sender, EventArgs e)
    /// {
    ///     this.InitForm(this.SimpleForm1, "CorePowerView", "CorePowerNew", "CorePowerEdit");
    ///     if (!IsPostBack)
    ///         ShowForm();
    /// }
    /// 重载三个虚方法：NewData(), ShowData(), CollectData()
    /// </example>
    public class FormPage<T> : PageBase, IDataForm<T>
        where T : EntityBase<T>
    {
        //---------------------------------------------
        // 成员
        //---------------------------------------------
        // 页面控件
        public Button btnClose;
        public Button btnSaveClose;
        public Button btnSaveNew;
        public ToolbarText lblInfo;
        public FormBase frm;

        // 几个按钮的显示控制
        public bool ShowBtnClose { get; set; } = false;
        public bool ShowBtnSave { get; set; } = true;
        public bool ShowBtnSaveNew { get; set; } = true;


        //---------------------------------------------
        // IDataForm 接口方法，请在子类中重载实现逻辑
        //---------------------------------------------
        /// <summary>新建数据时调用，可重载该方法清空表单</summary>
        public virtual void NewData() { }

        /// <summary>编辑数据时调用，可重载该方法显示数据</summary>
        public virtual void ShowData(T item) { }

        /// <summary>采集表单数据供保存时调用，可重载该方法从表单获取数据</summary>
        public virtual void CollectData(ref T item) { }


        /// <summary>数据</summary>
        public T Data
        {
            get
            {
                var id = Asp.GetQueryIntValue("id");
                if (id != null)
                    return GetData(id.Value);
                return null;
            }
        }

        //---------------------------------------------
        // 其它虚方法，可在子类中重载
        //---------------------------------------------
        /// <summary>获取数据，可重载该方法进行自定义获取方法</summary>
        public virtual T GetData(int id)
        {
            return AppContext.Current.Set<T>().Find(id);
        }

        /// <summary>保存数据前预处理。若为false则不进行后继存储操作。</summary>
        public virtual bool CheckData(T item)
        {
            return true;
        }

        /// <summary>新增或修改实体数据（可考虑加上T返回类型）</summary>
        public virtual void SaveData(T item)
        {
            item.Save();
        }

        //---------------------------------------------
        // 公有方法
        //---------------------------------------------
        /// <summary>
        /// 初始化表单。访问权限验证；生成工具栏按钮；请在OnInit事件中调用。
        /// </summary>
        /// <param name="form">页面中的表单</param>
        /// <param name="viewPower">查看权限</param>
        /// <param name="editPower">编辑权限</param>
        /// <param name="newPower">新建权限</param>
        /// <param name="toolbar">工具栏。按钮将在该工具栏中生成。若为空，则尝试在表单第一个工具栏中插入按钮。</param>
        /// <param name="relayoutToolbar">是否重新布局工具栏。为true的话将工具栏上的原控件移到右侧。</param>
        public void InitForm(FormBase form, PowerType viewPower, PowerType editPower, PowerType newPower, Toolbar toolbar = null, bool relayoutToolbar = true)
        {
            // 检测页面访问权限
            switch (this.Mode)
            {
                case PageMode.View: Common.CheckPagePower(viewPower); break;
                case PageMode.New: Common.CheckPagePower(newPower); break;
                case PageMode.Edit: Common.CheckPagePower(editPower); break;
            }

            // 工具栏
            this.frm = form;
            if (toolbar == null)
            {
                if (form.Toolbars.Count > 0)
                    toolbar = form.Toolbars[0];
                else
                {
                    toolbar = new Toolbar();
                    form.Toolbars.Add(toolbar);
                }
            }

            // 工具栏控件
            InitToolbar(toolbar, relayoutToolbar);
            this.ShowBtnClose = Asp.GetQueryBoolValue("showBtnClose") ?? false;
        }

        // 初始化工具栏控件
        // <f:Button runat="server" ID = "btnClose" Icon="SystemClose" EnablePostBack="false" Text="关闭" />
        // <f:Button runat="server" ID = "btnSaveClose" ValidateForms="SimpleForm1" Icon="SystemSaveClose" OnClick="btnSaveClose_Click" Text="保存后关闭" />
        // <f:Button runat="server" ID = "btnSaveNew" ValidateForms="SimpleForm1" Icon="SystemSaveNew" OnClick="btnSaveNew_Click" Text="保存并新增" />
        // <f:ToolbarText runat="server" />
        private void InitToolbar(Toolbar toolbar, bool relayoutToolbar)
        {
            // 信息标签
            lblInfo = new ToolbarText() { CssStyle = "color:red" };

            // 关闭按钮
            btnClose = new Button() { Icon = Icon.SystemClose, Text = "关闭", EnablePostBack = false };
            btnClose.OnClientClick = ActiveWindow.GetHidePostBackReference();

            // 保存并关闭按钮
            btnSaveClose = new Button() { Icon = Icon.SystemSaveClose, Text = "保存" };
            btnSaveClose.ValidateForms = new string[] { this.frm.ID };
            btnSaveClose.Click += (s, e) =>
            {
                if (Save())
                {
                    this.lblInfo.Text = string.Format("成功保存({0:HH:mm:ss})", DateTime.Now);
                    //PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                }
                else
                    this.lblInfo.Text = "保存失败";
            };

            // 保存并新增按钮
            btnSaveNew = new Button() { Icon = Icon.SystemSaveNew, Text = "保存并新增" };
            btnSaveNew.ValidateForms = new string[] { this.frm.ID };
            btnSaveNew.Click += (s, e) =>
            {
                if (Save())
                {
                    this.lblInfo.Text = string.Format("成功保存({0:HH:mm:ss})，新增中", DateTime.Now);
                    NewData();
                }
                else
                    this.lblInfo.Text = "保存失败";
            };

            // 添加到工具栏上
            if (relayoutToolbar)
                toolbar.Items.Insert(0, new ToolbarFill());
            toolbar.Items.Insert(0, btnSaveNew);
            toolbar.Items.Insert(0, btnSaveClose);
            toolbar.Items.Insert(0, btnClose);
            toolbar.Items.Add(lblInfo);
        }


        /// <summary>
        /// 显示表单，请在页面首次初始化代码中调用
        /// </summary>
        public void ShowForm()
        {
            // 新建
            var mode = this.Mode;
            if (mode == PageMode.New)
            {
                NewData();
                ShowButtons(true, true, true);
                return;
            }

            // 尝试获取实体
            var id = Asp.GetQueryIntValue("id");
            if (id == null)
                return;
            T item = GetData(id.Value);
            if (item == null)
            {
                Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }

            // 显示表单数据
            ShowData(item);

            // 查看或编辑
            if (mode == PageMode.View)
            {
                ShowButtons(true, false, false);
                FormHelper.SetFormEditable(this.frm, false);
            }
            if (mode == PageMode.Edit)
            {
                ShowButtons(true, true, false);
            }
        }

        // 显示按钮
        void ShowButtons(bool close, bool save, bool saveNew)
        {
            this.btnClose.Hidden = !(close && ShowBtnClose);
            this.btnSaveClose.Hidden = !(save && ShowBtnSave);
            this.btnSaveNew.Hidden = !(saveNew && ShowBtnSaveNew);
        }


        /// <summary>保存（含新增或修改逻辑）</summary>
        public virtual bool Save()
        {
            T item = (this.Mode == PageMode.New) ? AppContext.Current.Set<T>().Create() : GetData(Asp.GetQueryIntValue("id").Value);
            CollectData(ref item);
            if (CheckData(item))
            {
                SaveData(item);
                return true;
            }
            return false;
        }

    }
}