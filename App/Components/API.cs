using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using App.Components;
using App.DAL;

namespace App.WeiXin
{
    /// <summary>
    /// API接口清单
    /// 接口命名简明规范如下：
    /// - 动词加名词，如: GetUser, DeleteUser, EditUser, AddUser
    /// - 获取列表直接在名词后面加s，如：GetUsers，不要用GetUserList或GetUserCollection等，太累赘
    /// </summary>
    public class API
    {
        // 构建接口地址
        private static string GetUrl(Type type, string method)
        {
            return string.Format("{0}/WebCall.{1}.axd/{2}", Asp.Host, type.FullName, method);
        }

        // common
        public static string WeChatConfig           = GetUrl(typeof(WechatHelper), nameof(WechatHelper.GetJsSdkUiPackage));
        public static string SendSms                = GetUrl(typeof(DbVerifyCode), nameof(DbVerifyCode.SendSms));

        // user
        public static string Login                  = GetUrl(typeof(DbUser), nameof(DbUser.LoginMobile));
        public static string LoginDefault           = GetUrl(typeof(DbUser), nameof(DbUser.LoginDefault));
        public static string LoginByMsgCode         = GetUrl(typeof(DbUser), nameof(DbUser.LoginByMsgCode));
        public static string LogOut                 = GetUrl(typeof(DbUser), nameof(DbUser.LogoutWeChat));
        public static string GetLoginUser           = GetUrl(typeof(DbUser), nameof(DbUser.GetLoginUser));
        public static string GetUserView            = GetUrl(typeof(DbUser), nameof(DbUser.GetUserView));
        public static string BindTel                = GetUrl(typeof(DbUser), nameof(DbUser.BindTel));
        public static string EditUser               = GetUrl(typeof(DbUser), nameof(DbUser.EditUser));
        public static string EditMobile             = GetUrl(typeof(DbUser), nameof(DbUser.EditMobile));
        public static string SetPassword            = GetUrl(typeof(DbUser), nameof(DbUser.SetPassword));

    }
}