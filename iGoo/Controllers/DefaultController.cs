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
    public class DefaultController : Controller
    {

        public void LoadDefault()
        {
            WebsiteViewModel wvm = new WebsiteViewModel();
            ViewBag.Website = wvm.SelectTop().AsEnumerable().ToList();

            AttributeViewModel avm = new AttributeViewModel();
            avm.Code = "STATIC_SUPPORT";
            ViewBag.StaticSupport = avm.SelectByCode().AsEnumerable().ToList();

            AdvViewModel adv = new AdvViewModel();
            adv.CategoryID = new Guid("11111111-1111-1111-1111-111111111111");
            adv.Type = "ADV_LEFT";
            ViewBag.AdvLeft = adv.SelectByType().AsEnumerable().ToList();
            adv.Type = "ADV_TOP";
            ViewBag.AdvTop = adv.SelectByType().AsEnumerable().ToList();

            //Select menu
            CategoryViewModel cvm = new CategoryViewModel();
            cvm.Type = "LEFT";
            cvm.MenuAll = cvm.SelectOptimize();
            ViewBag.Menu = cvm.SelectMenu(new Guid("87A2AADF-D5F7-455D-AF80-71ECC2B683AC"));

            cvm.Type = "MENU";
            cvm.MenuAll = cvm.SelectOptimize();
            ViewBag.MenuNews = cvm.SelectMenu(new Guid("6c8df8b0-0365-4d23-ba29-1b9681b85059"));

            cvm.Type = "HEADER";
            ViewBag.MenuHeader = cvm.SelectByType().AsEnumerable().ToList();

            if (Session["MemberID"] != null)
            {
                UserViewModel uvm = new UserViewModel();
                uvm.UserID = new Guid(Session["MemberID"].ToString());
                ViewBag.NotificationCount = uvm.SelectNotificationCountByUserID().Rows[0][0];
            }

            //Select product history
            ProductViewModel pvm = new ProductViewModel();
            String product_history = Libs.ReadCookie("product_history");
            if (!String.IsNullOrEmpty(product_history))
            {
                product_history = product_history.Substring(1, product_history.Length - 1);
                pvm.List = product_history;
                ViewBag.ProductHistory = pvm.SelectByList().AsEnumerable().ToList();
            }

            //Check Goooglebot
            if (Request.ServerVariables["HTTP_USER_AGENT"].Contains("Googlebot"))
            {
                Libs.WriteGooglebot(Request.Url.AbsoluteUri);
            }
        }

        public void CheckMember()
        {
            if (Session["MemberID"] == null)
                Response.Redirect("/info/login");
            return;
        }
    }
}
