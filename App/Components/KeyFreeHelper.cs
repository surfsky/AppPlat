using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Caching;
using App.Components;
using Newtonsoft.Json;

/// <summary>
/// 智能锁接口方法
/// </summary>
namespace App.Components
{
    /// <summary>
    /// 智能锁用户
    /// </summary>
    class KeyFreeUser
    {
        /// <summary>公司id, 必填</summary>
        public int cmpId { get; set; }
        /// <summary>运营公司id, 必填</summary>
        public int proId { get; set; }
        /// <summary>人员划入对应的部门id, 必填</summary>
        public int deptId { get; set; }
        /// <summary>人员划入对应的部门id, 必填</summary>
        public int memberId { get; set; }
        /// <summary>性别， 0：男， 1：女</summary>
        public int gender { get; set; } = 0;
        /// <summary>人员所在楼层， 可选项</summary>
        public int floor { get; set; } = 0;
        /// <summary>地址信息， 可选项</summary>
        public string address { get; set; } = "";

        //--------------------------------------------------
        //注册人员API  参数
        //--------------------------------------------------
        /// <summary>手机号码, 必填</summary>
        public string phone { get; set; }
        /// <summary>人员群组id,可选项</summary>
        public string groupId { get; set; } = "";
        /// <summary>人员名称,可选项</summary>
        public string realName { get; set; } = "";


        //--------------------------------------------------
        //修改人员API  参数
        //-------------------------------------------------- 
        /// <summary>是否停用,（false为不停用,true为停用）,选填</summary>
        public bool disabled { get; set; } = true;
        /// <summary>是否受访,（flase为不受访,true为受访）,选填</summary>
        public bool visitor { get; set; } = false;
        /// <summary>人员组id数组,选填</summary>
        public int[] groupIds { get; set; } = null;
        /// <summary>角色id数组,选填</summary>
        public int[] roleIds { get; set; } = null;
        /// <summary>人员名称,可选项</summary>
        public string realname { get; set; } = "";
    } 

    /// <summary>
    /// 接口返回值
    /// </summary>
    public class KeyFreeResult
    {
        public int result { get; set; }
        public string resultInfo { get; set; }
        public object data { get; set; }
    }


    public class OpenHistoryResult
    {
        public int id { get; set; }
        public int deviceId { get; set; }
        public string deviceCode { get; set; }    // 88ACD160F7C3
        public string deviceName { get; set; }    // 亦美大厦（门禁）
        public int memberId { get; set; }
        public string memberName { get; set; }    // 独家记忆
        public string visitorId { get; set; }
        public string visitorName { get; set; }
        public int operateType { get; set; }
        public int proId { get; set; }
        public int cmpId { get; set; }
        public string cmpName { get; set; }      // 大南帝配健身房
        public string appId { get; set; }        // wxd7cdcffb3cf54e73
        public string phoneNo { get; set; }      // 13587422179
        public string openId { get; set; }       // odWygv0PgRRP8AeHuq76WRIcU_nQ
        public int idType { get; set; }
        public int eventType { get; set; }
        public int status { get; set; }
        public string operateTime { get; set; }
        public string eventTime { get; set; }
        public string serial { get; set; }
        public string operateName { get; set; }
    }

    /// <summary>
    /// 指纹锁接口方法
    /// 如果需要确定用户是否开门成功，必须去KeyFree平台查询用户的操作记录
    /// </summary>
    public class KeyFreeHelper
    {
        public static string Host { get { return "http://key1.cn/reformer/member"; } }
        public static string AppKey { get { return "oy3x7gcisgosqad8fcih5e9w"; } }
        public static string AppSecret { get { return "6p7g43b06jz8qdjmmdgsvysw4evsxadh"; } }
        public static int CmpId { get { return 953; } }    // 公司Id
        public static int ProId { get { return 318; } }    // 运营公司Id
        public static int DeptId { get { return 5254; } }  // 部门Id
        public static string RuleId = "2203";              // 规则id
        public static string DeviceGroupNo = "C953G1010";  // 设备组编号

        // TODO: 可以用 Common.GetCacheData() 方法简化
        private static Cache Cache = HttpRuntime.Cache;    // 缓存，不用多次生成token
        private static string _accesToken;
        public static string AccesToken
        {
            get
            {
                if (Cache["KeyFreeAccesToken"].IsNullOrEmpty())
                    _accesToken = InitAccesToken();
                return _accesToken;
            }
        }

