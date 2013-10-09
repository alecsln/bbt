using System;
using System.Web.Mvc;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;
using System.Web.Caching;

namespace iGoo.Helpers
{
    public class ThumbnailResult : ActionResult
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Filename { get; set; }

        public ThumbnailResult(int Width, int Height, string Filename)
        {
            this.Width = Width;
            this.Height = Height;
            this.Filename = Filename;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            //A little convention - place all images in the path Content/Images
            string FilePath = context.HttpContext.Server.MapPath(context.HttpContext.Request.ApplicationPath)
            + @"Uploads\" + Filename;

            if (!File.Exists(FilePath))
                FilePath = context.HttpContext.Server.MapPath(context.HttpContext.Request.ApplicationPath)
            + "Source/NoImage.jpg";

            Bitmap bitmap = new Bitmap(FilePath);

            //Cache
            HttpResponseBase Response = context.HttpContext.Response;
            Response.Cache.SetCacheability(HttpCacheability.Public);
            Response.Cache.SetExpires(Cache.NoAbsoluteExpiration);
            Response.AddFileDependency(FilePath);
            Response.Cache.SetLastModifiedFromFileDependencies(); 
            
            try
            {
                if (bitmap.Width < Width && bitmap.Height < Height)
                {
                    context.HttpContext.Response.ContentType = "image/jpeg";
                    bitmap.Save(context.HttpContext.Response.OutputStream, ImageFormat.Jpeg);
                    return;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                bitmap.Dispose();
            }


            Bitmap FinalBitmap = null;

            try
            {
                bitmap = new Bitmap(FilePath);

                int BitmapNewWidth;
                decimal Ratio;
                int BitmapNewHeight;

                if (bitmap.Width > bitmap.Height)
                {
                    Ratio = (decimal)Height / bitmap.Width;
                    BitmapNewWidth = Width;

                    decimal temp = bitmap.Height * Ratio;
                    BitmapNewHeight = (int)temp;
                }
                else
                {
                    Ratio = (decimal)Width / bitmap.Height;
                    BitmapNewHeight = Height;

                    decimal temp = bitmap.Width * Ratio;
                    BitmapNewWidth = (int)temp;
                }

        

                FinalBitmap = new Bitmap(BitmapNewWidth, BitmapNewHeight);
                Graphics graphics = Graphics.FromImage(FinalBitmap);
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.FillRectangle(Brushes.White, 0, 0, BitmapNewWidth, BitmapNewHeight);
                graphics.DrawImage(bitmap, 0, 0, BitmapNewWidth, BitmapNewHeight);

                context.HttpContext.Response.ContentType = "image/jpeg";
                FinalBitmap.Save(context.HttpContext.Response.OutputStream, ImageFormat.Jpeg);

            }
            catch (Exception e)
            {
               throw new Exception(e.Message);
            }
            finally
            {
                if (FinalBitmap != null) FinalBitmap.Dispose();
            }
           
        }
    }
}