using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iGoo.Models;
using iGoo.Helpers;
using System.Data;
using iGoo.Classes;

namespace iGoo.Controllers
{
    public class ProductController : DefaultController
    {
        public ActionResult Index(string title, int page)
        {
            LoadDefault();

            CategoryViewModel cvm = new CategoryViewModel();
            //Select Category by SEOName
            cvm.SEOName = title;
            List<DataRow> listCate = cvm.SelectBySEOName().AsEnumerable().ToList();
            ViewBag.Category = listCate;

            //Select product
            ProductViewModel pvm = new ProductViewModel();
            if (!Request.IsNull("key"))
                pvm.Title = Request.Get("key");
                
            pvm.CategoryID = new Guid(listCate[0]["CategoryID"].ToString());
            pvm.PageIndex = page;
            pvm.PageSize = 20;

            //Page
            List<DataRow> list = pvm.SelectByCategory().AsEnumerable().ToList();
            ViewBag.Product = list;
            ViewBag.TotalPages = list.Count > 0 ? (int)Math.Ceiling(Convert.ToDouble(list[0]["TotalRows"]) / (double)pvm.PageSize) : 0;           
            ViewBag.CategoryPage = page;

            return View();
        }

        public ActionResult View(string title)
        {
            LoadDefault();

            AttributeViewModel avm = new AttributeViewModel();
            avm.Code = "STATIC_PRODUCT_SUPPORT";
            ViewBag.StaticProductSupport = avm.SelectByCode().AsEnumerable().ToList();

            //Select product
            ProductViewModel pvm = new ProductViewModel();
            pvm.SEOName = title;
            List<DataRow> list = pvm.SelectBySEOName().AsEnumerable().ToList();
            ViewBag.Product = list;

            ViewBag.ProductRelated = list[0]["Related"].ToString().Split('\n');

            pvm.ProductID = new Guid(list[0]["ProductID"].ToString());
            pvm.Top = 8;
            ViewBag.ProductOther = pvm.SelectOtherProducts().AsEnumerable().ToList();

            CommentViewModel cv = new CommentViewModel();
            cv.ID = new Guid(list[0]["ProductID"].ToString());
            cv.Type = 1;
            ViewBag.Comment = cv.SelectByNewsID().AsEnumerable().ToList();

            return View();
        }

        public ActionResult SiteMap(int page)
        {
            WebsiteViewModel wvm = new WebsiteViewModel();
            ViewBag.Website = wvm.SelectTop().AsEnumerable().ToList();

            ProductViewModel pvm = new ProductViewModel();
            pvm.PageIndex = page;
            pvm.PageSize = 500;
            ViewBag.SiteMap = pvm.SelectSiteMap().AsEnumerable().ToList();
            return View();
        }

        public ActionResult SiteMapXML()
        {
            ProductViewModel pvm = new ProductViewModel();
            pvm.PageIndex = 1;
            pvm.PageSize = 500;
            ViewBag.SiteMap = pvm.SelectSiteMap().AsEnumerable().ToList();
            return View();
        }

        public ActionResult Rss(string title)
        {
            //Select Category by SEOName
            CategoryViewModel cvm = new CategoryViewModel();
            cvm.SEOName = title;
            List<DataRow> listCate = cvm.SelectBySEOName().AsEnumerable().ToList();
            ViewBag.Category = listCate;

            //Select news
            ProductViewModel pvm = new ProductViewModel();
            pvm.CategoryID = new Guid(listCate[0]["CategoryID"].ToString());
            pvm.PageIndex = 1;
            pvm.PageSize = 100;

            //Page
            List<DataRow> list = pvm.SelectByCategory().AsEnumerable().ToList();
            ViewBag.Rss = list;
            ViewBag.TotalPages = list.Count > 0 ? (int)Math.Ceiling(Convert.ToDouble(list[0]["TotalRows"]) / (double)pvm.PageSize) : 0;

            return View();
        }

        public ActionResult TagsProduct(string title)
        {
            LoadDefault();

            QuestionViewModel qvm = new QuestionViewModel();
            qvm.Top = 10;
            ViewBag.QuestionNew = qvm.SelectByCreated().AsEnumerable().ToList();

            CategoryViewModel cvm = new CategoryViewModel();
            cvm.Type = "LEFT";
            ViewBag.MenuLeft = cvm.SelectByType().AsEnumerable().ToList();

            AdvViewModel adv = new AdvViewModel();
            adv.CategoryID = new Guid("11111111-1111-1111-1111-111111111111");
            adv.Type = "ADV_RIGHT";
            ViewBag.AdvRight = adv.SelectByType().AsEnumerable().ToList();

            //Select meta
            AttributeViewModel avm = new AttributeViewModel();
            avm.Code = "1";
            avm.Value = title;
            ViewBag.Meta= avm.SelectByValue().AsEnumerable().ToList();

            //Select news
            ProductViewModel pvm = new ProductViewModel();
            ViewBag.TagsTitle = title;
            title = title.Replace('-', ' ');
            pvm.Tags = title;
            List<DataRow> list = pvm.SelectByTags().AsEnumerable().ToList();
            ViewBag.Product = list;

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public String AddComment()
        {
            try
            {
                if (Session["MemberID"] == null || Request.IsNull("txtComment") || Request.IsNull("ProductID"))
                    return "Đăng nhập hoặc Đăng ký ngay để có thể gửi bình luận!";
                CommentViewModel cm = new CommentViewModel();
                cm.Content = Request.Get("txtComment");
                if (cm.Content.ToString().Length < 20)
                    return "Nội dung < 20 ký tự!";
                cm.CommentID = Guid.NewGuid();
                cm.UserID = new Guid(Session["MemberID"].ToString());
                cm.ID = new Guid(Request.Get("ProductID"));
                cm.Type = 1;
                cm.Status = 1;
                cm.Spam = String.Empty;
                cm.Created = DateTime.Now;
                cm.Insert();
                return "Gửi bình luận thành công! Cảm ơn bạn đã đóng góp ý kiến!";
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return "Xin lỗi! Hãy thử lại!";
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public bool AddSpam()
        {
            try
            {
                if (Session["MemberID"] == null)
                    return false;
                CommentViewModel cm = new CommentViewModel();
                cm.CommentID = new Guid(Request.Get("CommentID"));
                cm.Spam = Session["MemberID"].ToString();
                cm.AddSpam();
                return true;
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return false;
            }
        }

    }
}
