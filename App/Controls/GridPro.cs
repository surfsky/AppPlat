using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using App.Components;
using App.DAL;
using EntityFramework.Extensions;
using FineUI;
using System.Linq.Expressions;


namespace App.Controls
{
    /// <summary>
    /// Grid 增强版
    /// - 包含分页、排序、删除、批量删除、导出Excel等逻辑
    /// - 可自动生成序号、查看、删除、编辑、ID列
    /// - 可根据实体属性自动生成列
    /// - 自动删除逻辑需要类型实现 IID 接口
    /// - 自动保持选中行
    /// - 内置弹窗控件，用于展示详情、修改、新增等页面
    /// - 排序和分页: 请调用 SetSortAndPage() 方法
    /// - 导出Excel: 请实现ExportExcel事件，可调用Grid1.ExportExcel()方法。
    /// </summary>
    /// <example>
    ///    &lt;f:GridPro ID="Grid1" runat="server"
    ///            AutoCreateFields="false"
    ///            NewUrlTmpl="~UserNew.aspx"
    ///            EditUrlTmpl="UserEdit.aspx?id={0}"
    ///            ViewUrlTmpl="UserView.aspx?id={0}"
    ///            ShowNumberField="true" ShowViewField="true" ShowDeleteField="true" ShowEditField="true" ShowIDField="false"
    ///            AllowNew="true" AllowEdit="true" AllowDelete="true" AllowBatchDelete="true" AllowExport="true"
    ///            RelayoutToolbar="false"
    ///            OnDelete="Grid1_Delete"
    ///    &gl;
    ///    Grid1.InitGrid<User>(BindGrid, Toolbar1);
    ///    Grid2.SetSortAndPage(....);
    /// </example>
    /// <info>
    /// - 由于设计器不支持泛型控件，所以本控件使用InitGrid&lt;T&gl;方法实现泛型
    /// - InitGrid()方法请在PageInit方法内调用，因为要动态生成一堆控件；
    /// </info>
    /// <target>
    /// - 将ID名称独立出来，不写死
    /// </target>
    /// <history>
    /// 2017-02-01 初始版本
    /// 2017-11-13 简化InitGrid方法，提取SetSortAndPage方法，使用强类型指定排序字段
    /// </history>
    public class GridPro: FineUI.Grid
    {
        //-------------------------------------------------
        // 属性和事件
        //-------------------------------------------------
        // 公共属性
        public string NewText { get; set; } = "新增";
        public string DeleteText { get; set; } = "删除";
        public string ExportText { get; set; } = "导出";
        public string NewUrlTmpl { get; set; }
        public string EditUrlTmpl { get; set; }
        public string ViewUrlTmpl { get; set; }
        public Window Win { get { return _window; } }
        public string WinID { get; set; } = "Window1";
        public int WinWidth { get; set; } = 700;
        public int WinHeight { get; set; } = 600;
        public CloseAction WinCloseAction { get; set; } = CloseAction.HidePostBack;
        public bool AutoCreateFields { get; set; } = false;
        public bool ShowNumberField { get; set; } = false;
        public bool ShowIDField { get; set; } = false;
        public bool ShowViewField { get; set; } = false;
        public bool ShowEditField { get; set; } = false;
        public bool ShowDeleteField { get; set; } = false;
        public bool AllowNew { get; set; } = false;
        public bool AllowEdit { get; set; } = false;
        public bool AllowDelete { get; set; } = false;
        public bool AllowBatchDelete { get; set; } = false;
        public bool AllowExport { get; set; } = false;
        public bool RelayoutToolbar { get; set; } = true;

        // 公开事件
        public event EventHandler WindowClose;
        public event EventHandler<List<int>> Delete;
        public event EventHandler Export;

        // 私有
        protected HiddenField _hidden;   // 隐藏控件，存储选中行ID列表
        protected FineUI.Window _window; // 弹窗控件

        // 以下三个按钮请在 InitGrid() 方法后调用
        public Button ExportButton { get; set; }
        public Button BatchDeleteButton { get; set; }
        public Button NewButton { get; set; }


        //-------------------------------------------------
        // 初始化
        //-------------------------------------------------
        // Init
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.BoxFlex = 1;
            this.ShowBorder = false;
            this.ShowHeader = false;
            this.DataKeyNames = new string[] { "ID" };
            this._hidden = new HiddenField();
        }