        private static string InitAccesToken()
        {
            var type = "2";
            var life = "3600";//有效时间60分钟 
            var dateStart = new DateTime(1970, 1, 1, 8, 0, 0);
            int timeStamp = Convert.ToInt32((DateTime.Now.AddSeconds(3600) - dateStart).TotalSeconds);
            var expires = timeStamp.ToString() + "000";//过期时间点 

            // TODO: 以下的 Append() 尽量改为 String.Format
            var builder = new StringBuilder();
            builder.Append(type).Append(life).Append(expires).Append(ProId).Append(AppKey).Append(AppSecret);
            var sign = GetSHA256HashFromString(builder.ToString());
            var token = new StringBuilder();
            token.Append(AppKey).Append("|")
                .Append(type).Append(".")
                .Append(sign).Append(".")
                .Append(life).Append(".")
                .Append(expires).Append("-")
                .Append(ProId)
                ;
            Cache.Insert("KeyFreeAccesToken", token.ToString(), null, DateTime.Now.AddSeconds(3500), TimeSpan.Zero);
            return Cache["KeyFreeAccesToken"].ToString();
        }

        /// <summary>sha256加密  位数不对64</summary>
        /// <param name="data"></param>
        private static string GetSHA256HashFromString(string data)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(data);
            try
            {
                var sha256 = new SHA256CryptoServiceProvider();
                var hash = sha256.ComputeHash(bytes);
                var sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetSHA256HashFromString() fail, error: " + ex.Message);
            }
        }


        //------------------------------------------------------------------------
        // KeyFree Api调用
        //------------------------------------------------------------------------
        /// <summary>注册人员Api</summary> 
        /// <param name="phone">手机号码</param>
        /// <param name="realName">真实姓名</param> 
        /// <param name="gender">性别， 0：男， 1：女</param>
        public static KeyFreeResult Register(string phone, string realName, int gender)
        {
            var r = new KeyFreeResult();
            r.result = 500;
            r.resultInfo = "-1";
            for (int i = 0; i < 5; i++)
            {
                if (!(r.result == 200 || r.result == 201))
                {
                    var url = string.Format("{0}/api/member/register", Host);
                    var user = new KeyFreeUser();
                    user.cmpId = CmpId;
                    user.deptId = DeptId;
                    user.proId = ProId;
                    user.phone = phone;
                    user.realName = phone;//realName不用了 直接传phone
                    user.gender = gender;
                    var dict = new Dictionary<string, string>();
                    dict.Add("data", JsonConvert.SerializeObject(user));
                    dict.Add("accessToken", AccesToken);
                    try
                    {
                        r = JsonConvert.DeserializeObject<KeyFreeResult>(HttpHelper.Post(url, dict));
                        Logger.LogToDb(string.Format("门禁系统：注册成功。Result：{0},JsonData：{1}", JsonConvert.SerializeObject(r), JsonConvert.SerializeObject(user)));
                        break;
                    }
                    catch (Exception e)
                    {
                        Logger.LogToDb(string.Format("门禁系统：注册失败。Message：{0}，JsonData：{1}", e.Message, JsonConvert.SerializeObject(user)));
                        r.result = 500;
                        r.resultInfo = "-1";
                    }
                }
            }
            return r;
        }

        /// <summary>修改人员</summary>
        /// <param name="memberId">人员id,必填</param>
        /// <param name="realname">真实姓名,必填</param>
        /// <param name="gender">性别,（男是0,女是1）,选填</param>
        /// <param name="address">地址,选填</param>
        /// <param name="floor">楼层,选填</param>
        /// <param name="disabled">是否停用,（false为不停用,true为停用）,选填</param>
        /// <param name="visitor">是否受访,（flase为不受访,true为受访）,选填</param>
        /// <param name="groupIds">人员组id数组,选填</param>
        /// <param name="roleIds">角色id数组,选填</param>
        public static KeyFreeResult UpdateById(string memberId, string realname, int gender = -1, string address = "", int floor = -1, bool disabled = false, bool visitor = false, int[] groupIds = null, int[] roleIds = null)
        {
            var r = new KeyFreeResult();
            r.result = 500;
            r.resultInfo = "-1";
            for (int i = 0; i < 5; i++)
            {
                if (!(r.result == 200 || r.result == 201))
                {
                    try
                    {
                        var url = string.Format("{0}/api/member/updateById", Host);
                        var user = new KeyFreeUser();
                        user.memberId = int.Parse(memberId);
                        user.cmpId = CmpId;
                        user.deptId = DeptId;
                        user.proId = ProId;
                        user.realname = realname;
                        user.address = address;
                        user.disabled = disabled;
                        user.visitor = visitor;
                        user.groupIds = groupIds;
                        user.roleIds = roleIds;
                        if (gender != -1) user.gender = gender;
                        if (floor != -1) user.floor = floor;
                        var dict = new Dictionary<string, string>();
                        dict.Add("data", JsonConvert.SerializeObject(user));
                        dict.Add("accessToken", AccesToken);
                        r = JsonConvert.DeserializeObject<KeyFreeResult>(HttpHelper.Post(url, dict));
                        break;
                    }
                    catch
                    {
                        r.result = 500;
                        r.resultInfo = "-1";
                    }
                }
            }
            return r;
        }

