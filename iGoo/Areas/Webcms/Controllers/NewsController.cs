using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iGoo.Areas.Webcms.Models;
using iGoo.Helpers;
using System.Data;
using iGoo.Classes;

namespace iGoo.Areas.Webcms.Controllers
{
    public class NewsController : DefaultController
    {
        private String per = Libs.GetPermission("NEWS");
        public ActionResult Index()
        {
            LoadDefault();

            if (per.IndexOf("S") < 0)
                return View("NotPermission");
            //Slect group
            CategoryViewModel ct = new CategoryViewModel();
            ct.Code = "CATEGORY_NEWS";
            ViewBag.GroupCate = ct.SelectChild().AsEnumerable().ToList();
            //ct.Code = "CATEGORY_GOOGLE";
            //ViewBag.GroupGoogle = ct.SelectChild().AsEnumerable().ToList();
            ct.MenuAll = ct.SelectOptimize();
            ViewBag.MenuCate = ct.SelectMenu(new Guid("6c8df8b0-0365-4d23-ba29-1b9681b85059"));

            AttributeViewModel at = new AttributeViewModel();
            at.Code = "ATTRIBUTE_NEWS";
            ViewBag.GroupNews = at.SelectChild().AsEnumerable().ToList();

            //Select news
            NewsViewModel nv = new NewsViewModel();
            if (!Request.IsNull("txtKey"))
                nv.Title = Request.Get("txtKey");
            if (!Request.IsNull("slSearchCate"))
                nv.CategoryID = new Guid(Request.Get("slSearchCate"));
            if (!Request.IsNull("slSearchType"))
                nv.Type = Request.Get("slSearchType");
            if (!Request.IsNull("slSearchStatus"))
                nv.Status = Request.GetNumber("slSearchStatus");

            nv.PageIndex = Request.IsNull("page") ? 1 : Request.GetNumber("page");
            nv.PageSize = Request.IsNull("show") ? 20 : Request.GetNumber("show");


            if (!Request.IsNull("OrderBy") && !Request.IsNull("Order"))
                nv.OrderBy = Request.Get("OrderBy") + " " + Request.Get("Order");

            //Eidt
            if (!Request.IsNull("ID"))
            {
                nv.NewsID = new Guid(Request.Get("ID"));
                ViewBag.Edit = nv.SelectOne().AsEnumerable().ToList();
            }

            //Page
            List<DataRow> list = nv.SelectAll().AsEnumerable().ToList();
            ViewBag.News = list;
            ViewBag.TotalPages = list.Count > 0 ? (int)Math.Ceiling(Convert.ToDouble(list[0]["TotalRows"]) / (double)nv.PageSize) : 0;
            return View();
        }

        public ActionResult Add()
        {
            LoadDefault();

            if (per.IndexOf("I") < 0)
                return View("NotPermission");
            //Slect group
            CategoryViewModel ct = new CategoryViewModel();
            //ct.Code = "CATEGORY_NEWS";
            //ViewBag.GroupCate = ct.SelectChild().AsEnumerable().ToList();
            //ct.Code = "CATEGORY_GOOGLE";
            //ViewBag.GroupGoogle = ct.SelectChild().AsEnumerable().ToList();


            ct.MenuAll = ct.SelectOptimize();
            ViewBag.MenuCate = ct.SelectMenu(new Guid("6c8df8b0-0365-4d23-ba29-1b9681b85059"));

            AttributeViewModel at = new AttributeViewModel();
            at.Code = "ATTRIBUTE_NEWS";
            ViewBag.Type = at.SelectChild().AsEnumerable().ToList();

            //Select user
            NewsViewModel nv = new NewsViewModel();

            //Eidt
            if (!Request.IsNull("ID"))
            {
                nv.NewsID = new Guid(Request.Get("ID"));
                ViewBag.Edit = nv.SelectOne().AsEnumerable().ToList();

            }

            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create()
        {
            try
            {
                CheckUser();
                NewsViewModel nv = new NewsViewModel();
                nv.CategoryID = new Guid(Request.Get("slSearchCate"));
                nv.Title = Request.Get("txtTitle");
                nv.SEOName = Request.Get("txtSEOName");
                nv.Image = Request.Get("txtImage");
                nv.SlideImage = Request.Get("txtSlideImage");
                nv.Brief = Request.Get("txtBrief");
                nv.Content = Request.Get("txtContent");
                nv.MetaTitle = Request.Get("txtMetaTitle");
                nv.MetaKeyword = Request.Get("txtMetaKeyword");
                nv.MetaDescription = Request.Get("txtMetaDescription");
                nv.Type = Request.Get("ckType").Replace(',', '#');
                nv.Tags = Libs.ReplaceSpace("  "," ", Request.Get("txtTags"));
                nv.TagsTitle = Libs.ReplaceSpace("  ", " ", Request.Get("txtTagsTitle"));
                nv.Related = Request.Get("txtRelated");
                nv.Status = Request.GetNumber("slStatus");
                if (!Request.IsNull("txtOrder"))
                    nv.Order = Request.GetNumber("txtOrder");
                else
                    nv.Order = 999;
                if (!Request.IsNull("txtPollID"))
                    nv.PollID = new Guid(Request.Get("txtPollID"));
                if (Request.IsNull("ID"))
                {
                    if (per.IndexOf("I") < 0)
                        return View("NotPermission");
                    nv.Visitor = 0;
                    nv.TotalComment = 0;
                    nv.Created = DateTime.Now;
                    nv.NewsID = Guid.NewGuid();
                    nv.UserID = new Guid(Session["UserID"].ToString());
                    nv.Insert();
                }
                else
                {
                    if (per.IndexOf("U") < 0)
                        return View("NotPermission");
                    if (!Request.IsNull("ckRefresh"))
                        nv.Created = DateTime.Now;
                    nv.NewsID = new Guid(Request.Get("ID"));
                    nv.Update();
                }

                return Redirect("/Webcms/News/Add?ID=" + Request.Get("ID") + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/News/Add?ID=" + Request.Get("ID") + "&error=1");
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update()
        {
            try
            {
                if (per.IndexOf("U") < 0)
                    return View("NotPermission");
                NewsViewModel nv = new NewsViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        nv.NewsID = new Guid(Request.Get("ckID-" + i.ToString()));
                        nv.Status = (Request.GetNumber("slStatus-" + i.ToString()));
                        if (!Request.IsNull("txtOrder-" + i.ToString()))
                            nv.Order = (Request.GetNumber("txtOrder-" + i.ToString()));
                        else
                            nv.Order = 999;
                        nv.Update();
                    }
                }

                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/News" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/News?error=1");
            }
        }

        [HttpPost]
        public ActionResult Delete()
        {
            try
            {
                if (per.IndexOf("D") < 0)
                    return View("NotPermission");
                NewsViewModel nv = new NewsViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        nv.NewsID = new Guid(Request.Get("ckID-" + i.ToString()));
                        nv.Delete();
                    }
                }

                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/News" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/News?error=1");
            }
        }

    }
}