        /// <summary>
        /// 初始化网格(及相关附属控件)，请在每次页面初始化时都调用。
        /// </summary>
        /// <param name="toolbar">按钮将在该工具栏上生成。若为空则设置为网格的第一个工具栏。</param>
        public void InitGrid<T>(Action bindGrid, Toolbar toolbar = null)
            where T : class, IID
        {
            this._bindAction = bindGrid;
            if (AllowBatchDelete)
            {
                this.EnableCheckBoxSelect = true;
                this.EnableMultiSelect = true;
            }

            // 动态生成控件
            CreateWindow();
            CreatePager();
            CreateColumns<T>();
            CreateToolbarButtons<T>(toolbar);
            SetDeleteCommand<T>();
            SetSortPageEvent();
        }

        /// <summary>设置排序和分页逻辑</summary>
        /// <remarks>
        /// AllowSorting="true" SortField="Month" SortDirection="ASC" OnSort="Grid1_Sort"  
        /// AllowPaging="true" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange"
        /// </remarks>
        public void SetSortAndPage<T>(bool allowSort = true, bool allowPage = true, int pageSize = 30, Expression<Func<T, object>> fieldSelector=null, bool ascend=true)
        {
            string sortField = (fieldSelector==null) ? "ID" : App.Components.ReflectionHelper.GetPropertyName<T>(fieldSelector);
            SetSortAndPage(allowSort, allowPage, pageSize, sortField, ascend);
        }


        /// <summary>设置排序和分页逻辑（建议使用强类型版本）</summary>
        /// <remarks>
        /// AllowSorting="true" SortField="Month" SortDirection="ASC" OnSort="Grid1_Sort"  
        /// AllowPaging="true" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange"
        /// </remarks>
        private void SetSortAndPage(bool allowSort = true, bool allowPage = true, int pageSize = 30, string sortField = "ID", bool ascend = true)
        {
            string sortDirection = ascend ? "ASC" : "Desc";
            if (allowPage) allowSort = true;  // 允许分页的话必须允许排序，否则SortAndPage()方法会报错
            this.AllowSorting = allowSort;
            this.SortField = sortField;
            this.SortDirection = sortDirection;
            this.AllowPaging = allowPage;
            this.IsDatabasePaging = true; // 只允许数据库分页
            this.PageSize = pageSize;
            this._ddlGridPageSize.Hidden = !allowPage;
            this._ddlGridPageSize.SelectedValue = PageSize.ToString();
        }

        void SetSortPageEvent()
        {
            this.Sort += (s, e) =>
            {
                this.SortDirection = e.SortDirection;
                this.SortField = e.SortField;
                ShowData();
            };
            this.PageIndexChange += (s, e) =>
            {
                this.PageIndex = e.NewPageIndex;
                ShowData();
            };
            _ddlGridPageSize.SelectedIndexChanged += (s, e) =>
            {
                this.PageSize = Convert.ToInt32(_ddlGridPageSize.SelectedValue);
                if (this.PageSize != 0)
                    ShowData();
            };
        }


        //-------------------------------------------------
        // 网格绑定及数据查询事件
        //-------------------------------------------------
        protected Action _bindAction;
        protected void ShowData()
        {
            GridHelper.SaveSelectedIds(this, _hidden);
            if (_bindAction != null)
                _bindAction();
            GridHelper.ShowSelectedIds(this, _hidden);
        }
        public void BindGrid<T>(IQueryable<T> q)
        {
            GridHelper.BindGrid(this, q);
        }

        //-------------------------------------------------
        // Window
        //-------------------------------------------------
        /*
        <f:Window ID = "Window1" runat="server" IsModal="true" Hidden="true" Target="Top" 
            Width="700px" Height="600px" 
            EnableResize="true" EnableMaximize="true" EnableClose="false"
            EnableIFrame="true" IFrameUrl="about:blank" 
            OnClose="Window1_Close"
            />
        */
        protected void CreateWindow()
        {
            _window = new FineUI.Window()
            {
                ID = this.WinID,
                IsModal = true,
                Hidden = true,
                Target = FineUI.Target.Top,
                Width = this.WinWidth,
                Height = this.WinHeight,
                EnableResize = true,
                EnableMaximize = true,
                EnableClose = true,
                EnableIFrame = true,
                IFrameUrl = "about:blank",
                CloseAction = this.WinCloseAction
            };
            _window.Close += (s, e) => 
            {
                if (WindowClose != null)
                    WindowClose(this, null);
                else
                    ShowData();
            };
            this.Controls.Add(_window);
        }

