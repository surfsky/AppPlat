using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// 阿里云短信服务
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using System.Configuration;

namespace App.Components
{
    /// <summary>
    /// 阿里云的短信服务
    /// https://dysms.console.aliyun.com/dysms.htm?spm=5176.2020520142.1002.d10dysms.4b4555e3LBtR4i#/template
    /// </summary>
    public class AliSmsHelper
    {
        /// <summary>
        /// 发送注册验证码。
        /// 验证码${code}，您正在注册成为新用户，感谢您的支持！
        /// </summary>
        public static void SendSmsRegist(string number, string code)
        {
            code = string.Format("{{code:'{0}'}}", code);
            SendSms(number, SiteConfig.AliSmsRegist, code);
        }

        /// <summary>
        /// 发送登录验证码。
        /// 验证码${code}，您正在进行身份验证，打死不要告诉别人哦！
        /// </summary>
        public static void SendSmsVerify(string number, string code)
        {
            code = string.Format("{{code:'{0}'}}", code);
            SendSms(number, SiteConfig.AliSmsVerify, code);
        }

        /// <summary>
        /// 发送修改密码验证码。
        /// 验证码${code}，您正在尝试修改登录密码，请妥善保管账户信息。
        /// </summary>
        public static void SendSmsChangePassword(string number, string code)
        {
            code = string.Format("{{code:'{0}'}}", code);
            SendSms(number, SiteConfig.AliSmsChangePassword, code);
        }

        /// <summary>
        /// 发送修改信息验证码。
        /// 验证码${code}，您正在尝试变更重要信息，请妥善保管账户信息。
        /// </summary>
        public static void SendSmsChangeInfo(string number, string code)
        {
            code = string.Format("{{code:'{0}'}}", code);
            SendSms(number, SiteConfig.AliSmsChangeInfo, code);
        }

        /// <summary>
        /// 发送通用通知消息。
        /// 您收到新消息, ${title}，请及时处理。
        /// TODO: 通用通知消息未完成，请到阿里云上配置一条。
        /// </summary>
        public static void SendSmsNotify(string number, string title, string url)
        {
            title = string.Format("{{code:'{0}'}}", title);
            SendSms(number, SiteConfig.AliSmsNotify, title);
        }

        /// <summary>
        /// 发送短信（阿里云短信服务）
        /// </summary>
        /// <param name="number">手机号码</param>
        /// <param name="templateCode">短信模板编码</param>
        /// <param name="templateParam">短信模板参数</param>
        /// <param name="outId">业务方扩展字段,最终在短信回执消息中将此值带回给调用者</param>
        static void SendSms(string number, string templateCode, string templateParam, string outId = "1")
        {
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", SiteConfig.AliSmsAccessKeyId, SiteConfig.AliSmsAccessKeySecret);
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", "Dysmsapi", "dysmsapi.aliyuncs.com");
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            try
            {
                request.PhoneNumbers = number;                            // 待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为20个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                request.SignName = SiteConfig.AliSmsSignName;                 // 必填:短信签名-可在短信控制台中找到
                request.TemplateCode = templateCode;                      // 必填:短信模板-可在短信控制台中找到
                request.TemplateParam = templateParam;                    // 可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                request.OutId = outId;                                    // 可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                var sendSmsResponse = acsClient.GetAcsResponse(request);  // 请求失败这里会抛ClientException异常
                Console.WriteLine(sendSmsResponse.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}