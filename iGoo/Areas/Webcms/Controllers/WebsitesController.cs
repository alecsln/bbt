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
    public class WebsitesController : DefaultController
    {
        private String per = Libs.GetPermission("WEBSITE");
        public ActionResult Index()
        {
            LoadDefault();

            if (per.IndexOf("S") < 0)
                return View("NotPermission");
            //Select user
            WebsiteViewModel wv = new WebsiteViewModel();
            ViewBag.Edit = wv.SelectTop().AsEnumerable().ToList();

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
                WebsiteViewModel wv = new WebsiteViewModel();
                wv.WebsiteID = new Guid(Request.Get("ID"));
                wv.Title = Request.Get("txtTitle");
                wv.MetaTitle = Request.Get("txtMetaTitle");
                wv.MetaKeyword = Request.Get("txtMetaKeyword");
                wv.MetaDescription = Request.Get("txtMetaDescription");
                wv.AddressFooter = Request.Get("txtAddressFooter");
                wv.AddressContact = Request.Get("txtAddressContact");
                wv.SEOFooter = Request.Get("txtSEOFooter");
                wv.SEOSitemap = Request.Get("txtSEOSitemap");
                wv.SEOTags = Request.Get("txtSEOTags");
                wv.Created = DateTime.Now;
                wv.Update();
              
                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Websites" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Websites?error=1");
            }
        }
    }
}
