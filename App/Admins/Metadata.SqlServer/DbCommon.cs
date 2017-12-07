using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Kingsoc.Data
{
    /// <summary>
    /// 数据库相关的最基本的一些数据转换操作
    /// </summary>
    public class DbCommon
    {
        //----------------------------------------------
        // 将object转化为指定类型（包含null处理）
        //----------------------------------------------
        // string
        public static string ToString(object o, string nullValue="")
        {
            return (o != DBNull.Value) ? Convert.ToString(o) : nullValue;
        }

        // int64
        public static bool ToBoolean(object o, bool nullValue=false)
        {
            return (o != DBNull.Value) ? Convert.ToBoolean(o) : nullValue;
        }

        // int64
        public static long ToInt64(object o, long nullValue=0)
        {
            return (o != DBNull.Value) ? (long)Convert.ToInt64(o) : nullValue;
        }

        // int32
        public static int ToInt32(object o, int nullValue=0)
        {
            return (o != DBNull.Value) ? (int)Convert.ToInt64(o) : nullValue;
        }

        // datetime
        public static DateTime ToDateTime(object o) { return ToDateTime(o, new DateTime()); }
        public static DateTime ToDateTime(object o, DateTime nullValue)
        {
            return (o != DBNull.Value) ? (DateTime)Convert.ToDateTime(o) : nullValue;
        }

        // single
        public static float ToSingle(object o, float nullValue=0.0f)
        {
            return (o != DBNull.Value) ? (float)Convert.ToSingle(o) : nullValue;
        }

        // double
        public static double ToDouble(object o, double nullValue=0.0)
        {
            return (o != DBNull.Value) ? (double)Convert.ToDouble(o) : nullValue;
        }

        // enum
        public static T ToEnum<T>(object o)
        {
            T nullValue = (T)Enum.GetValues(typeof(T)).GetValue(0);
            return ToEnum<T>(o, nullValue);
        }
        public static T ToEnum<T>(object o, T nullValue)
        {
            return (o != DBNull.Value)
                ? (T)Enum.Parse(typeof(T), o.ToString())
                : nullValue
                ;
        }

        // object
        public static T ToObject<T>(object o, T nullValue) where T : class
        {
            return (o != DBNull.Value)
                ? (T)Convert.ChangeType(o, typeof(T))
                : nullValue
                ;
        }

        /// <summary>
        /// 获取datatable指定列的值数组
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static List<string> ToStrings(DataTable dt, string columnName)
        {
            List<string> arr = new List<string>();
            foreach (DataRow dr in dt.Rows)
                arr.Add(ToString(dr[columnName]));
            return arr;
        }

    }
}
