using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Data.Entity;
using Kingsoc.Web.WebCall;
using System.Data;
using System.Web.Security;
using EntityFramework.Extensions;
using App.Components;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.AdvancedAPIs;
using App.WeiXin;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;
using System.Drawing;

namespace App.DAL
{
    /// <summary>
    /// 用户相关的数据接口
    /// </summary>
    public class DbUser
    {
        //----------------------------------------------------
        // 登录和注销
        //----------------------------------------------------
        // 注销
        [WebCall("注销")]
        public static void Logout(bool redirectToLoginPage = false)
        {
            Components.AuthHelper.Logout();
            if (redirectToLoginPage)
                FormsAuthentication.RedirectToLoginPage();
        }

        [WebCall("微信端注销", AuthLogin = true)]
        public static DataResult LogoutWeChat()
        {
            var user = User.Get(Common.LoginUser.ID);
            user.WechatOpenId = string.Empty;
            user.Save();
            Components.AuthHelper.Logout();
            return new DataResult("true", "注销成功", null, null);
        }

        // Web 客户端登陆
        [WebCall("Web 客户端端登陆（含验证码）")]
        public DataResult Login(string userName, string password, string verifyCode)
        {
            // 验证验证码
            if (Asp.GetSession(Common.SESSION_VERIFYCODE) == null
                || verifyCode.ToLower() != Asp.GetSession(Common.SESSION_VERIFYCODE).ToString().ToLower())
                return new DataResult("false", "验证码错误", null, null);

            // 校验账户和密码
            var user = User.Get(name: userName);
            if (user != null && user.InUsed && PasswordHelper.ComparePasswords(user.Password, password))
            {
                LoginSuccess(user);
                Logger.LogToDb("登录成功", LogLevel.Info, userName);
                return new DataResult("true", "登录成功", null, null);
            }
            else
            {
                Logger.LogToDb("登录失败", LogLevel.Warn, userName);
                return new DataResult("false", "账户或密码错误", null, null);
            }
        }

