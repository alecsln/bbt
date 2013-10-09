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
    public class ContactController : DefaultController
    {
        public ActionResult Index()
        {
            LoadDefault();
            AttributeViewModel at = new AttributeViewModel();
            at.Code = "ATTRIBUTE_CONTACT";
            ViewBag.GroupContact = at.SelectChild().AsEnumerable().ToList();

            CategoryViewModel cvm = new CategoryViewModel();
            cvm.Type = "LEFT";
            ViewBag.MenuLeft = cvm.SelectByType().AsEnumerable().ToList();

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddContact()
        {
            try
            {
                clsCMS_Contacts ct = new clsCMS_Contacts();
                ct.ContactID = Guid.NewGuid();
                ct.FullName = Request.Get("txtFullName");
                ct.Email = Request.Get("txtEmail");
                ct.Phone = Request.Get("txtPhone");
                ct.Subject = Request.Get("txtSubject");
                ct.Content = Request.Get("txtContent");
                if (!Request.IsNull("slSearchGroupContact"))
                    ct.GroupID = new Guid(Request.Get("slSearchGroupContact"));
                ct.Status = 0;
                ct.Created = DateTime.Now;
                ct.Insert();
                return Redirect("?urlResult=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("?urlError=1");
            }
        }

    }
}
