using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iGoo.Helpers;
using iGoo.Models;
using System.Diagnostics;
using System.Data;

namespace iGoo.Controllers
{
    public class HomeController : DefaultController
    {

        public ActionResult Index()
        {
            String url = HttpContext.Request.Headers["HOST"];
            String domain = Libs.sApp("Domain");
            if (!url.Equals(domain))
            {
                AttributeViewModel avm = new AttributeViewModel();
                avm.Value = url;
                DataTable dt = new DataTable();
                dt = avm.SelectPageView();
                if (dt.Rows.Count > 0)
                {
                    ViewBag.PageView = dt.AsEnumerable().ToList();
                    return View("~/Views/News/PageView.cshtml");
                }
            }

            LoadDefault();

            NewsViewModel nvm = new NewsViewModel();
            
            nvm.Type = "NEW";
            nvm.Top = 6;
            ViewBag.NewsNew = nvm.SelectByType().AsEnumerable().ToList();

            AdvViewModel adv = new AdvViewModel();
            adv.CategoryID = new Guid("11111111-1111-1111-1111-111111111111");
            //adv.Type = "ADV_RIGHT";
            //ViewBag.AdvRight = adv.SelectByType().AsEnumerable().ToList();
            adv.Type = "ADV_SLIDE";
            ViewBag.AdvSlide = adv.SelectByType().AsEnumerable().ToList();

            QuestionViewModel qvm = new QuestionViewModel();
            qvm.Top = 5;
            ViewBag.QuestionNew = qvm.SelectByCreated().AsEnumerable().ToList();

            CategoryViewModel cvm = new CategoryViewModel();
            cvm.Code = "CATEGORY_PRODUCT";
            ViewBag.ProductTags = cvm.SelectByCode().AsEnumerable().ToList();
            
            cvm.Type = "LEFT";
            ViewBag.MenuLeft = cvm.SelectByType().AsEnumerable().ToList();

            //Select product
            ProductViewModel pvm = new ProductViewModel();
            pvm.Top = 8;
            pvm.Type = "SALE";
            ViewBag.ProductSale = pvm.SelectByType().AsEnumerable().ToList();
            pvm.Type = "TOP";
            ViewBag.ProductTop = pvm.SelectByType().AsEnumerable().ToList();
            pvm.Type = "NEW";
            ViewBag.ProductNew = pvm.SelectByType().AsEnumerable().ToList();

            //Select product new comment
            ViewBag.ProductNewComment = pvm.SelectByModified().AsEnumerable().ToList();

            //Select poll
            PollViewModel pv = new PollViewModel();
            pv.Code = "HOMEPAGE";
            ViewBag.Poll = pv.SelectByCode().AsEnumerable().ToList();

            return View();
        }
    }
}
