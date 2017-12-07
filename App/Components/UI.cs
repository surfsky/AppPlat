using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;
using App.DAL;
using FineUI;
using App.Components;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Drawing;

namespace App.Components
{
    /// <summary>
    /// UI 操作 (FineUI) 辅助类
    /// </summary>
    public class UI
    {
        // 显示订单(想办法弄个参数回来，是否已经支付）
        public static bool ShowOrder(FineUI.Window win, int? orderId)
        {
            if (orderId != null)
            {
                var url = string.Format("OrderForm.aspx?id={0}&mode=view", orderId);
                var script = win.GetShowReference(url);
                PageContext.RegisterStartupScript(script);
                return true;
            }
            return false;
        }

        /*
        public static void ShowOrder(HtmlForm frm, int? orderId)
        {
            var win = new FineUI.Window()
            {
                IsModal = true,
                Hidden = false,
                Target = FineUI.Target.Top,
                Width = 500,
                Height = 700,
                EnableResize = true,
                EnableMaximize = true,
                EnableClose = true,
                EnableIFrame = true,
                IFrameUrl = "about:blank",
                CloseAction = CloseAction.HidePostBack
            };
            frm.Controls.Add(win);  // 临时加的没用的
            ShowOrder(win, orderId);
        }
        */

        //------------------------------------------------------
        // 权限检测
        //------------------------------------------------------
        /// <summary>显示错误警告框</summary>
        public static void ShowAlert(string info)
        {
            PageContext.RegisterStartupScript(Alert.GetShowInTopReference(info));
        }

        /// <summary>显示权限错误警告框</summary>
        public static void ShowPowerFailAlert()
        {
            ShowAlert(Common.CHECK_POWER_FAIL_ACTION_MESSAGE);
        }

        /// <summary>给父页面postback刷新消息</summary>
        public static void PostParentToRefresh(string argument="Refresh")
        {
            string script = string.Format("parent.__doPostBack('', '{0}');", argument);
            PageContext.RegisterStartupScript(script);
        }

        /// <summary>是否是PostBack刷新命令</summary>
        public static bool IsPostBackRefresh(Page page, string argument = "Refresh")
        {
            return page.IsPostBack && Asp.Request.Params["__EVENTARGUMENT"] == argument;
        }

        //------------------------------------------------------
        // DropDownList-Tree
        //------------------------------------------------------
        // 绑定到下拉列表（启用模拟树功能和不可选择项功能）
        public static void BindDDLTree<T>(DropDownList ddl, List<T> data, string title = "--请选择--", int? selectedId = null, int? disableId = null)
            where T : ITree, ICloneable, IID, new()
        {
            var tree = PrepareTree(data, "", disableId);
            ddl.EmptyText = title;
            ddl.AutoSelectFirstItem = false;
            ddl.EnableEdit = true;
            ddl.ForceSelection = true;
            ddl.EnableSimulateTree = true;
            ddl.DataTextField = "Name";
            ddl.DataValueField = "ID";
            ddl.DataSimulateTreeLevelField = "TreeLevel";
            ddl.DataEnableSelectField = "Enabled";
            ddl.DataSource = tree;
            ddl.DataBind();

            if (selectedId != null)
                ddl.SelectedValue = selectedId.ToString();
        }


        /// <summary>预处理树。加上根节点，禁止某些节点。</summary>
        static List<T> PrepareTree<T>(List<T> source, string title = "--根节点--", int? disableId = null) where T : ITree, ICloneable, IID, new()
        {
            bool addRootNode = !string.IsNullOrEmpty(title);
            List<T> result = new List<T>();

            // 添加根节点
            if (addRootNode)
            {
                T root = new T();
                root.Name = title;
                root.ID = -1;
                root.TreeLevel = 0;
                root.Enabled = true;
                result.Add(root);
            }

            // 拷贝到新列表
            foreach (T item in source)
            {
                T newT = (T)item.Clone();
                result.Add(newT);

                // 有根节点的话，所有节点的TreeLevel加一
                if (addRootNode)
                    newT.TreeLevel++;
            }

            // 当前节点及子节点都不可选择
            if (disableId != null)
            {
                bool startChildNode = false;
                int startTreeLevel = 0;
                foreach (T node in result)
                {
                    if (node.ID == disableId.Value)
                    {
                        startTreeLevel = node.TreeLevel;
                        node.Enabled = false;
                        startChildNode = true;
                    }
                    else
                    {
                        if (startChildNode)
                        {
                            if (node.TreeLevel > startTreeLevel)
                                node.Enabled = false;
                            else
                                startChildNode = false;
                        }
                    }
                }
            }
            return result;
        }

