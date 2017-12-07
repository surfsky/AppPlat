using App.Components;
using Kingsoc.Web.WebCall;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace App.DAL
{
    /// <summary>
    /// 验证码
    /// </summary>
    public class DbVerifyCode
    {
        /// <summary>
        /// 生成随机数字,暂时放这
        /// </summary>
        public  static string GetRandomNumber(int length)
        {
            string pattern = "0123456789";
            StringBuilder boundaryBuilder = new StringBuilder();
            Random rnd = new Random(GetRandomSeed());
            for (int i = 0; i < length; i++)
            {
                var index = rnd.Next(pattern.Length);
                boundaryBuilder.Append(pattern[index]);
            }
            return boundaryBuilder.ToString();
        }
        static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }



        /// <summary>
        /// 获取短信验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="source"></param>
        [WebCall("发送短信验证码")]
        public static DataResult SendSms(string mobile, int sourceType)
        {
            string source = sourceType == 1 ? "微信" : sourceType == 2 ? "安卓" : sourceType == 3 ? "苹果" : "";
            DataResult result = new DataResult("false", "获取失败", null, null);
            try
            {
                if (!string.IsNullOrEmpty(source))
                {
                    VerifyCode vCode = null;//  VerifyCode.Search(mobile, null, DateTime.Now).FirstOrDefault();
                    if (vCode != null)
                    {
                        result = new DataResult("false", "短信未过期", null, null);
                    }
                    else
                    {
                        vCode = new VerifyCode();
                        vCode.Code = GetRandomNumber(6);
                        vCode.CreateDt = DateTime.Now;
                        vCode.ExpireDt = vCode.CreateDt.AddMinutes(10);
                        vCode.Source = source;
                        vCode.Mobile = mobile; 
                        AliSmsHelper.SendSmsRegist(mobile, vCode.Code);
                        vCode.SaveNew();
                        result = new DataResult("true", "获取成功", null, null);
                    }
                }
            }
            catch (Exception e)
            {
                result = new DataResult("false", "系统异常" + e.Message, null, null);
            }
            return result;
        }
    }
}