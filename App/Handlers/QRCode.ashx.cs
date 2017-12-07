using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using ThoughtWorks.QRCode.Codec;
using App.Components;
using System.Web.SessionState;

namespace App.Handlers
{
    /// <summary>
    /// 二维码生成器
    /// http://localhost:5625/handlers/QRCode.ashx?value=helloworld&icon=/res/images/defaultuser.png
    /// http://localhost:5625/handlers/QRCode.ashx?value=helloworld&icon=https://ss0.bdstatic.com/5aV1bjqh_Q23odCf/static/superman/img/logo/bd_logo1_31bdc765.png
    /// </summary>
    public class QRCode : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable
        {
            get { return false; }
        }


        public void ProcessRequest(HttpContext context)
        {
            String value = context.Request["value"];
            string iconUrl = context.Request["icon"];

            // 二维码图片
            QRCodeEncoder encoder = new QRCodeEncoder();
            encoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE; //编码方法
            encoder.QRCodeScale = 24;//大小
            encoder.QRCodeVersion = 8;//版本
            encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            Bitmap bmp = encoder.Encode(value.ToString(), Encoding.UTF8);

            // 正中央放置小图标
            if (!iconUrl.IsNullOrEmpty())
            {
                int s = bmp.Width / 5;
                Image icon = HttpHelper.GetServerOrNetworkImage(iconUrl);
                icon = DrawHelper.CreateThumbnail(icon, s, s);
                var point = new Point((bmp.Width - s) / 2, (bmp.Height - s) / 2);
                bmp = DrawHelper.MergeImage(bmp, (Bitmap)icon, 0.95f, point);
                icon.Dispose();
            }

            // 输出图片
            HttpHelper.SetCache(context, 600);
            HttpHelper.WriteImage(bmp, "");
        }

    }
}