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
    public class ContactController : DefaultController
    {
        private String per = Libs.GetPermission("CONTACT");
        public ActionResult Index()
        {
            LoadDefault();
            //End select default

            AttributeViewModel at = new AttributeViewModel();
            at.Code = "ATTRIBUTE_CONTACT";
            ViewBag.GroupContact = at.SelectChild().AsEnumerable().ToList();

            ContactViewModel cvm = new ContactViewModel();
            if (!Request.IsNull("txtKey"))
                cvm.Subject = Request.Get("txtKey");
            if (!Request.IsNull("slSearchStatus"))
                cvm.Status = Request.GetNumber("slSearchStatus");
            if (!Request.IsNull("slSearchGroupContact"))
                cvm.GroupID = new Guid(Request.Get("slSearchGroupContact"));

            cvm.PageIndex = Request.IsNull("page") ? 1 : Request.GetNumber("page");
            cvm.PageSize = Request.IsNull("show") ? 20 : Request.GetNumber("show");

            if (!Request.IsNull("OrderBy") && !Request.IsNull("Order"))
                cvm.OrderBy = Request.Get("OrderBy") + " " + Request.Get("Order");

            //Page
            List<DataRow> list = cvm.SelectAll().AsEnumerable().ToList();
            ViewBag.Contact = list;
            ViewBag.TotalPages = list.Count > 0 ? (int)Math.Ceiling(Convert.ToDouble(list[0]["TotalRows"]) / (double)cvm.PageSize) : 0;
            return View();
        }

        public ActionResult Edit()
        {
            LoadDefault();
            //End select default

            if (per.IndexOf("s") < 0)
                return View("NotPermission");
            AttributeViewModel at = new AttributeViewModel();
            at.Code = "ATTRIBUTE_CONTACT";
            ViewBag.GroupContact = at.SelectChild().AsEnumerable().ToList();

            ContactViewModel cvm = new ContactViewModel();
            //Eidt
            if (!Request.IsNull("ID"))
            {
                cvm.ContactID = new Guid(Request.Get("ID"));
                cvm.Status = 1;
                cvm.Update();
                ViewBag.Edit = cvm.SelectOne().AsEnumerable().ToList();
            }
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create()
        {
            try
            {
                if (per.IndexOf("U") < 0)
                    return View("NotPermission");
                if (!Request.IsNull("ID") && !Request.IsNull("slSearchGroupContact"))
                {
                    ContactViewModel ct = new ContactViewModel();
                    ct.ContactID = new Guid(Request.Get("ID"));
                    ct.GroupID = new Guid(Request.Get("slSearchGroupContact"));
                    ct.Update();
                }
                return Redirect("/Webcms/Contact?result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Contact?error=1");
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
                ContactViewModel nv = new ContactViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        nv.ContactID = new Guid(Request.Get("ckID-" + i.ToString()));
                        nv.Status = (Request.GetNumber("slStatus-" + i.ToString()));
                        nv.Update();
                    }
                }

                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Contact" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Contact?error=1");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Reply()
        {
            try
            {
                if (per.IndexOf("U") < 0)
                    return View("NotPermission");
                ContactViewModel cvm = new ContactViewModel();
                if (Libs.SendMail(Request.Get("txtEmail").Trim(), Request.Get("txtSubjectReply"), Request.Get("txtContentReply")))
                {
                    cvm.ContactID = new Guid(Request.Get("ID"));
                    cvm.UserID = new Guid(Session["UserID"].ToString());
                    cvm.SubjectReply = Request.Get("txtSubjectReply");
                    cvm.ContentReply = Request.Get("txtContentReply");
                    cvm.DateReply = DateTime.Now;
                    cvm.Status = 2;
                    cvm.Update();
                    return Redirect("/Webcms/Contact?result=1");
                }
                else
                    return Redirect("/Webcms/Edit?error=0");
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
                ContactViewModel nv = new ContactViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        nv.ContactID = new Guid(Request.Get("ckID-" + i.ToString()));
                        nv.Delete();
                    }
                }

                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Contact" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/News?error=1");
            }
        }

    }
}
