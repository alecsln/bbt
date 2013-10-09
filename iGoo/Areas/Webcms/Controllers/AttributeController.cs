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
    public class AttributeController : DefaultController
    {
        private String per = Libs.GetPermission("ATTRIBUTE");
        public ActionResult Index()
        {
            LoadDefault();

            if (per.IndexOf("S") < 0)
                return View("NotPermission");
            //Select attribute
            AttributeViewModel av = new AttributeViewModel();
            if (!Request.IsNull("txtKey"))
                av.Name = Request.Get("txtKey");
            if (!Request.IsNull("slSearchStatus"))
                av.Status = Request.GetNumber("slSearchStatus");
            av.ParentID = Request.IsNull("ParentID") ? av.ParentID = Guid.Empty : av.ParentID = new Guid(Request.Get("ParentID"));

            av.PageIndex = Request.IsNull("page") ? 1 : Request.GetNumber("page");
            av.PageSize = Request.IsNull("show") ? 20 : Request.GetNumber("show");


            if (!Request.IsNull("OrderBy") && !Request.IsNull("Order"))
                av.OrderBy = Request.Get("OrderBy") + " " + Request.Get("Order");

            //Eidt
            if (!Request.IsNull("ID"))
            {
                av.AttributeID = new Guid(Request.Get("ID"));
                ViewBag.Edit = av.SelectOne().AsEnumerable().ToList();
                //foreach (DataRow row in av.SelectOne().Rows)
                //{
                //    ViewBag.Name = row["Name"].ToString();
                //    ViewBag.Code = row["Code"].ToString(); ;
                //    ViewBag.Value = row["Value"].ToString(); ;
                //    ViewBag.Description = row["Description"].ToString();
                //    ViewBag.Status = row["Status"].ToString();
                //}
            }

            //Page
            List<DataRow> list = av.SelectAll().AsEnumerable().ToList();
            ViewBag.Attribute = list;
            ViewBag.TotalPages = list.Count>0 ? (int)Math.Ceiling(Convert.ToDouble(list[0]["TotalRows"]) / (double)av.PageSize) : 0;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create()
        {
            try
            {
                AttributeViewModel a = new AttributeViewModel();
                a.Name = Request.Get("txtName");
                a.Code = Request.Get("txtCode");
                a.Value = Request.Get("txtValue");
                a.Description = Request.Get("txtDescription");
                a.Status = Request.GetNumber("slStatus");
                a.Order = Request.GetNumber("txtOrder");
                a.MetaKeyword = Request.Get("txtMetaKeyword");
                a.MetaDescription = Request.Get("txtMetaDescription");
                a.MetaTitle = Request.Get("txtMetaTitle");

                if (Request.IsNull("ID"))
                {
                    if (per.IndexOf("I") < 0)
                        return View("NotPermission");
                    if (Request.Get("ParentID").Equals(String.Empty))
                        a.ParentID = Guid.Empty;
                    else
                        a.ParentID = new Guid(Request.Get("ParentID"));
                    a.Created = DateTime.Now;
                    a.AttributeID = Guid.NewGuid();
                    a.Insert();
                }
                else
                {
                    if (per.IndexOf("U") < 0)
                        return View("NotPermission");
                    a.AttributeID = new Guid(Request.Get("ID"));
                    a.Update();
                }

                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Attribute" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Attribute?error=1");
            }
        }

        [HttpPost]
        public ActionResult Update()
        {
            try
            {
                if (per.IndexOf("U") < 0)
                    return View("NotPermission");
                AttributeViewModel a = new AttributeViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        a.Status = (Request.GetNumber("slStatus-" + i.ToString()));
                        a.Order = (Request.GetNumber("txtOrder-" + i.ToString()));
                        a.AttributeID = new Guid(Request.Get("ckID-" + i.ToString()));
                        a.Update();
                    }
                }

                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Attribute" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Attribute?error=1");
            }
        }

        [HttpPost]
        public ActionResult Delete()
        {
            try
            {
                if (per.IndexOf("D") < 0)
                    return View("NotPermission");
                AttributeViewModel a = new AttributeViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        a.AttributeID = new Guid(Request.Get("ckID-" + i.ToString()));
                        a.Delete();
                    }
                }

                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Attribute" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Attribute?error=1");
            }
        }

    }
}
