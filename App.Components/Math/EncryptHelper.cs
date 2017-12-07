using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace App.Components
{
    /// <summary>
    /// 加密类
    /// </summary>
    public class EncryptionHelper
    {
        /// <summary>
        /// 创建字符串的MD5哈希值
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns>字符串MD5哈希值的十六进制字符串</returns>
        public static string GetStringMD5(string inputString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(Encoding.ASCII.GetBytes(inputString));

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
                sb.AppendFormat("{0:x2}", bytes[i]);
            return sb.ToString();
        }

        /// <summary>
        /// 获取文件的MD5哈希信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>十六进制字符串</returns>
        public static string GetFileMD5(string filePath)
        {
            FileStream file = new FileStream(filePath, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(file);
            file.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
                sb.AppendFormat("{0:x2}", bytes[i]);
            return sb.ToString();
        }
    }
}
