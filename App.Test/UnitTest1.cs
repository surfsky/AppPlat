using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using App.DAL;
using App.Components;

namespace App.Test
{
    [TestClass]
    public class UnitTest1
    {
        // 测试阿里云短信接口（用代码发送只收到第一条短信，单步调试没问题）
        [TestMethod]
        public void TestSms()
        {
            AliSmsHelper.SendSmsRegist("15305770121", "12345");
            System.Threading.Thread.Sleep(3000);
            AliSmsHelper.SendSmsVerify("15305770121", "12345");
            System.Threading.Thread.Sleep(3000);
            AliSmsHelper.SendSmsChangePassword("15305770121", "12345");
            System.Threading.Thread.Sleep(3000);
            AliSmsHelper.SendSmsChangeInfo("15305770121", "12345");
        }

        [TestMethod]
        public void TestImage()
        {
            var img = HttpHelper.GetThumbnail("https://ss0.bdstatic.com/5aV1bjqh_Q23odCf/static/superman/img/logo_top_ca79a146.png", 20, 20);
            img = HttpHelper.GetThumbnail("../Res/images/defaultUser.png", 20, 20);
        }
    }
}