        /// <summary>删除人员</summary>
        /// <param name="memberId">人员Id</param>
        public static KeyFreeResult DeleteById(string memberId)
        {
            var r = new KeyFreeResult();
            r.result = 500;
            r.resultInfo = "-1";
            for (int i = 0; i < 5; i++)
            {
                if (!(r.result == 200 || r.result == 201))
                {
                    try
                    {
                        var url = string.Format("{0}/api/member/deleteById", Host);
                        var dict = new Dictionary<string, string>();
                        var user = new KeyFreeUser();
                        user.memberId = int.Parse(memberId);
                        user.proId = ProId;
                        user.cmpId = CmpId;
                        dict.Add("data", JsonConvert.SerializeObject(user));
                        dict.Add("accessToken", AccesToken);
                        r = JsonConvert.DeserializeObject<KeyFreeResult>(HttpHelper.Post(url, dict));
                        break;
                    }
                    catch
                    {
                        r.result = 500;
                        r.resultInfo = "-1";
                    }
                }
            }
            return r;
        }

        /// <summary>获取用户信息（输出json字符串，未反，需要时再做）</summary>
        /// <param name="memberId">KeyFree中的用户Id</param>
        public static KeyFreeResult QueryById(string memberId)
        {
            var r = new KeyFreeResult();
            r.result = 500;
            r.resultInfo = "-1";
            for (int i = 0; i < 5; i++)
            {
                if (!(r.result == 200 || r.result == 201))
                {
                    try
                    {
                        var url = string.Format("{0}/api/member/queryById", Host);
                        var dict = new Dictionary<string, string>();
                        var user = new KeyFreeUser();
                        user.memberId = int.Parse(memberId);
                        user.proId = ProId;
                        user.cmpId = CmpId;
                        dict.Add("data", JsonConvert.SerializeObject(user));
                        dict.Add("accessToken", AccesToken);
                        r = JsonConvert.DeserializeObject<KeyFreeResult>(HttpHelper.Post(url, dict, cookieContainer: null));
                        break;
                    }
                    catch
                    {
                        r.result = 500;
                        r.resultInfo = "-1";
                    }
                }
            }
            return r;
        }

        /// <summary>获取用户信息（输出json字符串，未反，需要时再做）</summary>
        /// <param name="phone">KeyFree中的用户联系电话（Phone字段）</param>
        public static KeyFreeResult QueryByPhone(string phone)
        {
            var r = new KeyFreeResult();
            r.result = 500;
            r.resultInfo = "-1";
            for (int i = 0; i < 5; i++)
            {
                if (!(r.result == 200 || r.result == 201))
                {
                    try
                    {
                        var url = string.Format("{0}/api/member/queryByPhone", Host);
                        var dict = new Dictionary<string, string>();
                        var user = new KeyFreeUser();
                        user.phone = phone;
                        user.proId = ProId;
                        user.cmpId = CmpId;
                        dict.Add("data", JsonConvert.SerializeObject(user));
                        dict.Add("accessToken", AccesToken);
                        r = JsonConvert.DeserializeObject<KeyFreeResult>(HttpHelper.Post(url, dict));
                        break;
                    }
                    catch
                    {
                        r.result = 500;
                        r.resultInfo = "-1";
                    }
                }
            }
            return r;
        }

