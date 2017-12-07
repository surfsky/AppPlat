using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace App.Components
{
    /// <summary>
    /// 反射相关静态方法和属性
    /// </summary>
    public static class ReflectionHelper
    {
        //------------------------------------------------
        // 数据集相关
        //------------------------------------------------
        // 获取数据集版本号
        public static Version AssemblyVersion
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public static string AssemblyPath
        {
            get { return Assembly.GetExecutingAssembly().Location; }
        }

        public static string AssemblyDirectory
        {
            get { return new FileInfo(AssemblyPath).DirectoryName; }
        }



        //------------------------------------------------
        // 辅助
        //------------------------------------------------
        /// <summary>获取当前方法名（未测试）</summary>
        public static string GetCurrentMethodName()
        {
            return new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name;
        }


        //------------------------------------------------
        // 类型相关
        //------------------------------------------------
        /// <summary>是否是某个类型（或子类型）</summary>
        public static bool IsType(this Type raw, Type match)
        {
            return (raw == match) ? true : raw.IsSubclassOf(match);
        }

        /// <summary>是否属于某个类型</summary>
        public static bool IsType(this Type type, string typeName)
        {
            if (type.ToString() == typeName)
                return true;
            if (type.ToString() == "System.Object")
                return false;
            return IsType(type.BaseType, typeName);
        }


        /// <summary>是否是泛型类型</summary>
        public static bool IsGenericType(this Type type)
        {
            return type.IsGenericType;
        }

        /// <summary>是否是可空类型</summary>
        public static bool IsNullable(this Type type)
        {
            return (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)));
        }

        /// <summary>获取可空类型中的值类型</summary>
        public static Type GetNullableDataType(this Type type)
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



        //------------------------------------------------
        // 特性相关
        //------------------------------------------------
        /// <summary>获取指定特性</summary>
        public static T GetAttribute<T>(this Type type) where T : Attribute
        {
            T[] arr = (T[])type.GetCustomAttributes(typeof(T), true);
            return (arr.Length == 0) ? null : arr[0];
        }

        /// <summary>获取指定特性</summary>
        public static T GetAttribute<T>(this PropertyInfo p) where T : Attribute
        {
            T[] arr = (T[])p.GetCustomAttributes(typeof(T), true);
            return (arr.Length == 0) ? null : arr[0];
        }


        //------------------------------------------------
        // 读写属性
        //------------------------------------------------
        /// <summary>获取对象的属性值。也可考虑用dynamic实现。</summary>
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            Type type = obj.GetType();
            PropertyInfo pi = type.GetProperty(propertyName);
            return pi.GetValue(obj);
        }

        /// <summary>设置对象的属性值。也可考虑用dynamic实现。</summary>
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
            pi.SetValue(obj, v, null);
        }


        /// <summary>获取对象的属性值（强类型版本）。var name = user.GetPropertyValue(t=> t.Name);</summary>
        public static TValue GetPropertyValue<T, TValue>(this T obj, Expression<Func<T, TValue>> property)
        {
            return property.Compile().Invoke(obj);
        }

        /// <summary>设置对象的属性值（强类型版本）。user.SetPropertyValue(t=> t.Name, "Cherry");</summary>
        public static void SetPropertyValue<T, TValue>(this T obj, Expression<Func<T, TValue>> property, TValue value)
        {
            string name = GetPropertyName(property);
            typeof(T).GetProperty(name).SetValue(obj, value, null);
        }


        //------------------------------------------------
        // Linq 强类型方法
        //------------------------------------------------
        /// <summary>获取类的属性名。var name = GetPropertyName<User>(t => t.Name);</summary>
        public static string GetPropertyName<T>(Expression<Func<T, object>> expr)
        {
            var name = "";
            if (expr.Body is UnaryExpression)
                name = ((MemberExpression)((UnaryExpression)expr.Body).Operand).Member.Name;   // array.Length 数组长度是一元操作符
            else if (expr.Body is MemberExpression)
                name = ((MemberExpression)expr.Body).Member.Name;                              // 成员表达式。如 t.Name
            else if (expr.Body is ParameterExpression)
                name = ((ParameterExpression)expr.Body).Type.Name;                             // 参数本身。如 t
            return name;
        }

        /// <summary>获取类的属性名。var name = GetPropertyName&lt;User, string&gt;(t => t.Name);</summary>
        public static string GetPropertyName<T, TMember>(Expression<Func<T, TMember>> property)
        {
            return GetMemberInfo(property).Name;
        }

        /// <summary>获取对象的属性名。可用于获取一些匿名对象的属性名。GetPropertyName(() => user.Name</summary>
        public static string GetPropertyName<T>(Expression<Func<T>> expr)
        {
            return (((MemberExpression)(expr.Body)).Member).Name;
        }

        /// <summary>获取类的成员信息。GetMemberInfo<User>(t => t.Name);</summary>
        public static MemberInfo GetMemberInfo<T, TMember>(Expression<Func<T, TMember>> property)
        {
            MemberExpression me;
            if (property.Body is UnaryExpression)
                me = ((UnaryExpression)property.Body).Operand as MemberExpression;    // array.Length 数组长度是一元操作符
            else
                me = property.Body as MemberExpression;
            return me.Member;
        }

        /// <summary>获取方法名（失败）</summary>
        public static string GetMethodName<T>(Expression<Func<T, object>> expr)
        {
            var name = "";
            if (expr.Body is MethodCallExpression)
                name = ((MethodCallExpression)expr.Body).Method.Name;
            return name;
        }


    }
}