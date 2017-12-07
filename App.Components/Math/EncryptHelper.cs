using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace App.Components
{
    /// <summary>
    /// ������
    /// </summary>
    public class EncryptionHelper
    {
        /// <summary>
        /// �����ַ�����MD5��ϣֵ
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns>�ַ���MD5��ϣֵ��ʮ�������ַ���</returns>
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
        /// ��ȡ�ļ���MD5��ϣ��Ϣ
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>ʮ�������ַ���</returns>
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