        //------------------------------------------------------
        // DropDownList
        //------------------------------------------------------
        // 绑定下拉框
        public static void BindDDL(DropDownList ddl, IEnumerable data, string textField, string valueField, string title = "--请选择--", int? selectedId = null)
        {
            ddl.EmptyText = title;
            ddl.AutoSelectFirstItem = false;
            ddl.EnableEdit = true;
            ddl.ForceSelection = true;
            ddl.DataSource = data;
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataBind();
            if (selectedId != null)
                ddl.SelectedValue = selectedId.ToString();
        }

        // 绑定下拉框（多选）
        public static void BindDDLMulti(DropDownList ddl, IEnumerable data, string textField, string valueField, string title = "--请选择--", string[] selectedValues = null)
        {
            ddl.EmptyText = title;
            ddl.AutoSelectFirstItem = false;
            ddl.EnableEdit = true;
            ddl.ForceSelection = true;
            ddl.EnableMultiSelect = true;
            ddl.DataSource = data;
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataBind();
            if (selectedValues != null)
                ddl.SelectedValueArray = selectedValues;
        }

        // 绑定下拉框（bool类型）
        public static void BindDDLBool(DropDownList ddl, string trueText, string falseText, string title = "--请选择--", bool? value = null)
        {
            ddl.EmptyText = title;
            ddl.AutoSelectFirstItem = false;
            ddl.EnableEdit = true;
            ddl.ForceSelection = true;
            ddl.Items.Add(new ListItem(trueText, "true"));
            ddl.Items.Add(new ListItem(falseText, "false"));
            if (value != null)
                ddl.SelectedValue = value.ToText();
        }

        // 绑定下拉框（枚举类型）
        public static void BindDDLEnum(DropDownList ddl, Type enumType, string title = "--请选择--", string enumGroup = "", int? selectedId = null)
        {
            var items = enumType.ToList();
            if (enumGroup.IsNullOrEmpty())
            {
                BindDDL(ddl, items, "Name", "ID", title, selectedId);
            }
            else
            {
                enumGroup = enumGroup?.ToLower();
                ddl.Items.Clear();
                foreach (var item in items)
                    if (item.Group?.ToLower() == enumGroup)
                        ddl.Items.Add(item.Name, item.ID.ToString());

                ddl.EmptyText = title;
                ddl.AutoSelectFirstItem = false;
                ddl.ForceSelection = true;
                ddl.EnableEdit = true;
                if (ddl.Items.Count == 1)
                {
                    ddl.EnableEdit = false;
                    ddl.SelectedIndex = 0;
                }
            }
        }

        // 获取下拉框选中值（整数）
        public static int? GetDDLValue(DropDownList ddl)
        {
            return ddl.SelectedItemArray.Length != 0 ? (int?)int.Parse(ddl.SelectedValue) : null;
        }

        // 获取下拉框布尔值
        public static bool? GetDDLBoolValue(DropDownList ddl)
        {
            return ddl.SelectedItemArray.Length != 0 ? (bool?)bool.Parse(ddl.SelectedValue) : null;
        }

        // 获取枚举值(下拉框列表)
        public static dynamic GetDDLEnumValue(DropDownList ddl, Type enumType)
        {
            int? n = GetDDLValue(ddl);
            if (n == null) return null;
            else return Enum.ToObject(enumType, n);
        }

        /// <summary>设置枚举值</summary>
        public static void SetDDLEnumValue(DropDownList ddl, object enumValue)
        {
            ddl.SelectedValue = (enumValue == null) ? "" : Convert.ToInt32(enumValue).ToString();
        }
        /// <summary>设置枚举值</summary>
        public static void SetDDLValue(DropDownList ddl, object value)
        {
            ddl.SelectedValue = (value == null) ? "" : value.ToString();
        }

        /// <summary>设置枚举值</summary>
        public static void SetDDLValues(DropDownList ddl, string[] values)
        {
            ddl.SelectedValueArray = values;
        }

        //------------------------------------------------------
        // RadioButtonList
        //------------------------------------------------------
        // 绑定到复选框列表
        public static void BindCBL(CheckBoxList cbl, IEnumerable data, string textField, string valueField, int? selectedId = null)
        {
            cbl.DataSource = data;
            cbl.DataTextField = textField;
            cbl.DataValueField = valueField;
            cbl.DataBind();
            if (selectedId != null)
                cbl.SelectedValueArray = new string[] { selectedId.ToString() };
        }

        // 绑定到单选框列表
        public static void BindCBLEnum(CheckBoxList cbl, Type enumType, int? selectedId = null)
        {
            var items = enumType.ToList();
            BindCBL(cbl, items, "Name", "ID", selectedId);
        }


