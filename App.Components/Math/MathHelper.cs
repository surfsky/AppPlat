using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Components
{
    /// <summary>
    /// 数学相关辅助方法
    /// </summary>
    public class MathHelper
    {
        /// <summary>限制数字大小在一个区间内</summary>
        public static int Limit(int n, int min, int max)
        {
            if (n > max) return max;
            if (n < min) return min;
            return n;
        }

        /// <summary>限制数字大小在一个区间内（泛型实现）</summary>
        public static T Limit<T>(T n, T min, T max) where T : IComparable<T>
        {
            if (n.CompareTo(max) > 0) return max;
            if (n.CompareTo(min) < 0) return min;
            return n;
        }

        /// <summary>限制数字大小在一个区间内（Dynamic实现）</summary>
        public static dynamic Limit(dynamic n, dynamic min, dynamic max)
        {
            if (n > max) return max;
            if (n < min) return min;
            return n;
        }

        /// <summary>找到数组中最小的数</summary>
        public static int Min(params int[] data)
        {
            if (data.Length == 1) return data[0];
            if (data.Length == 2) return Math.Min(data[0], data[1]);
            int result = data[0];
            for (int i = 1; i < data.Length; i++)
            {
                if (data[i] < result)
                    result = data[i];
            }
            return result;
        }
    }
}