using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iGoo.Models;
using iGoo.Helpers;
using System.Data;
using iGoo.Classes;
using System.Text.RegularExpressions;

namespace iGoo.Controllers
{
    public class PollController : DefaultController
    {
        public ActionResult Index()
        {
            PollViewModel pv = new PollViewModel();
            if (!Request.IsNull("ID"))
            {
                pv.PollID = new Guid(Request.Get("ID"));
                if (!Request.IsNull("vote"))
                {
                    pv.Status = Request.GetNumber("vote");
                    pv.UpdateVote();
                }
                ViewBag.Poll = pv.SelectOne().AsEnumerable().ToList();
            }
            return View();
        }

    }
}