        /// <summary>查询设备开启记录</summary>
        /// <param name="phone">手机号码  必</param>
        /// <param name="mac">设备mac 选</param>
        /// <param name="startTime">必</param>
        /// <param name="endTime">必</param>
        public static KeyFreeResult QueryEquipmentOpenHistory(string phone, DateTime startTime, DateTime endTime)
        {
            var r = new KeyFreeResult();
            r.result = 500;
            r.resultInfo = "-1";
            for (int i = 0; i < 5; i++)
            {
                if (!(r.result == 200 || r.result == 201))
                {
                    try
                    {
                        var url = string.Format("{0}/api/openHistory/query", Host);
                        var dict = new Dictionary<string, string>();
                        var json = string.Format("{{\"cmpId\":{0},\"proId\":{1},\"phone\":\"{2}\",\"mac\":\"{3}\",\"startTime\":\"{4}\",\"endTime\":\"{5}\"}}", CmpId, ProId, phone, "", startTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        dict.Add("data", json);
                        dict.Add("accessToken", AccesToken);
                        r = JsonConvert.DeserializeObject<KeyFreeResult>(HttpHelper.Post(url, dict));
                        break;
                    }
                    catch (Exception e)
                    {
                        r.result = 500;
                        r.resultInfo = "-1";
                        r.data = e.Message;
                    }
                }
            }
            return r;
        }

        /// <summary>人员设备授权</summary>
        /// <param name="phone"></param>
        /// <param name="mac"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public static KeyFreeResult EquipmentAuthorization(string memberId, DateTime startTime)
        {
            var r = new KeyFreeResult();
            r.result = 500;
            r.resultInfo = "-1";
            for (int i = 0; i < 5; i++)
            {
                if (!(r.result == 200 || r.result == 201))
                {
                    var url = string.Format("{0}/api/auth/grant", Host);
                    var dict = new Dictionary<string, string>();
                    var json = string.Format("{{\"deviceCode\":\"\",\"deviceGroupNo\":\"{0}\",\"memberId\":{1},\"ruleId\":{2},\"startDate\":\"{3}\",\"endDate\":\"{4}\"}}", DeviceGroupNo, memberId, RuleId, startTime.ToString("yyyy-MM-dd HH:mm"), startTime.AddYears(10).ToString("yyyy-MM-dd HH:mm"));
                    dict.Add("data", json);
                    dict.Add("accessToken", AccesToken);
                    try
                    {
                        r = JsonConvert.DeserializeObject<KeyFreeResult>(HttpHelper.Post(url, dict));
                        Logger.LogToDb(string.Format("门禁系统：授权成功。Result：{0},JsonData：{1}", JsonConvert.SerializeObject(r), json));
                        break;
                    }
                    catch (Exception ex)
                    {
                        r.result = 500;
                        r.resultInfo = "-1";
                        r.data = ex.Message;
                        Logger.LogToDb(string.Format("门禁系统：授权失败。Result：{0},JsonData：{1}", ex.Message, json));
                    }
                }
            }
            return r;
        }

        /// <summary>取消授权</summary>
        /// <param name="memberId"></param>
        public static KeyFreeResult CancelEquipmentAuthorization(string memberId)
        {
            var r = new KeyFreeResult();
            r.result = 500;
            r.resultInfo = "-1";
            for (int i = 0; i < 5; i++)
            {
                if (!(r.result == 200 || r.result == 201))
                {
                    try
                    {
                        var url = string.Format("{0}/api/auth/cancelGrant", Host);
                        var dict = new Dictionary<string, string>();
                        var json = string.Format("{{\"deviceCode\":\"\",\"deviceGroupNo\":\"{0}\",\"memberId\":{1},\"ruleId\":{2}}}", DeviceGroupNo, memberId, RuleId);
                        dict.Add("data", json);
                        dict.Add("accessToken", AccesToken);
                        r = JsonConvert.DeserializeObject<KeyFreeResult>(HttpHelper.Post(url, dict));
                        break;
                    }
                    catch (Exception ex)
                    {
                        r.result = 500;
                        r.resultInfo = "-1";
                        r.data = ex.Message;
                    }
                }
            }
            return r;
        }

        /// <summary>调用微信扫一扫</summary>
        /// <param name="openId"></param>
        /// <param name="phone"></param>
        public static void WeChatScan(System.Web.UI.Page page, string openId, string phone)
        {
            var url = string.Format("{0}/wx/wxscan3rd.shtml", Host);
            var dict = new Dictionary<string, string>();
            var json = string.Format("{{\"cmpId\":{0},\"phone\":\"{1}\",\"openId\":\"{2}\"}}", CmpId, phone, openId);
            dict.Add("data", json);
            dict.Add("accessToken", AccesToken);
            Asp.CreateFormAndPost(page, url, dict);
        }

        /// <summary>远程开门</summary>
        /// <param name="mac"></param>
        public static KeyFreeResult OpenDoor(string mac)
        {
            var r = new KeyFreeResult();
            r.result = 500;
            r.resultInfo = "-1";
            for (int i = 0; i < 5; i++)
            {
                if (!(r.result == 200 || r.result == 201))
                {
                    try
                    {
                        var url = string.Format("{0}/api/remote/openDoor", Host);
                        var dict = new Dictionary<string, string>();
                        var json = string.Format("{{\"cmpId\":{0},\"proId\":{1},\"mac\":\"{2}\"}}", CmpId, ProId, mac);
                        dict.Add("data", json);
                        dict.Add("accessToken", AccesToken);
                        r = JsonConvert.DeserializeObject<KeyFreeResult>(HttpHelper.Post(url, dict));
                        break;
                    }
                    catch (Exception ex)
                    {
                        r.result = 500;
                        r.resultInfo = "-1";
                        r.data = ex.Message;
                    }
                }
            }
            return r;
        }
    }
}