        // 获取多选框值列表
        public static string[] GetCBLValue(CheckBoxList cbl)
        {
            return cbl.SelectedValueArray;
        }

        // 获取多选框值列表
        public static int[] GetCBLIntValue(CheckBoxList cbl)
        {
            return cbl.SelectedValueArray.CastInt().ToArray();
        }

        //------------------------------------------------------
        // RadioButtonList
        //------------------------------------------------------
        // 绑定到单选框列表（默认设置第一个值选中）
        public static void BindRBL(RadioButtonList rbl, IEnumerable data, string textField, string valueField, int? selectedId = null)
        {
            rbl.DataSource = data;
            rbl.DataTextField = textField;
            rbl.DataValueField = valueField;
            rbl.DataBind();
            if (selectedId != null)
                rbl.SelectedValue = selectedId.ToString();
            else
                rbl.SelectedIndex = 0;
        }

        // 绑定到单选框列表
        public static void BindRBLEnum(RadioButtonList rbl, Type enumType, int? selectedId=null)
        {
            var items = enumType.ToList();
            BindRBL(rbl, items, "Name", "ID", selectedId);
        }

        // 绑定到单选框列表
        public static void BindRBLBool(RadioButtonList rbl, string trueText, string falseText, bool? value = null)
        {
            rbl.Items.Clear();
            rbl.Items.Add(new RadioItem(trueText, "true"));
            rbl.Items.Add(new RadioItem(falseText, "false"));
            if (value != null)
                rbl.SelectedValue = value.ToString();
        }

        // 获取枚举值(单选框列表)
        public static dynamic GetRBLEnumValue(RadioButtonList rbl, Type enumType)
        {
            if (rbl.SelectedIndex == -1)
                return null;
            else
            {
                int n = int.Parse(rbl.SelectedValue);
                return Enum.ToObject(enumType, n);
            }
        }

        // 获取枚举值(单选框列表)
        public static T? GetRBLEnumValue<T>(RadioButtonList rbl) where T : struct
        {
            if (rbl.SelectedIndex == -1)
                return null;
            else
            {
                int n = int.Parse(rbl.SelectedValue);
                return (T)Enum.ToObject(typeof(T), n);
            }
        }

        // 获取下拉框布尔值
        public static bool? GetRBLBoolValue(RadioButtonList rbl)
        {
            return rbl.SelectedIndex != -1 ? (bool?)bool.Parse(rbl.SelectedValue) : null;
        }

        /// <summary>设置枚举值</summary>
        public static void SetRBLEnumValue(RadioButtonList rbl, object enumValue)
        {
            rbl.SelectedValue = (enumValue == null) ? "" : Convert.ToInt32(enumValue).ToString();
        }

        //------------------------------------------------------
        // 其它控件的值获取
        //------------------------------------------------------
        /// <summary>获取文本框整型数据（RealTextField 的子类有：TextBox, NumberBox, DatePicker）</summary>
        public static int? GetTextBoxIntValue(RealTextField tb, int? defaultValue = null)
        {
            if (tb.Text.IsNullOrEmpty()) return defaultValue;
            else                         return int.Parse(tb.Text);
        }
        /// <summary>获取文本框Double数据（RealTextField 的子类有：TextBox, NumberBox, DatePicker）</summary>
        public static double? GetTextBoxDoubleValue(RealTextField tb, double? defaultValue = null)
        {
            if (tb.Text.IsNullOrEmpty()) return defaultValue;
            else                         return double.Parse(tb.Text);
        }

        // 获取日期时间
        public static DateTime? GetDateValue(DatePicker dp)
        {
            return dp.SelectedDate;
        }

        //------------------------------------------------------
        // 图片上传相关
        //------------------------------------------------------
        /// <summary>显示图片推荐尺寸</summary>
        public static void SetUploaderText(FineUI.FileUpload uploader, Size? size)
        {
            uploader.ButtonText = string.Format("上传图片(建议尺寸{0}x{1})", size?.Width, size?.Height);
        }


        /// <summary>上传图片到指定目录</summary>
        /// <param name="fileUpload">文件上传控件</param>
        /// <param name="folderName">上传目录名，如Users</param>
        /// <returns>图片的虚拟路径</returns>
        public static string UploadFile(FineUI.FileUpload fileUpload, string folderName, System.Drawing.Size? size=null)
        {
            if (!fileUpload.HasFile)
                return "";

            // 图片文件检测
            string fileName = fileUpload.ShortFileName;
            if (!Common.IsImageFile(fileName))
            {
                Alert.Show("无效的图片类型！");
                return "";
            }

            // 保存文件, 调整尺寸
            string virtualPath = Common.GetUploadFilePath(folderName, fileName);
            string physicalPath = SaveUploadFile(fileUpload, virtualPath);
            if (size != null)
                DrawHelper.CreateThumbnail(physicalPath, physicalPath, size.Value.Width);
            return virtualPath;
        }