        /// <summary>移动端登录(openId存在库中，默认登录)</summary>
        /// <param name="userToken">微信平台校验码</param> 
        public static bool LoginMobileByOpenId(string openId)
        {
            try
            {
                var user = User.GetDetail(null, null, null, openId);
                if (user != null)
                {
                    OAuthUserInfo userInfo = Asp.GetSession("OAuthUserInfo") as OAuthUserInfo;
                    if (user.NickName.IsNullOrEmpty())
                    {
                        user.NickName = userInfo.nickname;
                        user.Save();
                    }
                    if (user.Photo.IsNullOrEmpty())
                    {
                        user.Photo = userInfo.headimgurl;
                        user.Save();
                    }

                    Logout();//有没有 注销 再登录 
                    LoginSuccess(user);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogToDb(ex.Message);
            }
            return false;
        }

        /// <summary>移动端登录(openId存在库中，默认登录)测试</summary> 
        public static bool LoginMobileTest(string mobile)
        {
            User user = User.GetDetail(mobile: mobile);
            if (user != null)
            {
                Logout();//有没有 注销 再登录???
                LoginSuccess(user);
                return true;
            }
            return false;
        }

        // 登录成功：注册在线、获取角色、创建cookie验票、刷新Common.UserPowers
        static void LoginSuccess(User user, double cookieDurationHours = 12)
        {
            user = User.GetDetail(user.ID);
            user.LastLoginDt = System.DateTime.Now;
            user.Save();
            Online.RegisterOnlineUser(user.ID);

            // 将用户角色字符串保存到cookie验票里去（在Global中读取）
            string[] roles = user.Roles.Select(r => r.ToString()).ToArray();
            App.Components.AuthHelper.Login(user.Name, roles, DateTime.Now.AddHours(cookieDurationHours));
        }



        //----------------------------------------------------
        // 用户属性
        //----------------------------------------------------
        [WebCall("修改头像")]
        public static DataResult EditUserPhoto(int userId, string photo)
        {
            var user = User.Set.Where(u => u.ID == userId).FirstOrDefault();
            user.Photo = photo;
            user.Save();
            return new DataResult("true", "修改成功", null, null);
        }

        [WebCall("修改用户密码")]
        public static DataResult EditUserPassword(int userId, string oldPassword, string newPassword)
        {
            var user = User.Set.Where(u => u.ID == userId).FirstOrDefault();
            if (user == null)
                return new DataResult("false", "用户不存在", null, null);

            // 老密码和数据库校对
            if (!PasswordHelper.ComparePasswords(user.Password, oldPassword))
                return new DataResult("false", "旧密码不正确", null, null);

            // 保存新密码
            user.Password = PasswordHelper.CreateDbPassword(newPassword);
            user.Save();
            return new DataResult("true", "密码修改成功", null, null);
        }




        //----------------------------------------------------
        // 用户 & 部门
        //----------------------------------------------------
        [WebCall("根据部门编号获取用户列表", Wrap = true)]
        public static dynamic GetDeptUsers(int DeptID)
        {
            var users = User.Set.Where(u => u.Dept.ID == DeptID).Select(e => new { e.ID, e.RealName }).ToList();//&& u.CompanyID == CompanyID
            return users;
        }

        [WebCall("获取拥有某个角色的用户列表", Wrap = true)]
        public static dynamic GetUsersInRole(int RoleID)
        {
            var users = User.Set.Where(u => u.Roles.Contains((RoleType)RoleID))
                .Select(e => new { e.ID, e.NickName }).ToList();
            return users;
        }


        //------------------------------------------------
        // 微信客户端用户相关接口
        //------------------------------------------------
        [WebCall("默认登录")]
        public DataResult LoginDefault(string code)
        {
            try
            {
                var userInfo = Asp.GetSession("OAuthUserInfo") as OAuthUserInfo;
                if (userInfo == null)
                {
                    if (!code.IsNullOrEmpty())
                    {
                        var oauthResult = OAuthApi.GetAccessToken(WechatHelper.AppID, WechatHelper.AppSecret, code);
                        userInfo = OAuthApi.GetUserInfo(oauthResult.access_token, oauthResult.openid);
                        Asp.SetSession("OAuthUserInfo", userInfo);//userToken只能使用一次 获取用户信息记录到session
                        if (LoginMobileByOpenId(userInfo.openid))
                            return new DataResult("true", "登录成功", "WeChatScan.aspx", null);
                    }
                    else
                    {
                        return new DataResult("false", "未绑定微信号", "Login.aspx", null);
                    }
                }
                else
                {
                    if (LoginMobileByOpenId(userInfo.openid))
                        return new DataResult("true", "登录成功", "WeChatScan.aspx", null);
                }
            }
            catch (Exception e)
            {
                Logger.LogToDb(e.Message);
            }
            return new DataResult("false", "未绑定微信号", "Login.aspx", null);
        }

        [WebCall("绑定用户手机")]
        public DataResult BindTel(string mobile, string password, string msgCode, string inviterId)
        {
            var result = new DataResult("true", "绑定成功", null, null);
            var vCode = VerifyCode.GetDetail(mobile);
            if (vCode == null || vCode.ExpireDt < DateTime.Now)
                result = new DataResult("false", "验证码已过期", null, null);
            else if (vCode.Code != msgCode)
                result = new DataResult("false", "验证码错误", null, null);
            else
            {
                var user = App.DAL.User.Get(mobile: mobile);
                if (user != null)
                    result = new DataResult("false", "当前手机号码已被其他人使用", null, null);
                else
                {
                    //OAuthAccessTokenResult oauthResult = OAuthApi.GetAccessToken(WeChatHelper.AppID, WeChatHelper.AppSecret, userToken);
                    //OAuthUserInfo userInfo = OAuthApi.GetUserInfo(oauthResult.access_token, oauthResult.openid);
                    OAuthUserInfo userInfo = Asp.GetSession("OAuthUserInfo") as OAuthUserInfo;
                    user = new User();
                    user.Name = mobile;
                    user.Password = PasswordHelper.CreateDbPassword(password);
                    user.NickName = userInfo.nickname;
                    user.WechatOpenId = userInfo.openid;
                    user.Photo = userInfo.headimgurl;
                    user.Mobile = mobile;
                    user.Phone = mobile;
                    user.CreateDt = DateTime.Now;
                    user.InUsed = true;
                    user.Gender = userInfo.sex == 1 ? "男" : "女";
                    user.Roles = new List<RoleType>() { RoleType.Customer };
                    user.SaveNew();
                    LoginSuccess(user);

                    if (inviterId != "-1" && DESEncrypt.DecryptDES(inviterId) != inviterId)//这个解密失败，会返回传入的值
                    {
                        inviterId = DESEncrypt.DecryptDES(inviterId);
                        User inviter = User.Get(inviterId.ToInt32());
                        if (inviter != null)
                        {
                            //注册成功，才记录邀请记录
                            Invite invite = new Invite();
                            invite.InviterID = inviter.ID;
                            invite.InviteeID = user.ID;
                            invite.InviteeMobile = mobile;
                            invite.Sts = InviteStatus.New;
                            invite.Source = InviteSource.WeiXin;
                            invite.CreateDt = DateTime.Now;
                            invite.RegistDt = user.CreateDt;
                            invite.SaveNew();
                            result = new DataResult("true", "受邀,绑定成功", null, null);
                        }
                    }
                }
            }
            return result;
        }

        [WebCall("用户登录")]
        public static DataResult LoginMobile(string mobile, string password, string verifyCode, string OS)
        {
            var result = new DataResult("false", "账户或密码错误", null, null);
            if (Asp.GetSession(Common.SESSION_VERIFYCODE) == null
                || verifyCode.ToLower() != Asp.GetSession(Common.SESSION_VERIFYCODE).ToString().ToLower())
                return new DataResult("false", "验证码错误", null, null);
            else
            {
                // 校验账户和密码
                var user = User.GetDetail(null, null, mobile, null);
                var userInfo = Asp.GetSession("OAuthUserInfo") as OAuthUserInfo;
                if (user == null)
                {
                    result = new DataResult("false", "手机号码未在平台绑定", null, null);
                }
                else if (user != null && user.InUsed && PasswordHelper.ComparePasswords(user.Password, password))
                {
                    if (user.NickName.IsNullOrEmpty())
                        user.NickName = userInfo.nickname;
                    if (user.Photo.IsNullOrEmpty())
                        user.Photo = userInfo.headimgurl;
                    user.WechatOpenId = userInfo.openid;
                    LoginSuccess(user);
                    Logger.LogToDb("登录成功", LogLevel.Info, mobile, OS);
                    result = new DataResult("true", "登录成功", null, null);
                }
                else
                { Logger.LogToDb("登录失败", LogLevel.Warn, mobile, OS); }
            }
            return result;
        }

        [WebCall("通过短信登录")]
        public static DataResult LoginByMsgCode(string mobile, string msgCode)
        {
            var result = new DataResult("false", "验证码错误", null, null);
            var vCode = VerifyCode.GetDetail(mobile);
            if (vCode == null || vCode.ExpireDt < DateTime.Now || vCode.Code != msgCode)
                result = new DataResult("false", "验证码错误或过期", null, null);
            else
            {
                var user = User.GetDetail(null, null, mobile, null);
                var userInfo = Asp.GetSession("OAuthUserInfo") as OAuthUserInfo;
                if (user == null)
                    result = new DataResult("false", "手机号码未在平台绑定", null, null);
                else
                {
                    if (user.NickName.IsNullOrEmpty())
                        user.NickName = userInfo.nickname;
                    if (user.Photo.IsNullOrEmpty())
                        user.Photo = userInfo.headimgurl;

                    if (user.WechatOpenId.IsNullOrEmpty())
                        user.WechatOpenId = userInfo.openid;
                    LoginSuccess(user);
                    Logger.LogToDb("登录成功", LogLevel.Info, mobile);
                    result = new DataResult("true", "登录成功", null, null);
                }
            }
            return result;
        }

        [WebCall("获取登录用户的信息", AuthLogin = true)]
        public DataResult GetLoginUser()
        {
            var result = new DataResult("false", "当前用户未登录", null, null);
            if (Common.LoginUser != null)
            {
                var user = User.Get(name: Common.LoginUser.Name);
                result = new DataResult("true", "获取成功", new
                {
                    Name = user.Name,
                    NickName = user.NickName,
                    Email = user.Email,
                    RealName = user.RealName,
                    IdentityCard = user.IdentityCard,
                    Birthday = user.Birthday,
                    Photo = user.Photo.Replace("~", ""),
                    Mobile = user.Mobile,
                    CreateDt = user.CreateDt
                }, null);
            }

            return result;
        }

        [WebCall("修改用户信息", AuthLogin = true)]
        public DataResult EditUser(string name, string nickName, string email, string realName, string idCard, string birthday, string openId, string headImg)
        {
            var user = User.Get(name: name);
            if (user == null)
                return new DataResult("false", "无此用户", null, null);

            user.NickName = nickName;
            user.Email = email;
            user.RealName = realName;
            user.IdentityCard = idCard;
            user.Birthday = birthday.IsNullOrEmpty() ? null : (DateTime?)birthday.ToDateTime();
            if (!string.IsNullOrEmpty(headImg))
            {
                string type = headImg.Split(',')[0].Split(';')[0].Split('/')[1];
                if (type.ToUpper() == "JPG" || type.ToUpper() == "JPEG" || type.ToUpper() == "PNG")
                {
                    var ms = new MemoryStream(Convert.FromBase64String(headImg.Split(',')[1]));
                    var image = Image.FromStream(ms);
                    var path = "~/Files/Users/" + user.WechatOpenId + "." + type;
                    image.Save(HttpContext.Current.Server.MapPath(path));
                    user.Photo = path;
                }
                else
                {
                    return new DataResult("false", "请选择图片,格式.jpg/,jpeg/.png", null, null);
                }
            }
            user.Save();
            return new DataResult("true", "修改成功", null, null);
        }

        [WebCall("更换用户手机号码（账号）", AuthLogin = true)]
        public DataResult EditMobile(string newMobile, string msgCode, string password)
        {
            var result = new DataResult("false", "密码错误", null, null);
            var vCode = VerifyCode.GetDetail(newMobile);
            if (vCode == null || vCode.ExpireDt < DateTime.Now || vCode.Code != msgCode)
                result = new DataResult("false", "验证码错误或过期", null, null);
            else
            {
                if (User.Search(mobile: newMobile).Count() > 0)
                    result = new DataResult("false", "号码已使用", null, null);
                else
                {
                    var user = User.GetDetail(mobile: Common.LoginUser.Mobile);
                    if (user != null && user.InUsed && PasswordHelper.ComparePasswords(user.Password, password))
                    {
                        user.Mobile = newMobile;
                        user.Save();
                        Logout();
                        LoginSuccess(user);
                        result = new DataResult("true", "修改成功", null, null);
                    }
                }
            }
            return result;
        }

        [WebCall("忘记密码")]
        public DataResult SetPassword(string mobile, string password, string msgCode)
        {
            var result = new DataResult("false", "当前用户不存在", null, null);
            var vCode = VerifyCode.GetDetail(mobile);

            if (vCode == null || vCode.ExpireDt < DateTime.Now || vCode.Code != msgCode)
                result = new DataResult("false", "验证码错误或过期", null, null);
            else
            {
                var user = User.GetDetail(null, null, mobile, null);
                if (user != null)
                {
                    user.Password = PasswordHelper.CreateDbPassword(password);
                    user.Save();
                    result = new DataResult("true", "设置成功", null, null);
                }
            }
            return result;
        }

        [WebCall("获取用户信息，考虑跟GetLoginUser合并", AuthLogin = true)]
        public DataResult GetUserView(int userId)
        {
            var result = new DataResult("false", "找不到该用户", null, null);
            if (Common.LoginUser != null)
            {
                var user = User.Get(userId);
                result = new DataResult("true", "获取成功", new
                {
                    Name = user.Name,
                    NickName = user.NickName,
                    Email = user.Email,
                    RealName = user.RealName,
                    IdentityCard = user.IdentityCard,
                    Birthday = user.Birthday,
                    Photo = user.Photo.Replace("~", ""),
                    Mobile = user.Mobile,
                    CreateDt = user.CreateDt
                }, null);
            }
            return result;
        }
    }
}