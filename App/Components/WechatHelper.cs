using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Caching;
using App.Components;
using App.DAL;
using App.HttpApi;


namespace App.Components
{
    /// <summary>
    /// 微信辅助类库
    /// </summary>
    public partial class WechatHelper
    {
        public static string AppID { get { return SiteConfig.WechatAppID; } }           // wx6d978793fe036a69 （测试） wxd6772dcc5fca4c8c  
        public static string AppSecret { get { return SiteConfig.WechatAppSecret; } }   // e7fd6ce78e9078cef9366f521aaddfd6 （测试）         1c0de37ea891adc49cca5a25fa0a9dc6
        public static string MchId { get { return SiteConfig.WechatMchId; } }           // 商户ID
        public static string MchKey { get { return SiteConfig.WechatMchKey; } }         // 商户平台设置的密钥key 
        private static string UrlPayNotify { get { return SiteConfig.WechatUrlPay; } }  // 支付通知回调页面
        public static string AccesToken
        {
            get
            {
                if (!AccessTokenContainer.CheckRegistered(AppID))
                    AccessTokenContainer.Register(AppID, AppSecret, "_AccesToken");
                var tokenResult = AccessTokenContainer.GetAccessTokenResult(AppID);
                return tokenResult.access_token;
            }
        }


        //---------------------------------------------------------------------------------------------------------------
        // 接口（建议移到其他文件去）
        //---------------------------------------------------------------------------------------------------------------
        [HttpApi("获取微信JS-SDK凭证", Wrap = true)]
        public static JsSdkUiPackage GetJsSdkUiPackage(string url)
        {
            //webCall 传的url 不知道为什么会有   ,/HttpApi.App.WeiXin.WeChatHelper.axd   带这么一串
            url = url.Split(',')[0];
            try
            {
                return JSSDKHelper.GetJsSdkUiPackage(AppID, AppSecret, url);
                //return new JsSdkUiPackage("", "", "", "");
            }
            catch
            {
                return new JsSdkUiPackage("", "", "", "");
            }
        }


        //---------------------------------------------------------------------------------------------------------------
        // 支付相关
        //---------------------------------------------------------------------------------------------------------------
        /// <summary>微信支付-预支付订单</summary>
        public static UnifiedorderResult Pay(string body, double price, string openId, string orderId, string ip, string nonceStr)
        {
            var data = new TenPayV3UnifiedorderRequestData(
                AppID, MchId, body,
                orderId, Convert.ToInt32(price * 100), ip,
                UrlPayNotify, TenPayV3Type.JSAPI,
                openId, MchKey, nonceStr);
            return TenPayV3.Unifiedorder(data);
        }

        /// <summary>微信支付-获取微信支付签名</summary>
        public static string GetPaySign(string nonceStr, string timeStamp, string package)
        {
            return TenPayV3.GetJsPaySign(AppID, timeStamp, nonceStr, package, MchKey);
        }



    }
}