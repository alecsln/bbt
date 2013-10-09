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
    public class AnswerController : DefaultController
    {
        private String per = Libs.GetPermission("ANSWER");
        public ActionResult Index()
        {
            LoadDefault();

            if (per.IndexOf("S") < 0)
                return View("NotPermission");
            AnswerViewModel av = new AnswerViewModel();
            if (!Request.IsNull("txtKey"))
                av.Content = Request.Get("txtKey");
            if (!Request.IsNull("slSearchStatus"))
                av.Status = Request.GetNumber("slSearchStatus");

            av.PageIndex = Request.IsNull("page") ? 1 : Request.GetNumber("page");
            av.PageSize = Request.IsNull("show") ? 20 : Request.GetNumber("show");

            //Eidt
            if (!Request.IsNull("ID"))
            {
                av.AnswerID = new Guid(Request.Get("ID"));
                ViewBag.Edit = av.SelectOne().AsEnumerable().ToList();
            }

            //Page
            List<DataRow> list = av.SelectAll().AsEnumerable().ToList();
            ViewBag.Answer = list;
            ViewBag.TotalPages = list.Count > 0 ? (int)Math.Ceiling(Convert.ToDouble(list[0]["TotalRows"]) / (double)av.PageSize) : 0;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update()
        {
            try
            {
                if (per.IndexOf("U") < 0)
                    return View("NotPermission");
                AnswerViewModel a = new AnswerViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        a.Status = (Request.GetNumber("slStatus-" + i.ToString()));
                        a.AnswerID = new Guid(Request.Get("ckID-" + i.ToString()));
                        a.Update();
                    }
                }
                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Answer" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Answer?error=1");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete()
        {
            try
            {
                if (per.IndexOf("D") < 0)
                    return View("NotPermission");
                AnswerViewModel a = new AnswerViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        a.AnswerID = new Guid(Request.Get("ckID-" + i.ToString()));
                        a.Delete();
                    }
                }
                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Answer" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Answer?error=1");
            }
        }
    }
}
