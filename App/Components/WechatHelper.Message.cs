using App.Components;
using App.DAL;
using App.HttpApi;
using Newtonsoft.Json;
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

namespace App.Components
{
    /// <summary>
    /// 简化的微信模板消息结构
    /// </summary>
    public class WechatMessage
    {
        public TemplateDataItem first { get; set; }
        public TemplateDataItem reamrk { get; set; }
        public TemplateDataItem keyword1 { get; set; }
        public TemplateDataItem keyword2 { get; set; }
        public TemplateDataItem keyword3 { get; set; }
        public TemplateDataItem keyword4 { get; set; }

        public WechatMessage(string first, string remark, string keyword1 = "", string keyword2 = "", string keyword3 = "", string keyword4 = "")
        {
            this.first = new TemplateDataItem(first);
            this.reamrk = new TemplateDataItem(remark);
            this.keyword1 = new TemplateDataItem(keyword1);
            this.keyword2 = new TemplateDataItem(keyword2);
            this.keyword3 = new TemplateDataItem(keyword3);
            this.keyword4 = new TemplateDataItem(keyword4);
        }
    }

    /// <summary>
    /// 创建微信菜单
    /// https://mp.weixin.qq.com/wiki?t=resource/res_main&id=mp1421141013
    /// https://www.cnblogs.com/mchina/p/3276878.html
    /// </summary>
    public partial class WechatHelper
    {
        /// <summary>发送微信模板消息</summary>
        public static void SendMessage(string openId, string templateId, string url, object data)
        {
            try
            {
                TemplateApi.SendTemplateMessage(AccesToken, openId, templateId, url, data);
            }
            catch (Exception ex)
            {
                string txt = string.Format("微信发射失败：{0}", ex.Message);
                Logger.LogToDb(txt, LogLevel.Error);
            }
        }

        //---------------------------------------------------------------------------------------------------------------
        // 微信模板消息
        //---------------------------------------------------------------------------------------------------------------
        /// <summary>预约提醒</summary> 
        public static void NotifyUserToVisit(User user, Invite invite, string url)
        {
            var data = new WechatMessage(
                "您有一条预约，别忘记了哦~",
                "感谢您的预约。",
                user.NickName,
                user.Mobile,   //"#ff0000"
                string.Format("{0:yyyy-MM-dd HH:mm}", invite.AppointmentDt),
                "预约/拜访");
            WechatHelper.SendMessage(user.WechatOpenId, SiteConfig.WechatMsgVisit, url, data);
        }
    }

    
}