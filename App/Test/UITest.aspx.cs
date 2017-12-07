//using App.Components;
using App.Components;
using App.DAL;
using App.WeiXin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace App.Test
{
    public partial class UITest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Sign sign = new Sign();
            //sign.Type = SignType.Customer;
            //sign.UserID =1;
            //sign.SignInDt = DateTime.Now;
            //Sign.Add(sign);
            //Response.Write(JsonConvert.SerializeObject(KeyFreeHelper.QueryEquipmentOpenHistory("13587422179",DateTime.Now.AddDays(-24), DateTime.Now)));

            //User user = App.DAL.User.GetDetail(12); ;

            //WeChatHelper.NotifyUserToVisit(user, Invite.GetDetail(3));
            //MessageHelper.NotifyUserToVisit(user, Invite.GetDetail(3));
            //WeChatHelper.NotifyUserToClass(user.OpenId, FitClass.GetDetail(2));
            //MessageHelper.NotifyUserToClass(user, FitClass.GetDetail(2));
            //WeChatHelper.NotifyUserToRecharge(user.OpenId, FitUserCard.GetDetail(5));
            //MessageHelper.NotifyUserToRecharge( FitUserCard.GetDetail(5));

            //Message msg = new Message();
            //msg.Type = MessageType.System;
            //msg.Content = "测试：撒打发点<br/>测试：撒打发点撒打发点撒打发点<br/>测试：撒打发点撒打发点<br/>测试：撒打发点撒打发点撒打发点撒打发点撒打发点撒打发点<br/>测试：撒打发点撒打发点撒打发点";
            //msg.CreateDt = DateTime.Now;
            //msg.URL = "";
            //Message.Add(msg); 
            //Response.Write(Guid.NewGuid().ToString("N"));http%3a%2f%2fwww.weibofit.com%2fWeiXin%2fWeiboFit%2fFitCoachLogin.aspx
            //Response.Write(HttpUtility.UrlEncode("http://www.lowfit.cn/WeiXin/LofFit/WeChatScan.aspx"));

            //DbUser.CoachLogin("123456789", "123456", "842776"); 
            //Response.Write(JsonConvert.SerializeObject(DbUser.CoachLogin("123456789", "123456", "842776")) );
            //Response.Write(JsonConvert.SerializeObject(KeyFreeHelper.CancelEquipmentAuthorization("139705")));
            //QrCodeData or = new QrCodeData();
            ////or.ExpireDt = DateTime.Now;
            ////or.ID = "1";
            ////or.Title = "title";
            ////or.Type = "Type
            //Response.Write(DESEncrypt.DecryptDES("x+D1FCctMq7EBQo2kbJ37iWXEtYu6az/dUOrTRf/G7AJpqBAFIaI7AnLXqRkWG8hja/mRk6UPUZ5ypqA1hpGlM6Dtj9Wcuzq1rUoDEfHprZZUm/8nCLhcgygwm4HbgibL+yh0MO2FWFFY/WvoQVioA=="));
            //Response.Write(QrCodeData.GetDecryptResult(QrCodeData.GetEncryptResult(or)).Title);
            //Response.Write(HttpUtility.UrlEncode("http://www.lowfit.cn/WeiXin/LowFit/Login.aspx?a=1"));
            KeyFreeHelper.Register("13738336151", "A头头妈咪购购购",1);
        }
    }
}