        //-------------------------------------------------
        // 工具栏按钮
        //-------------------------------------------------
        /*
        <Toolbars>
            <f:Toolbar ID = "Toolbar1" runat="server">
                <Items>
                    <f:Button runat="server" ID="btnNew"            Icon="Add"       Text="新增"  />
                    <f:Button runat="server" ID="btnDeleteSelected" Icon="Delete"    Text="删除"       ConfirmText="确定删除选定记录？"/>
                    <f:Button runat="server" ID="btnExportExcel"    Icon="PageExcel" Text="导出Excel"  EnableAjax="false" DisableControlBeforePostBack="false" />
                    <f:ToolbarFill runat = "server" />
                </Items>
            </f:Toolbar>
        </Toolbars>
        */
        protected void CreateToolbarButtons<T>(FineUI.Toolbar toolbar=null) where T : class, IID
        {
            if (AllowNew || AllowBatchDelete || AllowExport)
            {
                // 如果工具栏为空，则尝试插在网格第一个工具栏内
                if (toolbar == null)
                {
                    if (this.Toolbars.Count == 0)
                        this.Toolbars.Add(new FineUI.Toolbar());
                    toolbar = this.Toolbars[0];
                }

                // 填满左侧
                if (RelayoutToolbar)
                    toolbar.Items.Insert(0, new FineUI.ToolbarFill());

                // 导出按钮
                if (AllowExport)
                {
                    var btn = new FineUI.Button() { Icon = FineUI.Icon.PageExcel, Text = this.ExportText, EnableAjax =  false, DisableControlBeforePostBack = false };
                    btn.Click += (s, e) => ExportFile<T>();
                    toolbar.Items.Insert(0, btn);
                    ExportButton = btn;
                }

                // 批量删除按钮
                if (AllowBatchDelete)
                {
                    var btn = new FineUI.Button(){ Icon = FineUI.Icon.Delete, Text = this.DeleteText, ConfirmText = "确定删除选定记录么？" };
                    btn.Click += (s, e) => DeleteSelectedRecords<T>();
                    toolbar.Items.Insert(0, btn);
                    BatchDeleteButton = btn;
                }

                // 新增按钮
                if (AllowNew)
                {
                    var btn = new FineUI.Button(){ Icon = FineUI.Icon.Add, Text = this.NewText};
                    btn.Click += (s, e) => AddNew();
                    toolbar.Items.Insert(0, btn);
                    NewButton = btn;
                }
            }
        }

        //-------------------------------------------------
        // 分页控件
        //-------------------------------------------------
        /*
        <PageItems>
            <f:ToolbarSeparator ID="ToolbarSeparator3" runat="server" />
            <f:ToolbarText ID="ToolbarText1" runat="server" Text="每页记录数：" />
            <f:DropDownList ID="ddlGridPageSize" Width="80px" AutoPostBack="true" OnSelectedIndexChanged="ddlGridPageSize_SelectedIndexChanged" runat="server">
                <f:ListItem Text="10" Value="10" />
                <f:ListItem Text="20" Value="20" />
                <f:ListItem Text="50" Value="50" />
                <f:ListItem Text="100" Value="100" />
            </f:DropDownList>
        </PageItems>
        */
        protected FineUI.DropDownList _ddlGridPageSize;
        private void CreatePager() 
        {
            this.PageItems.Add(new FineUI.ToolbarSeparator());
            this.PageItems.Add(new FineUI.ToolbarText() { Text = "每页记录数" });
            _ddlGridPageSize = new FineUI.DropDownList() { Width = 80, AutoPostBack = true };
            _ddlGridPageSize.Items.Add(new FineUI.ListItem("10", "10"));
            _ddlGridPageSize.Items.Add(new FineUI.ListItem("20", "20"));
            _ddlGridPageSize.Items.Add(new FineUI.ListItem("30", "30"));
            _ddlGridPageSize.Items.Add(new FineUI.ListItem("50", "50"));
            _ddlGridPageSize.Items.Add(new FineUI.ListItem("100", "100"));
            this.PageItems.Add(_ddlGridPageSize);
        }




