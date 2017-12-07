using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Serialization;

namespace App.Components
{
    /// <summary>
    /// 编辑器类型
    /// </summary>
    public enum EditorType
    {
        [UI("自动选择")]     Auto,
        [UI("标签")]         Label,
        [UI("文本框 ")]      TextBox,
        [UI("多行文本框")]   TextArea,
        [UI("HTML编辑框")]   HtmlEditor,
        [UI("MD编辑框")]     MarkdownEditor,
        [UI("数字框")]       NumberBox,
        [UI("日期选择")]     DatePicker,
        [UI("时间选择")]     TimePicker,
        [UI("日期时间选择")] DateTimePicker,
        [UI("图片")]         Image
    }

    //========================================================
    // UIAttribute
    //========================================================
    /// <summary>
    /// 描述字段对应控件的 UI 外观
    /// </summary>
    public class UIAttribute : Attribute
    {
        /// <summary>标题</summary>
        public string Title { get; set; }

        /// <summary>分组</summary>
        public string Group { get; set; }

        /// <summary>格式化字符串</summary>
        public string FormatString { get; set; }

        /// <summary>宽度</summary>
        public int Width { get; set; } = 100;

        /// <summary>是否只读</summary>
        public bool ReadOnly { get; set; } = false;

        //---------------------------------------------
        // 可见性
        //---------------------------------------------
        /// <summary>网格状态时是否显示（这三个属性可能要合并掉，用n个UIAttribute替代）</summary>
        public bool ShowInGrid { get; set; } = true;

        /// <summary>编辑状态时是否显示</summary>
        public bool ShowInForm { get; set; } = true;

        /// <summary>详情状态时是否显示</summary>
        public bool ShowInDetail { get; set; } = true;

        public bool Ignore
        {
            get
            {
                return this.ShowInGrid && this.ShowInForm && this.ShowInDetail;
            }
            set
            {
                if (value)
                {
                    ShowInGrid = false;
                    ShowInForm = false;
                    ShowInDetail = false;
                }
            }
        }

        //---------------------------------------------
        // 编辑状态
        //---------------------------------------------
        /// <summary>编辑控件</summary>
        public EditorType Editor { get; set; } = EditorType.Auto;

        /// <summary>编辑控件相关属性</summary>
        public string EditorInfo { get; set; } = "";

        /// <summary>是否必填</summary>
        public bool EditRequired { get; set; } = false;

        /// <summary>精度（小数类型）</summary>
        public int EditPrecision { get; set; } = 2;

        /// <summary>正则表达式</summary>
        public string EditRegex { get; set; } = "";


        //---------------------------------------------
        // 自动计算字段
        //---------------------------------------------
        // 全名
        [XmlIgnore]
        public string FullTitle
        {
            get
            {
                if (string.IsNullOrEmpty(Group)) return Title;
                else return string.Format("{0}-{1}", Group, Title);
            }
        }

        /// <summary>对应的字段信息</summary>
        [XmlIgnore]
        public PropertyInfo Field { get; set; }


        //---------------------------------------------
        // 构造函数
        //---------------------------------------------
        public UIAttribute(string title, string group = null, string formatString = "{0}")
        {
            this.Title = title;
            this.FormatString = formatString;
            this.Group = group;
        }
    }


    //========================================================
    // UIExtension
    //========================================================
    /// <summary>
    /// 辅助扩展方法
    /// </summary>
    public static class UIExtension
    {
        /// <summary>获取类拥有的 UIAttribute 列表</summary>
        public static List<UIAttribute> GetUIAttributes(this Type type)
        {
            var attrs = new List<UIAttribute>();
            foreach (var prop in type.GetProperties())
            {
                UIAttribute attr = ReflectionHelper.GetAttribute<UIAttribute>(prop);
                if (attr == null)
                    attr = new UIAttribute(prop.Name);
                attr.Field = prop;
                attrs.Add(attr);
            }
            return attrs;
        }

