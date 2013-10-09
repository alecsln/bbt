using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iGoo.Helpers;

namespace iGoo.Controllers
{
    public class ThumbnailController : ImageController
    {
        public ThumbnailResult ResizeImage(int width, int height, string date, string file, string ext)
        {
            return Thumbnail(width, height, date + "/" + file + "." + ext);
        }

    }
}
