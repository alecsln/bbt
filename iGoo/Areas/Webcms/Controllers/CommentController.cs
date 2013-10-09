using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iGoo.Helpers;
using System.Data;
using iGoo.Classes;
using iGoo.Areas.Webcms.Models;

namespace iGoo.Areas.Webcms.Controllers
{
    public class CommentController : DefaultController
    {
        private String per = Libs.GetPermission("COMMENT");
        public ActionResult Index()
        {
            LoadDefault();

            if (per.IndexOf("S") < 0)
                return View("NotPermission");
            CommentViewModel cv = new CommentViewModel();
            if (!Request.IsNull("txtKey"))
                cv.Content = Request.Get("txtKey");
            if (!Request.IsNull("NewsID"))
                cv.ID = new Guid(Request.Get("NewsID"));
            if (!Request.IsNull("slSearchType"))
                cv.Type = Request.GetNumber("slSearchType");
            if (!Request.IsNull("slSearchStatus"))
                cv.Status = Request.GetNumber("slSearchStatus");

            cv.PageIndex = Request.IsNull("page") ? 1 : Request.GetNumber("page");
            cv.PageSize = Request.IsNull("show") ? 20 : Request.GetNumber("show");

            //Eidt
            if (!Request.IsNull("ID"))
            {
                cv.CommentID = new Guid(Request.Get("ID"));
                ViewBag.Edit = cv.SelectOne().AsEnumerable().ToList();
            }

            //Page
            List<DataRow> list = cv.SelectAll().AsEnumerable().ToList();
            ViewBag.Comments = list;
            ViewBag.TotalPages = list.Count > 0 ? (int)Math.Ceiling(Convert.ToDouble(list[0]["TotalRows"]) / (double)cv.PageSize) : 0;
            return View();
        }

        [HttpPost]
        public ActionResult Create()
        {
            try
            {
                if (per.IndexOf("U") < 0)
                    return View("NotPermission");
                CommentViewModel u = new CommentViewModel();
                u.CommentID = new Guid(Request.Get("ID"));
                u.Content = Request.Get("txtContent");
                u.Status = Request.GetNumber("slStatus");
                u.Update();

                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Comment" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Comment?error=1");
            }
        }

        [HttpPost]
        public ActionResult Update()
        {
            try
            {
                if (per.IndexOf("U") < 0)
                    return View("NotPermission");
                CommentViewModel u = new CommentViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        u.Status = (Request.GetNumber("slStatus-" + i.ToString()));
                        u.CommentID = new Guid(Request.Get("ckID-" + i.ToString()));
                        u.Update();
                    }
                }
                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Comment" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Comment?error=1");
            }
        }

        [HttpPost]
        public ActionResult Delete()
        {
            try
            {
                if (per.IndexOf("D") < 0)
                    return View("NotPermission");
                CommentViewModel u = new CommentViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        u.CommentID = new Guid(Request.Get("ckID-" + i.ToString()));
                        u.Delete();
                    }
                }
                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Comment" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Comment?error=1");
            }
        }
    }
}