        //-------------------------------------------------
        // 列
        //-------------------------------------------------
        /*
        <Columns>
            <f:RowNumberField Width = "30px" EnablePagingNumber="true" />
            <f:WindowField ColumnID = "Edit" TextAlign="Center" Icon="Info" ToolTip="查看" Width="30px" 
                WindowID="Window1" Title="查看" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="UserView.aspx?id={0}"
                />
            <f:WindowField ColumnID = "Edit" TextAlign="Center" Icon="Pencil" ToolTip="编辑" Width="30px" 
                WindowID="Window1" Title="编辑" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="UserEdit.aspx?id={0}"
                />
            <f:LinkButtonField ColumnID = "Delete" TextAlign="Center" Icon="Delete" ToolTip="删除" Width="30px" 
                ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" 
                />
            <f:BoundField DataField = "ID" SortField="ID" Width="30px" HeaderText="ID" Hidden="true" />
        </Columns>
        */
        public void CreateColumns<T>() 
            where T : class, IID
        {
            if (ShowIDField)
                this.Columns.Insert(0, new FineUI.BoundField()
                {
                    DataField = "ID", SortField = "ID", HeaderText = "ID",
                    Width = 30
                });
            if (ShowDeleteField && AllowDelete)
                this.Columns.Insert(0, new FineUI.LinkButtonField()
                {
                    ColumnID = "Delete",
                    TextAlign = FineUI.TextAlign.Center,
                    Icon = FineUI.Icon.Delete,
                    ToolTip = this.DeleteText,
                    ConfirmText = "确定删除此记录?",
                    ConfirmTarget = FineUI.Target.Top,
                    CommandName = "Delete",
                    Width = 30
                });
            if (ShowEditField && AllowEdit)
                this.Columns.Insert(0, new FineUI.WindowField()
                {
                    ColumnID = "Edit",
                    TextAlign = FineUI.TextAlign.Center,
                    Icon = FineUI.Icon.Pencil,
                    ToolTip = "编辑",
                    Width = 30,
                    Title = "编辑",
                    WindowID = this.WinID,
                    DataIFrameUrlFields = "ID",
                    DataIFrameUrlFormatString = this.EditUrlTmpl
                });
            if (ShowViewField)
                this.Columns.Insert(0, new FineUI.WindowField()
                {
                    ColumnID = "View",
                    TextAlign = FineUI.TextAlign.Center,
                    Icon = FineUI.Icon.Information,
                    ToolTip = "查看",
                    Width = 30,
                    Title = "查看",
                    WindowID = this.WinID,
                    DataIFrameUrlFields = "ID",
                    DataIFrameUrlFormatString = this.ViewUrlTmpl
                });
            if (ShowNumberField)
                this.Columns.Insert(0, new FineUI.RowNumberField()
                {
                    Width = 30, EnablePagingNumber = true
                });

            // 自动生成属性列
            if (AutoCreateFields)
                AddColumns(this, new UISetting(typeof(T)));
        }

        //-------------------------------------------------
        // 新增
        //-------------------------------------------------
        private void AddNew()
        {
            int id = GridHelper.GetSelectedId(this);
            string url = string.Format(this.NewUrlTmpl, id);
            PageContext.RegisterStartupScript(this._window.GetShowReference(url, "新增"));
        }

        //-------------------------------------------------
        // 删除
        //-------------------------------------------------
        // 行删除按钮（先尝试直接调用 OnDelete 事件，若不存在则调用默认删除方法）
        protected void SetDeleteCommand<T>() where T : class, IID
        {
            if (ShowDeleteField && AllowDelete)
            {
                this.RowCommand += (s, e) =>
                {
                    if (e.CommandName == "Delete")
                    {
                        int id = GridHelper.GetSelectedId(this);
                        if (id == -1) return;
                        if (Delete != null)
                            Delete(this, new List<int>() { id });
                        else
                            AppContext.Current.Set<T>().Where(t => t.ID == id).Delete();
                        ShowData();
                    }
                };
            }
        }

        // 批量删除（先尝试直接调用 OnDelete 事件，若不存在则调用默认删除方法）
        protected void DeleteSelectedRecords<T>() where T : class, IID
        {
            List<int> ids = GridHelper.GetSelectedIds(this);
            if (ids.Count == 0) return;
            if (Delete != null)
                Delete(this, ids);
            else
                AppContext.Current.Set<T>().Where(t => ids.Contains(t.ID)).Delete();
            ShowData();
        }

