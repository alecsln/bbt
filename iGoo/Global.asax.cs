using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data;
using iGoo.Classes;
using iGoo.Models;
using System.Text.RegularExpressions;
using iGoo.Helpers;

namespace iGoo
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //Default
            routes.MapRoute("Default", "", new { controller = "Home", action = "Index" }, new string[] { "iGoo.Controllers" });
            //routes.MapRoute("Default", "{controller}/{action}", new { controller = "Home", action = "Index" }, new string[] { "iGoo.Controllers" });
            //Action
            routes.MapRoute("ActionLogin", "info/{action}/{page}", new { controller = "Info", action = "Index", page = "1" }, new string[] { "iGoo.Controllers" });
            routes.MapRoute("ActionContact", "contact/{action}", new { controller = "Contact", action = "Index"}, new string[] { "iGoo.Controllers" });
            routes.MapRoute("ActionNews", "news/{action}", new { controller = "News", action = "Index" }, new string[] { "iGoo.Controllers" });
            routes.MapRoute("ActionProduct", "product/{action}", new { controller = "Product", action = "Index" }, new string[] { "iGoo.Controllers" });
            routes.MapRoute("ActionAnswer", "answer/{action}", new { controller = "Answer", action = "Index" }, new string[] { "iGoo.Controllers" });
            routes.MapRoute("ActionOrder", "order/{action}", new { controller = "Order", action = "Index" }, new string[] { "iGoo.Controllers" });
            routes.MapRoute("ActionPoll", "poll/{action}", new { controller = "Poll", action = "Index" }, new string[] { "iGoo.Controllers" });
            //News Rss
            routes.MapRoute("NewsRss", "{title}.rss", new { controller = "News", action = "Rss" }, new string[] { "iGoo.Controllers" });
            //News sitemap
            routes.MapRoute("NewsSiteMap", "sitemap/news/{page}", new { controller = "News", action = "SiteMap", page = "1" }, new string[] { "iGoo.Controllers" });
            routes.MapRoute("NewsSiteMapXML", "sitemap/news.xml", new { controller = "News", action = "SiteMapXML"}, new string[] { "iGoo.Controllers" });
            routes.MapRoute("WebSiteMapXML", "sitemap.xml", new { controller = "News", action = "WebSiteMapXML" }, new string[] { "iGoo.Controllers" });
            //Image thumb
            routes.MapRoute("ImageThumb", "thumb/{date}/{file}-{width}x{height}.{ext}", new { controller = "Thumbnail", action = "ResizeImage" }, new string[] { "iGoo.Controllers" });
            //Answer sitemap
            routes.MapRoute("AnswerSiteMap", "sitemap/answer/{page}", new { controller = "Answer", action = "SiteMap", page = "1" }, new string[] { "iGoo.Controllers" });
            routes.MapRoute("AnswerSiteMapXML", "sitemap/answer.xml", new { controller = "Answer", action = "SiteMapXML"}, new string[] { "iGoo.Controllers" });
            //Google search
            routes.MapRoute("GoogleSearch", "search", new { controller = "News", action = "Search"}, new string[] { "iGoo.Controllers" });
            //404
            routes.MapRoute("404", "404.html", new { controller = "News", action = "Error" }, new string[] { "iGoo.Controllers" });
            //Tags news
            routes.MapRoute("TagsNews", "tags/{title}", new { controller = "News", action = "TagsNews", title = string.Empty }, new string[] { "iGoo.Controllers" });
            //Tags answer
            routes.MapRoute("TagsAnswer", "hoidap/tags/{title}", new { controller = "Answer", action = "TagsAnswer", title = string.Empty }, new string[] { "iGoo.Controllers" });
            //Tags product
            routes.MapRoute("TagsProduct", "p/tags/{title}", new { controller = "Product", action = "TagsProduct", title = string.Empty }, new string[] { "iGoo.Controllers" });
            //News view
            routes.MapRoute("NewsView", "{title}.html", new { controller = "News", action = "View" }, new string[] { "iGoo.Controllers" });
            //Page view
            routes.MapRoute("PageView", "s/{title}", new { controller = "News", action = "PageView" }, new string[] { "iGoo.Controllers" });
            //Answer Rss
            routes.MapRoute("AnswerRss", "hoidap/{title}.rss", new { controller = "Answer", action = "Rss" }, new string[] { "iGoo.Controllers" });
            //Answer view
            routes.MapRoute("AnswerView", "hoidap/{title}.html", new { controller = "Answer", action = "View" }, new string[] { "iGoo.Controllers" });
            //Category Answer
            routes.MapRoute("HomeAnswer", "hoidap", new { controller = "Answer", action = "Index"}, new string[] { "iGoo.Controllers" });
            routes.MapRoute("CateAnswer", "hoidap/{title}/{page}", new { controller = "Answer", action = "List", page = "1" }, new string[] { "iGoo.Controllers" });
            //Prodcut view
            routes.MapRoute("ProductView", "p/{title}.html", new { controller = "Product", action = "View" }, new string[] { "iGoo.Controllers" });
            //Product Rss
            routes.MapRoute("ProductRss", "p/{title}.rss", new { controller = "Product", action = "Rss" }, new string[] { "iGoo.Controllers" });
            //Product sitemap
            routes.MapRoute("ProductSiteMap", "sitemap/product/{page}", new { controller = "Product", action = "SiteMap", page = "1" }, new string[] { "iGoo.Controllers" });
            routes.MapRoute("ProductSiteMapXML", "sitemap/product.xml", new { controller = "Product", action = "SiteMapXML"}, new string[] { "iGoo.Controllers" });
            //Category prodcut
            routes.MapRoute("CateProduct", "p/{title}/{page}", new { controller = "Product", action = "Index", page = "1" }, new string[] { "iGoo.Controllers" });
            //Username
            routes.MapRoute("UserName", "u/{title}/{page}", new { controller = "Info", action = "UserName", page = "1" }, new string[] { "iGoo.Controllers" });
            //Contact
            routes.MapRoute("Contact", "contact", new { controller = "Contact", action = "Index" }, new string[] { "iGoo.Controllers" });
            //Order product
            routes.MapRoute("Order", "order", new { controller = "Order", action = "Index" }, new string[] { "iGoo.Controllers" });
            //Category news
            routes.MapRoute("CateNews", "{title}/{page}", new { controller = "News", action = "Index",page="1"}, new string[] { "iGoo.Controllers" });
            
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_BeginRequest()
        {
            String url = HttpContext.Current.Request.Headers["HOST"];
            String domain = Libs.sApp("Domain");
            int index = url.IndexOf("www." + domain);
            if (index >=0)
            {
                HttpContext.Current.Response.StatusCode = 301;
                HttpContext.Current.Response.AppendHeader("Location", "http://"+domain);
            }
        }
    }
}