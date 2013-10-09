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
    public class AnswerController : DefaultController
    {
        
        public ActionResult Index()
        {
            LoadDefault();
            
            UserViewModel uvm = new UserViewModel();
            uvm.TotalRows = 10;
            ViewBag.UserAnswer = uvm.SelectTopUserAnswer().AsEnumerable().ToList();

            CategoryViewModel cvm = new CategoryViewModel();
            cvm.Type = "ANSWER";
            ViewBag.MenuAnswer = cvm.SelectByType().AsEnumerable().ToList();

            cvm.Code = "CATEGORY_ANSWER";
            ViewBag.Category = cvm.SelectByCode().AsEnumerable().ToList();

            QuestionViewModel qvm = new QuestionViewModel();
            qvm.Top = 20;
            ViewBag.Question = qvm.SelectByCreated().AsEnumerable().ToList();

            return View();
        }

        public ActionResult List(string title, int page)
        {
            LoadDefault();

            CategoryViewModel cvm = new CategoryViewModel();
            cvm.Type = "ANSWER";
            ViewBag.MenuAnswer = cvm.SelectByType().AsEnumerable().ToList();

            //Select Category by SEOName
            cvm.SEOName = title;
            List<DataRow> listCate = cvm.SelectBySEOName().AsEnumerable().ToList();
            ViewBag.Category = listCate;

            //Select news
            QuestionViewModel qvm = new QuestionViewModel();
            qvm.CategoryID = new Guid(listCate[0]["CategoryID"].ToString());
            if (!Request.IsNull("key"))
                qvm.Title = Request.Get("key");
            qvm.PageIndex = page;
            qvm.PageSize = 20;

            //Page
            List<DataRow> list = qvm.SelectByCategory().AsEnumerable().ToList();
            ViewBag.Question = list;
            ViewBag.TotalPages = list.Count > 0 ? (int)Math.Ceiling(Convert.ToDouble(list[0]["TotalRows"]) / (double)qvm.PageSize) : 0;

            ViewBag.CategoryPage = page;

            return View();
        }

        public ActionResult TagsAnswer(string title)
        {
            LoadDefault();
            CategoryViewModel cvm = new CategoryViewModel();
            cvm.Type = "ANSWER";
            ViewBag.MenuAnswer = cvm.SelectByType().AsEnumerable().ToList();

            //Select news
            QuestionViewModel qvm = new QuestionViewModel();
            ViewBag.TagsTitle = title;
            qvm.Title = title.Replace('-', ' ');
            List<DataRow> list = qvm.SelectByTags().AsEnumerable().ToList();
            ViewBag.Question = list;

            String MetaDescription = String.Empty;
            foreach (DataRow row in list)
            {
                if (!String.IsNullOrEmpty(row["Title"].ToString()))
                    MetaDescription += row["Title"].ToString() + ", ";
            }
            ViewBag.CategoryDescription = (MetaDescription.EndsWith(", ") ? MetaDescription.Substring(0, MetaDescription.Length - 2) : MetaDescription).Truncate(160);

            return View();
        }

        public ActionResult View(string title)
        {
            LoadDefault();

            CategoryViewModel cvm = new CategoryViewModel();
            cvm.Type = "ANSWER";
            ViewBag.MenuAnswer = cvm.SelectByType().AsEnumerable().ToList();

            //Select news
            QuestionViewModel qvm = new QuestionViewModel();
            qvm.SEOName = title;
            List<DataRow> list = qvm.SelectBySEOName().AsEnumerable().ToList();
            ViewBag.Question = list;

            AnswerViewModel cv = new AnswerViewModel();
            cv.QuestionID = new Guid(list[0]["QuestionID"].ToString());
            ViewBag.Answer = cv.SelectByQuestionID().AsEnumerable().ToList();
            return View();
        }

        public ActionResult SiteMap(int page)
        {
            WebsiteViewModel wvm = new WebsiteViewModel();
            ViewBag.Website = wvm.SelectTop().AsEnumerable().ToList();

            QuestionViewModel qvm = new QuestionViewModel();
            qvm.PageIndex = page;
            qvm.PageSize = 500;
            ViewBag.SiteMap = qvm.SelectSiteMap().AsEnumerable().ToList();
            return View();
        }

        public ActionResult SiteMapXML()
        {
            QuestionViewModel qvm = new QuestionViewModel();
            qvm.PageIndex = 1;
            qvm.PageSize = 500;
            ViewBag.SiteMap = qvm.SelectSiteMap().AsEnumerable().ToList();
            return View();
        }

        public ActionResult Question()
        {
            LoadDefault();
            QuestionViewModel qvm = new QuestionViewModel();
            qvm.Top = 5;
            ViewBag.QuestionNew = qvm.SelectByCreated().AsEnumerable().ToList();

            CategoryViewModel cvm = new CategoryViewModel();
            cvm.Type = "ANSWER";
            ViewBag.MenuAnswer = cvm.SelectByType().AsEnumerable().ToList();

            QuestionViewModel qv = new QuestionViewModel();
            if (!Request.IsNull("id"))
            {
                qv.QuestionID = new Guid(Request.Get("id"));
                ViewBag.Edit = qv.SelectOne().AsEnumerable().ToList();
            }

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
            QuestionViewModel qvm = new QuestionViewModel();
            qvm.CategoryID = new Guid(listCate[0]["CategoryID"].ToString());
            qvm.PageIndex = 1;
            qvm.PageSize = 100;

            //Page
            List<DataRow> list = qvm.SelectByCategory().AsEnumerable().ToList();
            ViewBag.Rss = list;
            ViewBag.TotalPages = list.Count > 0 ? (int)Math.Ceiling(Convert.ToDouble(list[0]["TotalRows"]) / (double)qvm.PageSize) : 0;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public String AddQuestion()
        {
            try
            {
                if (Session["MemberID"] == null)
                    return "Đăng nhập hoặc Đăng ký ngay để có thể đặt câu hỏi!";
                QuestionViewModel qv = new QuestionViewModel();
                qv.Content = Request.Get("txtContent");
                if (qv.Content.ToString().Length<20)
                    return "Nội dung < 20 ký tự!";
                
                qv.UserID = new Guid(Session["MemberID"].ToString());
                qv.CategoryID = new Guid(Request.Get("cate"));
                qv.Title = Request.Get("txtTitle");
                qv.SEOName = Libs.UrlSEO(qv.Title.ToString());
                qv.Status = 1;
                if (Request.IsNull("ID"))
                {
                    qv.Created = DateTime.Now;
                    qv.Modified = qv.Created;
                    qv.QuestionID = Guid.NewGuid();
                    qv.Insert();
                }
                else
                {
                    qv.Modified = DateTime.Now;
                    qv.QuestionID = new Guid(Request.Get("ID"));
                    qv.Update();
                }

                return "hoidap/"+qv.SEOName.ToString() + ".html";
            }
            catch (Exception ex)
            {
                if (ex.ToString().IndexOf("Cannot insert duplicate key row in object") >= 0)
                    return "2";
                else
                {
                    Libs.WriteError(ex.ToString());
                    return "0";
                }
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public String AddAnswer()
        {
            try
            {
                if (Session["MemberID"] == null || Request.IsNull("txtAnswer") || Request.IsNull("QuestionID"))
                    return "Đăng nhập hoặc Đăng ký ngay để có thể trả lời câu hỏi!";
                AnswerViewModel avm = new AnswerViewModel();
                avm.AnswerID = Guid.NewGuid();
                avm.UserID = new Guid(Session["MemberID"].ToString());
                //avm.UserID = new Guid("944f49a8-eebe-4203-84c0-67d34b174604");
                avm.QuestionID = new Guid(Request.Get("QuestionID"));
                avm.Content = Request.Get("txtAnswer");
                if (avm.Content.ToString().Length < 20)
                    return "Nội dung < 20 ký tự!";
                avm.Status = 1;
                avm.Created = DateTime.Now;
                avm.Insert();
                return "Gửi câu trả lời thành công! Cám ơn bạn đã trả lời câu hỏi!";
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return "Xin lỗi! Hãy thử lại!";
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public bool AddSpamAnswer()
        {
            try
            {
                if (Session["MemberID"] == null)
                    return false;
                AnswerViewModel av = new AnswerViewModel();
                av.AnswerID = new Guid(Request.Get("AnswerID"));
                av.Spam = Session["MemberID"].ToString();
                av.AddSpam();
                return true;
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return false;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public bool AddSpamQuestion()
        {
            try
            {
                if (Session["MemberID"] == null)
                    return false;
                QuestionViewModel qv = new QuestionViewModel();
                qv.QuestionID = new Guid(Request.Get("QuestionID"));
                qv.Spam = Session["MemberID"].ToString();
                qv.AddSpam();
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
