using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing.Drawing2D;

namespace App.Components
{
    /// <summary>
    /// 绘图相关辅助方法
    /// </summary>
    public class DrawHelper
    {
        /// <summary>
        /// 加载图片。如果用Image.FromFile()方法的话会锁定图片，无法编辑、移动、删除。
        /// </summary>
        public static Image LoadImage(string path)
        {
            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            return Image.FromStream(fs);
        }

        /// <summary>绘制缩略图</summary>
        public static void CreateThumbnail(string sourceImagePath, string targetImagePath, int width, int height=-1)
        {
            string savePath = targetImagePath.IsNullOrEmpty() ? sourceImagePath : targetImagePath;
            Image img = Image.FromFile(sourceImagePath);
            Image bmp = CreateThumbnail(img, width, height);
            img.Dispose();
            bmp.Save(savePath);
            bmp.Dispose();
        }

        /// <summary>创建缩略图</summary>
        public static Image CreateThumbnail(Image img, int width, int height=-1)
        {
            if (img == null) return null;
            // 计算图片的尺寸
            if (height == -1)
                height = img.Height * width / img.Width;

            // 绘制Bitmap新实例
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Transparent);
            g.DrawImage(img, new Rectangle(0, 0, width, height));
            g.Dispose();

            return bmp;
        }

        /// <summary>
        /// 合并两张图片。第二张图片可指定不透明度以及粘贴位置。
        /// 注意 img 和 img2 在本函数中都没有释放，请自行Dispose。
        /// </summary>
        public static Bitmap MergeImage(Bitmap img, Bitmap img2, float opacity, params Point[] points)
        {
            if (img == null || img2 == null)
                return null;

            // 创建一个图像用于最后输出
            Bitmap bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            g.InterpolationMode = InterpolationMode.High;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));

            // 设置图像绘制属性: 设置透明度
            ImageAttributes imageAttributes = new ImageAttributes();
            float[][] colorMatrixElements = {
                new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
                new float[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f},
                new float[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f},
                new float[] {0.0f, 0.0f, 0.0f, opacity, 0.0f},
                new float[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}};
            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            // 合并
            int wmWidth = img2.Width;
            int wmHeight = img2.Height;
            foreach (Point pt in points)
                g.DrawImage(
                    img2,
                    new Rectangle(pt.X, pt.Y, wmWidth, wmHeight),
                    0, 0, wmWidth, wmHeight,
                    GraphicsUnit.Pixel,
                    imageAttributes
                    );

            // 释放资源
            img.Dispose();
            g.Dispose();
            return bmp;
        }

        /// <summary>正弦扭曲图片</summary>  
        /// <param name="img">图片路径</param>  
        /// <param name="range">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>  
        /// <param name="phase">波形的起始相位，取值区间[0-2*PI)</param>  
        /// <param name="direction">扭曲方向</param>  
        /// <remarks>现在只能实现0度和90度扭曲，难的验证码是三维曲面扭曲，字体完全变形粘连才难破解（容后）</remarks>
        public static Bitmap TwistImage(Bitmap img, double range = 3, double phase = 0, bool direction = false)
        {
            double PI2 = 6.283185307179586476925286766559;
            Bitmap destBmp = new Bitmap(img.Width, img.Height);
            Graphics g = Graphics.FromImage(destBmp);
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            g.Dispose();

            // 遍历填充像素
            double baseAxisLen = direction ? (double)destBmp.Height : (double)destBmp.Width;
            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = direction ? (PI2 * (double)j) / baseAxisLen : (PI2 * (double)i) / baseAxisLen;
                    dx += phase;
                    double dy = Math.Sin(dx);

                    // 取得当前点的颜色
                    int nOldX = 0, nOldY = 0;
                    nOldX = direction ? i + (int)(dy * range) : i;
                    nOldY = direction ? j : j + (int)(dy * range);
                    Color color = img.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            return destBmp;
        }
    }
}