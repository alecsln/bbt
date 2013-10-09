using System.Web.Mvc;

namespace iGoo.Helpers
{
    public class ImageController : Controller
    {
        protected internal virtual ThumbnailResult Thumbnail(int Width, int Height, string Filename)
        {
            return new ThumbnailResult(Width, Height, Filename);
        }
    }
}