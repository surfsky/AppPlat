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
    /// 微信菜单
    /// </summary>
    public class WechatMenu
    {
        public List<WechatButton> button { get; set; } = new List<WechatButton>();
    }

    /// <summary>
    /// 微信菜单按钮
    /// </summary>
    public class WechatButton
    {
        public string name { get; set; }
        public WechatButtonType type { get; set; }
        public string url { get; set; }
        public string appid { get; set; }
        public string key { get; set; }
        public List<WechatButton> sub_button { get; set; } = new List<WechatButton>();

        public WechatButton() { }
        public WechatButton(string name, WechatButtonType type, string url)
        {
            this.name = name;
            this.type = type;
            this.url = url;
        }
    }

    /// <summary>
    /// 按钮类型
    /// </summary>
    public enum WechatButtonType
    {
        click,
        view,
        scancode_push,
        scancode_waitmsg,
        pic_sysphoto,
        pic_photo_or_album,
        pic_weixin,
        location_select,
        media_id,
        view_limited
    }


    /// <summary>
    /// 发送微信模板消息
    /// </summary>
    public partial class WechatHelper
    {
        /// <summary>设置微信菜单</summary>
        public static string SetMenu(WechatMenu menu)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", WechatHelper.AccesToken);
            return HttpHelper.PostJson(url, JsonConvert.SerializeObject(menu));
        }
    }
}