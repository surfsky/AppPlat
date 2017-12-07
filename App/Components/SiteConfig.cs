using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.DAL;
using System.Configuration;
using System.Drawing;
using EntityFramework.Extensions;
using App.Components;

namespace App
{
    /// <summary>
    /// 网站配置信息
    /// </summary>
    public class SiteConfig
    {
        //-------------------------------------------
        // 预设值
        //-------------------------------------------
        // 图片路径及尺寸设置
        public static string DefaultUserImage = "~/Res/Images/defaultUser.png";
        public static Size SizeUserImage = new Size(200, 200);
        public static Size SizeFaceImage = new Size(800, 800);


        //-------------------------------------------
        // 以下数据来自web.config文件（仅程序员可更改）
        //-------------------------------------------
        // 站点相关
        public static string Site = ConfigurationManager.AppSettings["Site"];
        public static string SiteDomain = ConfigurationManager.AppSettings["SiteDomain"];
        public static string SiteICP = ConfigurationManager.AppSettings["SiteICP"];

        // 阿里短信 
        public static string AliSmsSignName = ConfigurationManager.AppSettings["AliSmsSignName"];
        public static string AliSmsAccessKeyId = ConfigurationManager.AppSettings["AliSmsAccessKeyId"];
        public static string AliSmsAccessKeySecret = ConfigurationManager.AppSettings["AliSmsAccessKeySecret"];
        public static string AliSmsRegist = ConfigurationManager.AppSettings["AliSmsRegist"];
        public static string AliSmsVerify = ConfigurationManager.AppSettings["AliSmsVerify"];
        public static string AliSmsChangePassword = ConfigurationManager.AppSettings["AliSmsChangePassword"];
        public static string AliSmsChangeInfo = ConfigurationManager.AppSettings["AliSmsChangeInfo"];
        public static string AliSmsNotify = ConfigurationManager.AppSettings["AliSmsNotify"];

        //微信公众号
        public static string WechatAppID = ConfigurationManager.AppSettings["WechatAppID"]; //wx6d978793fe036a69 （测试）   
        public static string WechatAppSecret = ConfigurationManager.AppSettings["WechatAppSecret"]; //e7fd6ce78e9078cef9366f521aaddfd6 （测试） 
        public static string WechatMchId = ConfigurationManager.AppSettings["WechatMchId"];  //微信商户号
        public static string WechatMchKey = ConfigurationManager.AppSettings["WechatMchKey"]; //商户平台设置的密钥key
        public static string WechatUrlPay = ConfigurationManager.AppSettings["WechatUrlPay"]; // 微信支付成功回调地址
        public static string WechatMsgVisit = ConfigurationManager.AppSettings["WechatMsgVisit"]; // 微信邀请信息模板id


        //-------------------------------------------
        // 以下信息保存在数据库中（用户可更改）
        //-------------------------------------------
        public static string SiteTitle { get; set; }
        public static int PageSize { get; set; }
        public static string HelpList { get; set; }
        public static string MenuType { get; set; }
        public static string Theme { get; set; }
        public static string DefaultPassword { get; set; }

        //
        static SiteConfig()
        {
            Load();
        }

        public static void Load()
        {
            SiteTitle = Config.GetValue("Site", "Title");
            PageSize = Config.GetValue("Site", "PageSize").ToInt32();
            HelpList = Config.GetValue("Site", "HelpList");
            MenuType = Config.GetValue("Site", "MenuType");
            Theme = Config.GetValue("Site", "Theme");
            DefaultPassword = Config.GetValue("Site", "DefaultPassword");
        }

        public static void Save()
        {
            Config.SetValue("Site", "SiteTitle", SiteTitle);
            Config.SetValue("Site", "PageSize", PageSize.ToString());
            Config.SetValue("Site", "HelpList", HelpList);
            Config.SetValue("Site", "MenuType", MenuType);
            Config.SetValue("Site", "Theme", Theme);
            Config.SetValue("Site", "DefaultPassword", DefaultPassword);
        }


        //-------------------------------------------
        // 枚举信息也保存在 Config 表中
        //-------------------------------------------
        /// <summary>清除枚举数据</summary>
        public static void ClearEnumData()
        {
            Config.Set
                .Where(t => t.Category != "Site")
                .Where(t => t.Category != "AppBanner")
                .Delete();
        }


        /// <summary>将枚举值加入，作为数据字典</summary>
        public static void AddEnum(Type enumType)
        {
            var category = enumType.Name;
            foreach (object value in Enum.GetValues(enumType))
            {
                var id = (int)value;
                var name = value.GetDescription();
                var cfg = new Config() { Category = category, Key = value.ToString(), Value = id.ToString(), Title = name };
                cfg.Save(false);
            }
        }


    }
}