        /// <summary>保存上传文件</summary>
        private static string SaveUploadFile(FileUpload fileUpload, string virtualPath)
        {
            string physicalPath = HttpContext.Current.Server.MapPath(virtualPath);
            var fi = new FileInfo(physicalPath);
            if (!Directory.Exists(fi.Directory.FullName))
                Directory.CreateDirectory(fi.Directory.FullName);
            fileUpload.SaveAs(physicalPath);
            return physicalPath;
        }



        /// <summary>替换image控件图片，若有需要，物理删除原文件</summary>
        /// <param name="img">图像控件</param>
        /// <param name="newImageUrl">新图片虚拟路径</param>
        public static void SetImage(FineUI.Image img, string newImageUrl, bool deleteOldImage=false)
        {
            if (deleteOldImage)
                Common.SafeDeleteFile(img.ImageUrl);
            img.ImageUrl = newImageUrl;
        }



        //------------------------------------------------------
        // 权限相关
        //------------------------------------------------------
        // 根据权限设置按钮状态
        public static void SetButtonByPower(FineUI.Button btn, PowerType power)
        {
            if (!Common.CheckPower(power))
            {
                btn.Hidden = true;
            }
        }

        // 根据权限设置多选列的显隐
        public static void SetGridCheckColumnByPower(FineUI.Grid grid, PowerType power)
        {
            grid.EnableCheckBoxSelect = Common.CheckPower(power);
        }

        // 根据权限设置网格列状态
        // Common.SetGridColumnByPower("CoreDeptEdit", Grid1, "editField");
        // Common.SetGridColumnByPower("CoreDeptDelete", Grid1, "deleteField");
        public static void SetGridColumnByPower(FineUI.Grid grid, string columnID, PowerType power)
        {
            if (!Common.CheckPower(power))
            {
                BaseField field = grid.FindColumn(columnID) as BaseField;
                field.ToolTip = Common.CHECK_POWER_FAIL_ACTION_MESSAGE;
                field.Hidden = true;     // 整个列都隐藏
                //field.Enabled = false; // 按钮不能点
                //field.Visible = false; // BUG: 加上该语句页面就无法显示
            }
        }


        /// <summary> 给网格批量操作按钮增加客户端确认框</summary>
        public static void SetGridBatchActionConfirm(
            Grid grid, FineUI.Button btn,
            string actionName = "删除",
            string confirmTemplate = "确定要{0}选中的<span><script>{1}</script></span>项记录吗？",
            string noSelectionMessage = "请至少应该选择一条记录！"
            )
        {
            btn.OnClientClick = grid.GetNoSelectionAlertInParentReference(noSelectionMessage);
            btn.ConfirmText = String.Format(confirmTemplate, actionName, grid.GetSelectedCountReference());
            btn.ConfirmTarget = FineUI.Target.Top;
        }
        public static void SetGridBatchActionConfirm(
            Grid grid, FineUI.MenuButton btn,
            string actionName = "删除",
            string confirmTemplate = "确定要{0}选中的<span><script>{1}</script></span>项记录吗？",
            string noSelectionMessage = "请至少应该选择一条记录！"
            )
        {
            btn.OnClientClick = grid.GetNoSelectionAlertInParentReference(noSelectionMessage);
            btn.ConfirmText = String.Format(confirmTemplate, actionName, grid.GetSelectedCountReference());
            btn.ConfirmTarget = FineUI.Target.Top;
        }

        /// <summary>
        /// 在主窗口增加标签页。
        /// 注：addMainTab在Main.aspx中注册了
        /// </summary>
        /// <param name="id">标签页的ID，若再次调用，会跳到已经打开的标签页。</param>
        /// <param name="url">标签页URL</param>
        /// <param name="title">标题</param>
        /// <param name="icon">图标</param>
        /// <param name="refreshWhenExists">再次调用时是否刷新</param>
        public static void AddMainTab(string id, string url, string title, FineUI.Icon icon, bool refreshWhenExists=false)
        {
            string iconPath = GetIconUrl(icon);
            string script = string.Format("window.top.addMainTab('{0}', '{1}', '{2}', '{3}', {4})", id, url, title, iconPath, refreshWhenExists);
            FineUI.PageContext.RegisterStartupScript(script);
        }

        // 获取FineUI图标的url
        public static string GetIconUrl(FineUI.Icon icon)
        {
            //return string.Format("~/Res/Icon/{0}.png", FineUI.Icon.Comment);
            return FineUI.IconHelper.GetIconUrl(icon);
        }

    }
}