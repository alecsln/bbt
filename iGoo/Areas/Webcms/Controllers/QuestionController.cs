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
    public class QuestionController : DefaultController
    {
        private String per = Libs.GetPermission("QUENSTION");
        public ActionResult Index()
        {
            LoadDefault();
            if (per.IndexOf("S") < 0)
                return View("NotPermission");
            QuestionViewModel qv = new QuestionViewModel();
            if (!Request.IsNull("txtKey"))
                qv.Title = Request.Get("txtKey");
            if (!Request.IsNull("slSearchStatus"))
                qv.Status = Request.GetNumber("slSearchStatus");

            qv.PageIndex = Request.IsNull("page") ? 1 : Request.GetNumber("page");
            qv.PageSize = Request.IsNull("show") ? 20 : Request.GetNumber("show");

            //Eidt
            if (!Request.IsNull("ID"))
            {
                qv.QuestionID = new Guid(Request.Get("ID"));
                ViewBag.Edit = qv.SelectOne().AsEnumerable().ToList();
            }

            //Page
            List<DataRow> list = qv.SelectAll().AsEnumerable().ToList();
            ViewBag.Question = list;
            ViewBag.TotalPages = list.Count > 0 ? (int)Math.Ceiling(Convert.ToDouble(list[0]["TotalRows"]) / (double)qv.PageSize) : 0;
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
                QuestionViewModel q = new QuestionViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        q.Status = (Request.GetNumber("slStatus-" + i.ToString()));
                        q.QuestionID = new Guid(Request.Get("ckID-" + i.ToString()));
                        q.Update();
                    }
                }
                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Question" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Question?error=1");
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
                QuestionViewModel q = new QuestionViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        q.QuestionID = new Guid(Request.Get("ckID-" + i.ToString()));
                        q.Delete();
                    }
                }
                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Question" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Question?error=1");
            }
        }

    }
}
