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
    /// 表单辅助方法（自动生成表单）
    /// </summary>
    public class FormHelper
    {
        //-----------------------------------------------
        // 构建表单
        //-----------------------------------------------
        // 动态生成表单（返回“字段名-控件”字典）
        public static Dictionary<string, Field> BuildForm(FormBase form, UISetting ui, bool showIdField = false, bool readOnly=false)
        {
            var map = new Dictionary<string, Field>();
            foreach (var attr in ui.Settings)
            {
                if (attr.Field.Name == "ID" && !showIdField) continue;
                if (attr.ShowInForm)
                {
                    // 根据 UIAttribute 创建控件
                    var editorType = GetDefaultEditorName(attr.Field.PropertyType, readOnly);
                    if (attr.Editor != EditorType.Auto) editorType = attr.Editor;
                    string title = string.IsNullOrEmpty(attr.Group) ? attr.Title : attr.Group + "-" + attr.Title;
                    var editor = CreateEditor(attr.Field.Name, title, editorType, attr.EditRequired, attr.EditPrecision);

                    // 添加到表单并记录到字典
                    form.Items.Add(editor);
                    map.Add(attr.Field.Name, editor);
                }
            }
            return map;
        }

        // 根据对象类型，获取默认编辑控件名称
        public static EditorType GetDefaultEditorName(Type type, bool readOnly)
        {
            if (readOnly)
            {
                return EditorType.TextBox;
            }
            else
            {
                if (type == typeof(String))             return EditorType.TextBox;
                if (type == typeof(Int16))              return EditorType.NumberBox;
                if (type == typeof(Int32))              return EditorType.NumberBox;
                if (type == typeof(Int64))              return EditorType.NumberBox;
                if (type == typeof(Double))             return EditorType.NumberBox;
                if (type == typeof(Single))             return EditorType.NumberBox;
                if (type == typeof(Decimal))            return EditorType.NumberBox;
                if (type == typeof(DateTime))           return EditorType.DatePicker;
                if (type == typeof(Nullable<Int16>))    return EditorType.NumberBox;
                if (type == typeof(Nullable<Int32>))    return EditorType.NumberBox;
                if (type == typeof(Nullable<Int64>))    return EditorType.NumberBox;
                if (type == typeof(Nullable<Double>))   return EditorType.NumberBox;
                if (type == typeof(Nullable<Single>))   return EditorType.NumberBox;
                if (type == typeof(Nullable<Decimal>))  return EditorType.NumberBox;
                if (type == typeof(Nullable<DateTime>)) return EditorType.DatePicker;
                return EditorType.TextBox;
            }
        }


        // 创建绑定列
        /*
        <f:Label runat = "server" ID ="lblId" Label="ID" Hidden="false" />
        <f:TextBox runat = "server" ID="tbxYear"            Label="年度（YYYY）" Required="true" ShowRedStar="true" />
        <f:NumberBox runat = "server" ID="tbxCityGDP"         Label=" 全市生产总值        " DecimalPrecision="2" />
        */
        public static FineUI.Field CreateEditor(string dataField, string title, EditorType editor, bool required = false, int precision = 2)
        {
            switch (editor)
            {
                case EditorType.Label:            return new FineUI.Label()      { ID = dataField, Label = title, Hidden = false };
                case EditorType.TextBox:          return new FineUI.TextBox()    { ID = dataField, Label = title, Required = required, ShowRedStar = required };
                case EditorType.TextArea:         return new FineUI.TextArea()   { ID = dataField, Label = title, Required = required, ShowRedStar = required, Height=200};
                case EditorType.HtmlEditor:       return new FineUI.HtmlEditor() { ID = dataField, Label = title, ShowRedStar = required, Height=400};
                case EditorType.MarkdownEditor:   return new FineUI.TextArea()   { ID = dataField, Label = title, Required = required, ShowRedStar = required, Height=400};
                case EditorType.NumberBox:        return new FineUI.NumberBox()  { ID = dataField, Label = title, Required = required, ShowRedStar = required, DecimalPrecision = precision };
                case EditorType.DatePicker:       return new FineUI.DatePicker() { ID = dataField, Label = title, Required = required, ShowRedStar = required, SelectedDate = DateTime.Today };
                case EditorType.TimePicker:       return new FineUI.TimePicker() { ID = dataField, Label = title, Required = required, ShowRedStar = required, SelectedDate = DateTime.Now };
                case EditorType.DateTimePicker:   return new FineUI.DatePicker() { ID = dataField, Label = title, Required = required, ShowRedStar = required, SelectedDate = DateTime.Now };
                case EditorType.Image:            return new FineUI.Image()      { ID = dataField, Label = title, ShowRedStar = required};
                default:                          return new FineUI.TextBox()    { ID = dataField, Label = title, Required = required, ShowRedStar = required };
            }
        }

        //-----------------------------------------------
        // 显示表单数据
        //-----------------------------------------------
        /// <summary>绑定表单。自动生成表单控件，并显示数据</summary>
        /// <param name="form">表单对象</param>
        /// <param name="o">数据对象</param>
        public static void BindForm(FormBase form, object o)
        {
            var map = FormHelper.BuildForm(form, new UISetting(o.GetType()));
            ShowFormData(map, o);
        }

        /// <summary>显示表单</summary>
        /// <param name="map">属性名-控件字典</param>
        /// <param name="o">对象</param>
        public static void ShowFormData(Dictionary<string, Field> map, object o)
        {
            foreach (string key in map.Keys)
            {
                FormHelper.SetEditorValue(map[key], o.GetPropertyValue(key));
            }
        }

        //-----------------------------------------------
        // 辅助方法
        //-----------------------------------------------
        /// <summary>设置表单为只读</summary>
        public static void SetFormEditable(FormBase form, bool editable = true)
        {
            ProcessFormItems(form, t => 
            {
                if (t is FileUpload) t.Hidden = !editable;
                else                 t.Readonly = !editable;
            });
        }

        // 遍历处理表单的所有控件
        public static void ProcessFormItems(FormBase form, Action<Field> process)
        {
            if (form is SimpleForm)
            {
                foreach (var item in form.Items)
                {
                    if (item is FormBase)        ProcessFormItems(item as FormBase, process);
                    else if (item is Field)      process(item as Field);
                }
            }
            if (form is Form)
            {
                var frm = form as Form;
                foreach (FormRow row in frm.Rows)
                {
                    foreach (var item in row.Controls)
                    {
                        if (item is FormBase)    ProcessFormItems(item as FormBase, process);
                        else if (item is Field)  process(item as Field);
                    }
                }
            }
        }

        // 设置控件值
        public static void SetEditorValue(Field editor, object value, string editorProperty = "Text")
        {
            if (editor is DatePicker)
            {
                var edt = editor as DatePicker;
                edt.SelectedDate = (DateTime?)value;
            }
            else if (editor is TimePicker)
            {
                var edt = editor as TimePicker;
                edt.SelectedDate = (DateTime?)value;
            }
            else if (editor is Image)
            {
                var edt = editor as Image;
                edt.ImageUrl = value.ToString();
            }
            else
            {
                editor.SetPropertyValue(editorProperty, value.ToText());
            }
        }

        // 获取控件值
        public static string GetEditorValue(Field editor, string editorProperty = "Text")
        {
            return editor.GetPropertyValue(editorProperty).ToText();
        }

        /// <summary>采集表单数据</summary>
        /// <param name="map">属性名-控件字典</param>
        /// <param name="o">数据对象</param>
        public static void CollectData(Dictionary<string, Field> map, ref object o)
        {
            foreach (var key in map.Keys)
            {
                string value = GetEditorValue(map[key]);
                o.SetPropertyValue(key, value);
            }
        }
    }
}