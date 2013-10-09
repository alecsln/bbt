using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iGoo.Areas.Webcms.Models;
using iGoo.Helpers;
using System.Diagnostics;
using System.Data;
using iGoo.Classes;
using System.Text;

namespace iGoo.Areas.Webcms.Controllers
{
    public class CategoryController : DefaultController
    {
        private String per = Libs.GetPermission("CATEGORY");
        public ActionResult Index()
        {
            LoadDefault();
            //End select default

            if (per.IndexOf("S") < 0)
                return View("NotPermission");
            //Slect group
            AttributeViewModel at = new AttributeViewModel();
            at.Code = "ATTRIBUTE_CATEGORY";
            ViewBag.Local = at.SelectChild().AsEnumerable().ToList();

            //Select Category
            CategoryViewModel cv = new CategoryViewModel();
            cv.MenuAll = cv.SelectOptimize();
            ViewBag.MenuCate = cv.SelectMenu(new Guid("00000000-0000-0000-0000-000000000000"));

            if (!Request.IsNull("txtKey"))
                cv.Name = Request.Get("txtKey");
            if (!Request.IsNull("slSearchStatus"))
                cv.Status = Request.GetNumber("slSearchStatus");
            cv.ParentID = Request.IsNull("ParentID") ? cv.ParentID = Guid.Empty : cv.ParentID = new Guid(Request.Get("ParentID"));

            cv.PageIndex = Request.IsNull("page") ? 1 : Request.GetNumber("page");
            cv.PageSize = Request.IsNull("show") ? 20 : Request.GetNumber("show");

            if (!Request.IsNull("OrderBy") && !Request.IsNull("Order"))
                cv.OrderBy = Request.Get("OrderBy") + " " + Request.Get("Order");

            //Eidt
            if (!Request.IsNull("ID"))
            {
                cv.CategoryID = new Guid(Request.Get("ID"));
                ViewBag.Edit = cv.SelectOne().AsEnumerable().ToList();
            }

            //Page
            List<DataRow> list = cv.SelectAll().AsEnumerable().ToList();
            ViewBag.Category = list;
            ViewBag.TotalPages = list.Count > 0 ? (int)Math.Ceiling(Convert.ToDouble(list[0]["TotalRows"]) / (double)cv.PageSize) : 0;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create()
        {
            try
            {
                CategoryViewModel c = new CategoryViewModel();
                c.Name = Request.Get("txtName");
                c.Code = Request.Get("txtCode");
                c.Description = Request.Get("txtDescription");
                c.Status = Request.GetNumber("slStatus");
                c.UrlLink = Request.Get("txtUrlLink");
                c.SEOName = Request.Get("txtSEOName");
                c.ClassName = Request.Get("txtClassName");
                String tag = ReplaceSpace(Request.Get("txtTags"));
                tag = tag.Replace(" ,", ",");
                tag = tag.Replace(", ", ",");
                c.SEOTags = Request.Get("txtSEOTags");
                c.Image = Request.Get("txtImage");
                c.Type = Request.Get("txtType");
                c.MetaTitle = Request.Get("txtMetaTitle");
                c.MetaKeyword = Request.Get("txtMetaKeyword");
                c.MetaDescription = Request.Get("txtMetaDescription");
                c.Order = Request.GetNumber("txtOrder");
                c.Type = Request.Get("ckType").Replace(',', '#');
                if (Request.IsNull("ID"))
                {
                    if (per.IndexOf("I") < 0)
                        return View("NotPermission");
                    if (!Request.IsNull("slSearchCate"))
                        c.ParentID = new Guid(Request.Get("slSearchCate"));
                    else if (Request.Get("ParentID").Equals(String.Empty))
                        c.ParentID = Guid.Empty;
                    else
                        c.ParentID = new Guid(Request.Get("ParentID"));
                    c.Created = DateTime.Now;
                    c.CategoryID = Guid.NewGuid();
                    c.Insert();
                }
                else
                {
                    if (per.IndexOf("U") < 0)
                        return View("NotPermission");
                    c.CategoryID = new Guid(Request.Get("ID"));
                    if (!Request.IsNull("slSearchCate"))
                        c.ParentID = new Guid(Request.Get("slSearchCate"));
                    c.Update();
                }

                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Category" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Category?error=1");
            }
        }

        [HttpPost]
        public ActionResult Update()
        {
            try
            {
                if (per.IndexOf("U") < 0)
                    return View("NotPermission");
                CategoryViewModel c = new CategoryViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        c.Status = (Request.GetNumber("slStatus-" + i.ToString()));
                        c.Order = (Request.GetNumber("txtOrder-" + i.ToString()));
                        c.CategoryID = new Guid(Request.Get("ckID-" + i.ToString()));
                        c.Update();
                    }
                }

                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Category" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Category?error=1");
            }
        }

        [HttpPost]
        public ActionResult Delete()
        {
            try
            {
                if (per.IndexOf("D") < 0)
                    return View("NotPermission");
                CategoryViewModel c = new CategoryViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        c.CategoryID = new Guid(Request.Get("ckID-" + i.ToString()));
                        c.Delete();
                    }
                }

                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Category" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Category?error=1");
            }
        }

        private String ReplaceSpace(String str)
        {
            if (str.IndexOf("  ") > -1)
            {
                str = str.Replace("  ", " ");
                return ReplaceSpace(str);
            }
            return str;
        }
    }
}
