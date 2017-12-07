using App.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.SessionState;

namespace App.Handlers
{
    /// <summary>
    /// 验证码
    /// 更复杂的验证码可参考：
    /// 三维验证码：https://www.cnblogs.com/Aimeast/archive/2011/05/02/2034525.html
    /// 空心字验证码：http://blog.51cto.com/xclub/1597200
    /// </summary>
    public class VerifyCode : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable
        {
            get { return false; }
        }
        
        public void ProcessRequest(HttpContext context)
        {
            int codeW = 80;
            int codeH = 40;
            int fontSize = 18;

            // 颜色列表，用于验证码、噪线、噪点 
            // 字体列表，用于验证码 
            // 验证码的字符集，去掉了一些容易混淆的字符 
            Color[] color = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };
            string[] font = { "Times New Roman", "Verdana", "Arial", "Gungsuh", "Impact" };
            char[] character = { '2', '3', '4', '5', '6', '8', '9', 'a', 'b', 'd', 'e', 'f', 'h', 'k', 'm', 'n', 'r', 'x', 'y', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };

            // 生成验证码字符串 
            // 写入Session
            Random rnd = new Random();
            string chkCode = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                chkCode += character[rnd.Next(character.Length)];
            }
            context.Session[Common.SESSION_VERIFYCODE] = chkCode;

            // 创建画布
            Bitmap bmp = new Bitmap(codeW, codeH);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            // 画噪线 
            for (int i = 0; i < 2; i++)
            {
                int x1 = rnd.Next(codeW);
                int y1 = rnd.Next(codeH);
                int x2 = rnd.Next(codeW);
                int y2 = rnd.Next(codeH);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawLine(new Pen(clr), x1, y1, x2, y2);
            }

            // 画验证码字符串 
            for (int i = 0; i < chkCode.Length; i++)
            {
                string fnt = font[rnd.Next(font.Length)];
                Font ft = new Font(fnt, fontSize);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawString(chkCode[i].ToString(), ft, new SolidBrush(clr), (float)i * 18 + 2, (float)5);
            }

            // 画噪点 
            for (int i = 0; i < 100; i++)
            {
                int x = rnd.Next(bmp.Width);
                int y = rnd.Next(bmp.Height);
                Color clr = color[rnd.Next(color.Length)];
                bmp.SetPixel(x, y, clr);
            }

            // 扭曲
            bmp = DrawHelper.TwistImage(bmp);

            // 清除该页输出缓存，设置该页无缓存 
            HttpHelper.SetCache(context, cacheLocation: HttpCacheability.NoCache);
            HttpHelper.WriteImage(bmp);
            bmp.Dispose();
            g.Dispose();
        }
    }
}