        /// <summary>获取枚举值的文本说明（来自UIAttribute或DescriptionAttribute）</summary>
        public static string GetDescription(this object enumValue)
        {
            if (enumValue == null) return "";
            var enumType = enumValue.GetType();
            FieldInfo info = enumType.GetField(enumValue.ToString());
            if (info != null)
            {
                var attr1 = info.GetCustomAttribute<UIAttribute>();
                var attr2 = info.GetCustomAttribute<DescriptionAttribute>();
                if (attr1 != null) return attr1.Title;
                if (attr2 != null) return attr2.Description;
                return enumValue.ToString();

            }
            return string.Empty;
        }

        /// <summary>获取枚举值的文本说明（来自UIAttribute或DescriptionAttribute）</summary>
        public static string GetGroup(this object enumValue)
        {
            if (enumValue == null) return "";
            var enumType = enumValue.GetType();
            FieldInfo info = enumType.GetField(enumValue.ToString());
            if (info != null)
            {
                var attr1 = info.GetCustomAttribute<UIAttribute>();
                if (attr1 != null)
                    return attr1.Group;
            }
            return "";
        }

        /// <summary>获取属性的文本说明（来自UIAttribute或DescriptionAttribute）</summary>
        /// <example>var text = typeof(Product).GetDescription("Name");</example>
        public static string GetDescription(this Type type, string propertyName)
        {
            var info = type.GetProperty(propertyName);
            return GetDescription(info);
        }

        /// <summary>获取属性的文本说明（来自UIAttribute或DescriptionAttribute）</summary>
        /// <example>var text = typeof(Product).GetDescription("Name");</example>
        public static string GetDescription(this PropertyInfo info)
        {
            if (info != null)
            {
                var attr1 = info.GetCustomAttribute<UIAttribute>();
                var attr2 = info.GetCustomAttribute<DescriptionAttribute>();
                if (attr1 != null) return attr1.Title;
                if (attr2 != null) return attr2.Description;
            }
            return "";
        }

        /// <summary>获取属性的文本说明（未完成）</summary>
        /// <example>
        /// TODO: 
        /// var text = typeof(Product).GetDescription(t => t.Name);
        /// var text = product.Name.GetDescription();
        /// var text = Product.GetDescription(t => t.Name);
        /// var text = UIExtension.GetDescription<Product, string>(t => t.Name);
        /// </example>
        public static string GetDescription<TSource, TResult>(Expression<Func<TSource, TResult>> expression)
        {
            throw new NotImplementedException();
        }
    }

    //========================================================
    // UISetting
    //========================================================
    /// <summary>
    /// UI 设置。可根据该类动态设置用户界面（如网格、表单等）
    /// TODO: 未完成
    /// </summary>
    [XmlInclude(typeof(UIAttribute))]
    public class UISetting : IID
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public Type ModelType { get; set; }
        public List<UIAttribute> Settings { get; set; }


        // 构造函数
        public UISetting(Type type, string title = "", int id = -1)
        {
            var attr = ReflectionHelper.GetAttribute<UIAttribute>(type);

            // title
            if (!string.IsNullOrEmpty(title))
                this.Title = title;
            else
            {
                if (attr != null && !string.IsNullOrEmpty(attr.Title))
                    this.Title = attr.Title;
                else
                    this.Title = type.Name;
            }

            //
            this.ID = id;
            this.ModelType = type;
            this.Settings = type.GetUIAttributes();
        }

        /// <summary>从xml文件中加载</summary>
        public static UISetting LoadXml(string filePath)
        {
            return SerializeHelper.LoadXml(filePath, typeof(UISetting)) as UISetting;
        }
        public void SaveXml(string filePath)
        {
            SerializeHelper.SaveXml(filePath, this);
        }
        //public static UISetting Demo()
        //{
        //    return new UISetting(typeof(User), "用户");
        //}
    }
}