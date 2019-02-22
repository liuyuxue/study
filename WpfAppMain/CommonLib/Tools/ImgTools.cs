using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Tools
{
    public class ImgTools 
    {
        /// <summary>
        /// wpf控件转Bitmap
        /// </summary>
        /// <param name="vsual"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static System.Windows.Media.Imaging.RenderTargetBitmap RenderVisaulToBitmap(System.Windows.Media.Visual vsual, int width, int height)
        {
            var rtb = new System.Windows.Media.Imaging.RenderTargetBitmap(width, height, 96, 96, System.Windows.Media.PixelFormats.Default);
            rtb.Render(vsual);
            return rtb;
        }

        /// <summary>
        /// Bitmap转图片文件
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="jpgFilename"></param>
        public static void BitmapToImgFile(System.Windows.Media.Imaging.BitmapSource bitmap, string jpgFilename)
        {
            System.IO.Stream destStream = System.IO.File.Create(jpgFilename);
            System.Windows.Media.Imaging.BitmapEncoder encoder = null;
            encoder = new System.Windows.Media.Imaging.JpegBitmapEncoder();
            encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bitmap));
            encoder.Save(destStream);
            destStream.Close();
        }


        /// <summary>
        /// stream转byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public byte[] StreamToBytes(System.IO.Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            return bytes;

        }

        /// 将 byte[]转成 Stream
        public System.IO.Stream BytesToStream(byte[] bytes)
        {
            System.IO.Stream stream = new System.IO.MemoryStream(bytes);
            return stream;
        }



        ///将 Stream 写入文件 
        public static void StreamToFile(Stream stream, string fileName)
        {
            // 把 Stream 转换成 byte[] 
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始 
            stream.Seek(0, System.IO.SeekOrigin.Begin);

            // 把 byte[] 写入文件 
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }

        ///从文件读取 Stream
        public static Stream FileToStream(string fileName)
        {
            // 打开文件 
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 读取文件的 byte[] 
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            // 把 byte[] 转换成 Stream 
            Stream stream = new MemoryStream(bytes);
            return stream;

        }

        /// <summary>
        /// wpf的Bitmap转Byte[]
        /// </summary>
        /// <param name="bitmapSource"></param>
        /// <returns></returns>
        public static byte[] BitmapToBytes(System.Windows.Media.Imaging.BitmapSource bitmapSource)
        {
            System.Windows.Media.Imaging.JpegBitmapEncoder encoder = new System.Windows.Media.Imaging.JpegBitmapEncoder();
            //encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            encoder.QualityLevel = 100;
            // byte[] bit = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bitmapSource));
                encoder.Save(stream);
                byte[] bit = stream.ToArray();
                stream.Close();
                return bit;
            }
        }

        /// <summary>
        /// winform的Bitmap转Byte[]
        /// </summary>
        /// <param name="bitmapSource"></param>
        /// <returns></returns>
        public static byte[] Bitmap2Byte(System.Drawing.Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return data;
            }
        }


        /// <summary>
        /// byte[]转winform框架中的Bitmap
        /// </summary>
        /// <param name="bytelist"></param>
        /// <returns></returns>
        public static System.Drawing.Bitmap Bytes2Bitmap(byte[] bytelist)
        {
            MemoryStream ms1 = new MemoryStream(bytelist);
            System.Drawing.Bitmap bm = (System.Drawing.Bitmap)System.Drawing.Image.FromStream(ms1);
            ms1.Close();
            return bm;
        }

        /// <summary>
        /// 从Bitmap转换成BitmapSource
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public System.Windows.Media.Imaging.BitmapSource ChangeBitmapToBitmapSource(System.Drawing.Bitmap bmp)
        {
            System.Windows.Media.Imaging.BitmapSource returnSource;
            try
            {
                returnSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bmp.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
            catch
            {
                returnSource = null;
            }
            return returnSource;



        }


        /// <summary>
        /// 从BitmapSource转换成Bitmap
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public System.Drawing.Bitmap ChangeBitmapSourceToBitmap ( System.Windows.Media.Imaging.BitmapSource bitmapSource)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            var encoder = new System.Windows.Media.Imaging.BmpBitmapEncoder();
            encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create((System.Windows.Media.Imaging.BitmapSource)bitmapSource));
            encoder.Save(ms);

            var bp = new System.Drawing.Bitmap(ms);
            ms.Close();
            return bp;
        }


        public static System.Windows.Controls.Image GetImageFromVisual(System.Windows.Media.Visual visual, int width, int height)
        {
            var img = new System.Windows.Controls.Image();
            if (visual != null && width > 0 && height > 0)
            {
                var bitmap = new System.Windows.Media.Imaging.RenderTargetBitmap(width, height, 96.0, 96.0, 
                    System.Windows.Media.PixelFormats.Default);
                bitmap.Render(visual);
                img.Source = bitmap;
            }
            return img;
        }

        public static System.Windows.Controls.Image GetImageFromVisual2(System.Windows.Media.Visual visual, int width, int height)
        {
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            var bitmap = new System.Windows.Media.Imaging.RenderTargetBitmap(width, height, 96.0, 96.0, 
                System.Windows.Media.PixelFormats.Default);
            var drawingVisual = new System.Windows.Media.DrawingVisual();
            using (System.Windows.Media.DrawingContext context = drawingVisual.RenderOpen())
            {
                System.Windows.Media.VisualBrush brush = new System.Windows.Media.VisualBrush(visual)
                {
                    Stretch = System.Windows.Media.Stretch.None
                };
                context.DrawRectangle(brush, null, new System.Windows.Rect(0.0, 0.0, (double)width, (double)height));
                context.Close();
            }
            bitmap.Render(drawingVisual);
            img.Source = bitmap;
            return img;
        }

    }
}
