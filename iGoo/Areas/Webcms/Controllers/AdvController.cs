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
    public class AdvController : DefaultController
    {
        private String per = Libs.GetPermission("ADV");
        public ActionResult Index()
        {
            LoadDefault();
            if (per.IndexOf("S") < 0)
                return View("NotPermission");
            //Slect group
            AttributeViewModel at = new AttributeViewModel();
            at.Code = "ATTRIBUTE_ADV";
            ViewBag.GroupAttribute = at.SelectChild().AsEnumerable().ToList();

            CategoryViewModel ct = new CategoryViewModel();
            ct.Code = "CATEGORY_NEWS";
            ViewBag.GroupCategory = ct.SelectChild().AsEnumerable().ToList();
            ct.Code = "CATEGORY_GOOGLE";
            ViewBag.GroupGoogle = ct.SelectChild().AsEnumerable().ToList();

            //Select user
            AdvViewModel avm = new AdvViewModel();
            if (!Request.IsNull("txtKey"))
                avm.Title = Request.Get("txtKey");
            if (!Request.IsNull("slSearchStatus"))
                avm.Status = Request.GetNumber("slSearchStatus");
            if (!Request.IsNull("slType"))
                avm.Type = Request.Get("slType");

            avm.PageIndex = Request.IsNull("page") ? 1 : Request.GetNumber("page");
            avm.PageSize = Request.IsNull("show") ? 20 : Request.GetNumber("show");


            if (!Request.IsNull("OrderBy") && !Request.IsNull("Order"))
                avm.OrderBy = Request.Get("OrderBy") + " " + Request.Get("Order");


            //Eidt
            if (!Request.IsNull("ID"))
            {
                avm.AdvID = new Guid(Request.Get("ID"));
                ViewBag.Edit = avm.SelectOne().AsEnumerable().ToList();

            }

            //Page
            List<DataRow> list = avm.SelectAll().AsEnumerable().ToList();
            ViewBag.Advs = list;
            ViewBag.TotalPages = list.Count > 0 ? (int)Math.Ceiling(Convert.ToDouble(list[0]["TotalRows"]) / (double)avm.PageSize) : 0;
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create()
        {
            try
            {
                AdvViewModel a = new AdvViewModel();
                a.Title = Request.Get("txtTitle");
                a.File = Request.Get("txtFile");
                a.UrlLink = Request.Get("txtUrlLink");
                a.Width = Request.GetNumber("txtWidth");
                a.Height = Request.GetNumber("txtHeight");
                a.Target = Request.GetNumber("slTarget");
                a.Order = Request.GetNumber("txtOrder");
                a.CategoryID = new Guid(Request.Get("slCategory"));
                a.Type = Request.Get("slType");
                a.Status = Request.GetNumber("slStatus");

                if (Request.IsNull("ID"))
                {
                    if (per.IndexOf("I") < 0)
                        return View("NotPermission");
                    a.Created = DateTime.Now;
                    a.AdvID = Guid.NewGuid();
                    a.Insert();
                }
                else
                {
                    if (per.IndexOf("U") < 0)
                        return View("NotPermission");
                    a.AdvID = new Guid(Request.Get("ID"));
                    a.Update();
                }

                return Redirect("/Webcms/Adv?result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Adv?error=1");
            }
        }

        [HttpPost]
        public ActionResult Update()
        {
            try
            {
                if (per.IndexOf("U") < 0)
                    return View("NotPermission");
                AdvViewModel a = new AdvViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        a.Status = (Request.GetNumber("slStatus-" + i.ToString()));
                        a.Order = (Request.GetNumber("txtOrder-" + i.ToString()));
                        a.AdvID = new Guid(Request.Get("ckID-" + i.ToString()));
                        a.Update();
                    }
                }

                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Adv?result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Adv?error=1");
            }
        }

        [HttpPost]
        public ActionResult Delete()
        {
            try
            {
                if (per.IndexOf("D") < 0)
                    return View("NotPermission");
                AdvViewModel a = new AdvViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        a.AdvID = new Guid(Request.Get("ckID-" + i.ToString()));
                        a.Delete();
                    }
                }

                return Redirect("/Webcms/Adv?result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Adv?error=1");
            }
        }

    }
}
