using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace App.Components
{
    /// <summary>
    /// 二维码类别
    /// </summary>
    public enum QrCodeType : int
    {
        [UI("用户信息")] UserInfo = 0,
        [UI("课程签到")] ClassEnroll = 1,
        [UI("会籍顾问推广")] Popularize = 2
    }

    /// <summary>
    /// 二维码结构类别
    /// </summary>
    public enum QrCodeResultType : int
    {
        [UI("返回结果，地址跳转")] Url = 0,
        [UI("通知信息")] Message = 1
    }

    /// <summary>
    /// 二维码数据结构
    /// 出于通用考虑，都尽量用string类型的。
    /// </summary>
    public class QrCodeData
    {
        public QrCodeType Type { get; set; }
        public QrCodeResultType ResultType { get; set; } = QrCodeResultType.Url;
        public string Title { get; set; }
        public string ID { get; set; }
        public string Url { get; set; }
        public DateTime ExpireDt { get; set; } = DateTime.Now.AddMinutes(2);

        // 可在此类中增加加密解密逻辑
        public QrCodeData() { }

        /// <summary>构造方法</summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="type"></param>
        /// <param name="expireDt">默认2分钟后过期</param>
        public QrCodeData(string id, string title, string url, QrCodeType type, QrCodeResultType resultType = QrCodeResultType.Url, DateTime? expireDt = null)
        {
            Type = type;
            Title = title;
            ID = id;
            Url = url;
            ResultType = resultType;
            if (expireDt != null)
                ExpireDt = expireDt.ToDateTime();
        }
        public QrCodeData(string id, string title, QrCodeType type, QrCodeResultType resultType = QrCodeResultType.Message, DateTime? expireDt = null)
        {
            Type = type;
            Title = title;
            ID = id;
            ResultType = resultType;
            if (expireDt != null)
                ExpireDt = expireDt.ToDateTime();
        }

        /// <summary> 转json </summary> 
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary> 加密,经过url编码 </summary> 
        public string ToEncryptResult()
        {
            return DESEncrypt.EncryptDES(ToJson()).ToUrlEncode();
        }

        /// <summary> 解密 </summary> 
        public static QrCodeData GetDecryptResult(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<QrCodeData>(DESEncrypt.DecryptDES(json));
            }
            catch
            {
                return null;
            }
        }


    }
     
}