        //-------------------------------------------------
        // 导出
        //-------------------------------------------------
        /// <summary>导出Excel。要求实现Export事件</summary>
        protected void ExportFile<T>()
        {
            if (Export != null)
                Export(this, null);
        }
        /// <summary>导出Excel。</summary>
        public void ExportExcel<T>(List<T> list, string fileName="")
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = string.IsNullOrEmpty(Title)
                    ? string.Format("{0}.xls", typeof(T).GetTypeInfo().Name)
                    : string.Format("{0}-{1:yyyyMMddHHmm}.xls", Title, DateTime.Now)
                    ;
            ExcelHelper.Export<T>(list, fileName);
        }

        //-------------------------------------------------
        // 自动生成绑定列
        //-------------------------------------------------
        /// <summary>
        /// 供普通Grid快速显示只读数据用，本方法会自动生成列。
        /// 由于其列是自动生成的，请在每次页面访问时都调用。
        /// 要想实现完整的分页、排序、删除、编辑等逻辑，请用GridPro控件。
        /// </summary>
        public static void BindGrid(Grid grid, DataTable dt)
        {
            grid.Columns.Clear();
            foreach (DataColumn col in dt.Columns)
            {
                grid.Columns.Add(CreateBoundField(col.ColumnName, col.ColumnName, "", 100));
            }
            grid.DataSource = dt;
            grid.DataBind();
        }
        public static void BindGrid(Grid grid, Type type, object data)
        {
            BindGrid(grid, new UISetting(type), data);
        }
        protected static void BindGrid(Grid grid, UISetting ui, object data)
        {
            grid.Columns.Clear();
            AddColumns(grid, ui);
            grid.DataSource = data;
            grid.DataBind();
        }

        /// <summary>根据UISetting，自动添加网格列</summary>
        protected static void AddColumns(Grid grid, UISetting ui)
        {
            FineUI.GroupField group = null;
            foreach (var attr in ui.Settings)
            {
                // 跳过字段；跳过复杂字段（未实现）;构建绑定列
                if (attr.Field.Name == "ID") continue;  // 跳过ID字段
                if (attr.ShowInGrid)
                {
                    // 根据 UIAttribute 创建绑定列
                    string formatString = GetDefaultFormatString(attr.Field.PropertyType);
                    if (!string.IsNullOrEmpty(attr.FormatString)) formatString = attr.FormatString;
                    var field = CreateBoundField(attr.Field.Name, attr.Title, formatString, attr.Width);

                    // 分组
                    if (string.IsNullOrEmpty(attr.Group))
                        grid.Columns.Add(field);
                    else
                    {
                        if (group != null && group.HeaderText == attr.Group)
                            group.Columns.Add(field);
                        else
                        {
                            group = new FineUI.GroupField() { HeaderText = attr.Group };
                            group.Columns.Add(field);
                            grid.Columns.Add(group);
                        }
                    }
                }
            }
        }

        // 创建绑定列
        protected static FineUI.BoundField CreateBoundField(string dataField, string title, string formatString, int width)
        {
            return new FineUI.BoundField()
            {
                DataField = dataField,
                SortField = dataField,
                HeaderText = title,
                DataFormatString = formatString,
                Width = width
            };
        }

        // 根据对象类型，获取默认的格式化字符串
        protected static string GetDefaultFormatString(Type type)
        {
            if (type == typeof(Double)) return "{0:0.00}";
            if (type == typeof(Single)) return "{0:0.00}";
            if (type == typeof(Decimal)) return "{0:0.00}";
            if (type == typeof(DateTime)) return "{0:yyyy-MM-dd}";
            if (type == typeof(Nullable<Double>)) return "{0:0.00}";
            if (type == typeof(Nullable<Single>)) return "{0:0.00}";
            if (type == typeof(Nullable<Decimal>)) return "{0:0.00}";
            if (type == typeof(Nullable<DateTime>)) return "{0:yyyy-MM-dd}";
            return "{0}";
        }

        // 是否只读
        public bool ReadOnly
        {
            set
            {
                if (value == true)
                    this.AllowNew = this.AllowDelete = this.AllowBatchDelete = this.AllowEdit = this.ShowEditField = false;
            }
        }
    }
}