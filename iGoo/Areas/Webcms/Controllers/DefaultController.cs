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
    public class DefaultController : Controller
    {
        public void LoadDefault()
        {
            CheckUser();
            ///Select default

            //Select menu
            ModuleViewModel mv = new ModuleViewModel();
            mv.MenuAll = mv.SelectAll();
            ViewBag.Menu = mv.SelectMenu(Guid.Empty);
            //End select default
        }

        public void CheckUser()
        {
            if (Session["UserID"] == null)
                Response.Redirect("/Webcms");
            return;
        }

    }
}
