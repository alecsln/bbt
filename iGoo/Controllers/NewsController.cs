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
    public class NewsController : DefaultController
    {

        public ActionResult Index(string title, int page)
        {
            LoadDefault();

            CategoryViewModel cvm = new CategoryViewModel();            
            //Select Category by SEOName
            cvm.SEOName = title;
            List<DataRow> listCate = cvm.SelectBySEOName().AsEnumerable().ToList();
            ViewBag.Category = listCate;

            AdvViewModel adv = new AdvViewModel();
            adv.CategoryID = new Guid("11111111-1111-1111-1111-111111111111");
            adv.Type = "ADV_RIGHT";
            ViewBag.AdvRight = adv.SelectByType().AsEnumerable().ToList();

            //Select news
            NewsViewModel nvm = new NewsViewModel();
            nvm.Top = 10;
            nvm.CategoryID = new Guid(listCate[0]["CategoryID"].ToString());
            ViewBag.NewsComment = nvm.SelectByModified().AsEnumerable().ToList();

            if (!Request.IsNull("key"))
                nvm.Title = Request.Get("key");

            nvm.PageIndex = page;
            nvm.PageSize = 10;

            //Page
            List<DataRow> list = nvm.SelectByCategory().AsEnumerable().ToList();
            ViewBag.News = list;
            ViewBag.TotalPages = list.Count > 0 ? (int)Math.Ceiling(Convert.ToDouble(list[0]["TotalRows"]) / (double)nvm.PageSize) : 0;
            ViewBag.CategoryPage = page;

            return View();
        }

        public ActionResult View(string title)
        {
            LoadDefault();
                        
            //Select news
            NewsViewModel nvm = new NewsViewModel();
            nvm.SEOName = title;
            List<DataRow> list = nvm.SelectBySEOName().AsEnumerable().ToList();
            ViewBag.News = list;

            ViewBag.NewsRelated = list[0]["Related"].ToString().Split('\n');

            //Select news other and new news
            nvm.NewsID = new Guid(list[0]["NewsID"].ToString());
            nvm.Top = 5;
            ViewBag.OtherNews = nvm.SelectOtherNews().AsEnumerable().ToList();
            
            ViewBag.NewNews = nvm.SelectNewNews().AsEnumerable().ToList();

            CommentViewModel cv = new CommentViewModel();
            cv.ID = new Guid(list[0]["NewsID"].ToString());
            cv.Type = 0;
            ViewBag.Comment = cv.SelectByNewsID().AsEnumerable().ToList();

            return View();
        }

        public ActionResult SiteMap(int page)
        {
            WebsiteViewModel wvm = new WebsiteViewModel();
            ViewBag.Website = wvm.SelectTop().AsEnumerable().ToList();

            NewsViewModel nvm = new NewsViewModel();
            nvm.PageIndex = page;
            nvm.PageSize = 500;
            ViewBag.SiteMap = nvm.SelectSiteMap().AsEnumerable().ToList();
            return View();
        }

        public ActionResult SiteMapXML()
        {
            NewsViewModel nvm = new NewsViewModel();
            nvm.PageIndex = 1;
            nvm.PageSize = 500;
            ViewBag.SiteMap = nvm.SelectSiteMap().AsEnumerable().ToList();
            return View();
        }

        public ActionResult WebSiteMapXML()
        {
            WebsiteViewModel wvm = new WebsiteViewModel();
            ViewBag.Website = wvm.SelectTop().AsEnumerable().ToList();
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
            NewsViewModel nvm = new NewsViewModel();
            nvm.CategoryID = new Guid(listCate[0]["CategoryID"].ToString());
            nvm.PageIndex = 1;
            nvm.PageSize = 100;

            //Page
            List<DataRow> list = nvm.SelectByCategory().AsEnumerable().ToList();
            ViewBag.Rss = list;
            ViewBag.TotalPages = list.Count > 0 ? (int)Math.Ceiling(Convert.ToDouble(list[0]["TotalRows"]) / (double)nvm.PageSize) : 0;

            return View();
        }

        public ActionResult TagsNews(string title)
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
            avm.Code = "0";
            avm.Value = title;
            ViewBag.Meta = avm.SelectByValue().AsEnumerable().ToList();

            //Select news
            NewsViewModel nv = new NewsViewModel();
            ViewBag.TagsTitle = title;
            title = title.Replace('-', ' ');
            nv.Tags = title;
            List<DataRow> list = nv.SelectByTags().AsEnumerable().ToList();
            ViewBag.News = list;

           return View();
        }

        public ActionResult PageView(string title)
        {
            AttributeViewModel avm = new AttributeViewModel();
            avm.Code = title;
            ViewBag.PageView = avm.SelectPageView().AsEnumerable().ToList();
            return View();
        }

        public ActionResult Search()
        {
            LoadDefault();

            CategoryViewModel cvm = new CategoryViewModel();
            cvm.Type = "LEFT";
            ViewBag.MenuLeft = cvm.SelectByType().AsEnumerable().ToList();

            return View();
        }

        public ActionResult Error()
        {
            Libs.WriteError("404 Error: " + Request.Get("aspxerrorpath"));
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public String AddComment()
        {
            try
            {
                if (Session["MemberID"] == null || Request.IsNull("txtComment") || Request.IsNull("NewsID"))
                    return "Đăng nhập hoặc Đăng ký ngay để có thể gửi bình luận!";
                CommentViewModel cm = new CommentViewModel();
                cm.Content = Request.Get("txtComment");
                if (cm.Content.ToString().Length < 20)
                    return "Nội dung < 20 ký tự!";
                cm.CommentID = Guid.NewGuid();
                cm.UserID = new Guid(Session["MemberID"].ToString());
                cm.ID = new Guid(Request.Get("NewsID"));
                cm.Type = 0;
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
