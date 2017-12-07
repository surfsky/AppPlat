using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace App.Controls
{
    /// <summary>
    /// 反射相关静态方法和属性
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// 将可空对象安全转化为字符串
        /// </summary>
        public static string ToText(this object o)
        {
            return o == null ? "" : o.ToString();
        }

        // 获取数据集版本号
        public static Version AssemblyVersion
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        /// <summary>获取指定特性</summary>
        public static T GetAttribute<T>(Type type) where T : Attribute
        {
            T[] arr = (T[])type.GetCustomAttributes(typeof(T), true);
            return (arr.Length == 0) ? null : arr[0];
        }

        /// <summary>获取指定特性</summary>
        public static T GetAttribute<T>(PropertyInfo p) where T : Attribute
        {
            T[] arr = (T[])p.GetCustomAttributes(typeof(T), true);
            return (arr.Length == 0) ? null : arr[0];
        }

        /// <summary>获取可空类型中的值类型</summary>
        public static Type GetNullableDataType(Type type)
        {
            if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
                return type.GetGenericArguments()[0];
            return type;
        }

        /// <summary>获取泛型中的数据类型</summary>
        public static Type GetGenericDataType(Type type)
        {
            if (type.IsGenericType)
                return type.GetGenericArguments()[0];
            return type;
        }


        /// <summary>获取对象的属性值。也可用dynamic实现。</summary>
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            Type type = obj.GetType();
            PropertyInfo pi = type.GetProperty(propertyName);
            return pi.GetValue(obj);
        }

        /// <summary>设置对象的属性值。也可用dynamic实现。</summary>
        public static void SetPropertyValue(this object obj, string propertyName, string propertyValue)
        {
            Type type = obj.GetType();
            PropertyInfo pi = type.GetProperty(propertyName);
            object v = propertyValue;

            // 值类型判断。将字符串转化为对应的值类型。
            if (pi.PropertyType == typeof(bool))      v = (propertyValue == "") ? false          : Boolean.Parse(propertyValue);
            if (pi.PropertyType == typeof(Int16))     v = (propertyValue == "") ? (Int16)0       : Int16.Parse(propertyValue);
            if (pi.PropertyType == typeof(Int32))     v = (propertyValue == "") ? (Int32)0       : Int32.Parse(propertyValue);
            if (pi.PropertyType == typeof(Int64))     v = (propertyValue == "") ? (Int32)0       : Int64.Parse(propertyValue);
            if (pi.PropertyType == typeof(float))     v = (propertyValue == "") ? (float)0       : float.Parse(propertyValue);
            if (pi.PropertyType == typeof(double))    v = (propertyValue == "") ? (double)0      : double.Parse(propertyValue);
            if (pi.PropertyType == typeof(decimal))   v = (propertyValue == "") ? (decimal)0     : decimal.Parse(propertyValue);
            if (pi.PropertyType == typeof(DateTime))  v = (propertyValue == "") ? new DateTime() : DateTime.Parse(propertyValue);
            if (pi.PropertyType == typeof(bool?))     v = (propertyValue == "") ? null           : new bool?(Boolean.Parse(propertyValue));
            if (pi.PropertyType == typeof(Int16?))    v = (propertyValue == "") ? null           : new Int16?(Int16.Parse(propertyValue));
            if (pi.PropertyType == typeof(Int32?))    v = (propertyValue == "") ? null           : new Int32?(Int32.Parse(propertyValue));
            if (pi.PropertyType == typeof(Int64?))    v = (propertyValue == "") ? null           : new Int64?(Int64.Parse(propertyValue));
            if (pi.PropertyType == typeof(float?))    v = (propertyValue == "") ? null           : new float?(float.Parse(propertyValue));
            if (pi.PropertyType == typeof(double?))   v = (propertyValue == "") ? null           : new double?(double.Parse(propertyValue));
            if (pi.PropertyType == typeof(decimal?))  v = (propertyValue == "") ? null           : new decimal?(decimal.Parse(propertyValue));
            if (pi.PropertyType == typeof(DateTime?)) v = (propertyValue == "") ? null           : new DateTime?(DateTime.Parse(propertyValue));

            //
            if (pi.CanWrite)
                pi.SetValue(obj, v, null);
        }

        /// <summary>递归判断是否属于某个类型</summary>
        public static bool IsType(this Type type1, Type type2)
        {
            if (type1 == type2)
                return true;
            if (type1.ToString() == "System.Object")
                return false;
            return IsType(type1.BaseType, type2);
        }

        public static bool IsType(this Type type, string typeName)
        {
            if (type.ToString() == typeName)
                return true;
            if (type.ToString() == "System.Object")
                return false;
            return IsType(type.BaseType, typeName);
        }

    }
}