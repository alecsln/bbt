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
    public class ProductController : DefaultController
    {
        private String per = Libs.GetPermission("PRODUCT");
        public ActionResult Index()
        {
            LoadDefault();
            if (per.IndexOf("S") < 0)
                return View("NotPermission");
            //Slect group
            CategoryViewModel ct = new CategoryViewModel();
            //ct.Code = "CATEGORY_PRODUCT";
            //ViewBag.GroupProduct = ct.SelectChild().AsEnumerable().ToList();
            ct.MenuAll = ct.SelectOptimize();
            ViewBag.MenuCate = ct.SelectMenu(new Guid("87A2AADF-D5F7-455D-AF80-71ECC2B683AC"));

            AttributeViewModel at = new AttributeViewModel();
            at.Code = "ATTRIBUTE_PRODUCT_STATUS";
            ViewBag.GroupType = at.SelectChild().AsEnumerable().ToList();

            //Select news
            ProductViewModel pv = new ProductViewModel();
            if (!Request.IsNull("txtKey"))
                pv.Title = Request.Get("txtKey");
            if (!Request.IsNull("slSearchCate"))
                pv.CategoryID = new Guid(Request.Get("slSearchCate"));
            if (!Request.IsNull("slSearchType"))
                pv.Type = Request.Get("slSearchType");
            if (!Request.IsNull("slSearchStatus"))
                pv.Status = Request.GetNumber("slSearchStatus");

            pv.PageIndex = Request.IsNull("page") ? 1 : Request.GetNumber("page");
            pv.PageSize = Request.IsNull("show") ? 20 : Request.GetNumber("show");


            if (!Request.IsNull("OrderBy") && !Request.IsNull("Order"))
                pv.OrderBy = Request.Get("OrderBy") + " " + Request.Get("Order");

            //Eidt
            if (!Request.IsNull("ID"))
            {
                pv.ProductID = new Guid(Request.Get("ID"));
                ViewBag.Edit = pv.SelectOne().AsEnumerable().ToList();
            }

            //Page
            List<DataRow> list = pv.SelectAll().AsEnumerable().ToList();
            ViewBag.Product = list;
            ViewBag.TotalPages = list.Count > 0 ? (int)Math.Ceiling(Convert.ToDouble(list[0]["TotalRows"]) / (double)pv.PageSize) : 0;
            return View();
        }

        public ActionResult Add()
        {
            LoadDefault();

            if (per.IndexOf("I") < 0)
                return View("NotPermission");
            //Slect group
            CategoryViewModel ct = new CategoryViewModel();
            //ct.Code = "CATEGORY_PRODUCT";
            //ViewBag.GroupProduct = ct.SelectChild().AsEnumerable().ToList();
            ct.MenuAll = ct.SelectOptimize();
            ViewBag.MenuCate = ct.SelectMenu(new Guid("87A2AADF-D5F7-455D-AF80-71ECC2B683AC"));

            AttributeViewModel at = new AttributeViewModel();
            at.Code = "ATTRIBUTE_MANUFACTURE";
            ViewBag.ManuFacture = at.SelectChild().AsEnumerable().ToList();
            at.Code = "ATTRIBUTE_PRODUCT_STATUS";
            ViewBag.Type = at.SelectChild().AsEnumerable().ToList();
            at.Code = "ATTRIBUTE_PRODUCT";
            ViewBag.Filter = at.SelectChild().AsEnumerable().ToList();

            //Select user
            ProductViewModel pv = new ProductViewModel();

            //Eidt
            if (!Request.IsNull("ID"))
            {
                pv.ProductID = new Guid(Request.Get("ID"));
                ViewBag.Edit = pv.SelectOne().AsEnumerable().ToList();

            }

            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create()
        {
            try
            {
                CheckUser();
                ProductViewModel pv = new ProductViewModel();
                pv.CategoryID = new Guid(Request.Get("slSearchCate"));
                pv.UserID = new Guid(Session["UserID"].ToString());
                pv.ManufactureID = new Guid(Request.Get("slManuFacture"));
                pv.Title = Request.Get("txtTitle");
                pv.SEOName = Request.Get("txtSEOName");
                pv.Image = Request.Get("txtImage");
                pv.Brief = Request.Get("txtBrief");
                pv.Content = Request.Get("txtContent");
                pv.MetaTitle = Request.Get("txtMetaTitle");
                pv.MetaKeyword = Request.Get("txtMetaKeyword");
                pv.MetaDescription = Request.Get("txtMetaDescription");
                pv.Tags = Libs.ReplaceSpace("  ", " ", Request.Get("txtTags"));
                pv.TagsTitle = Libs.ReplaceSpace("  ", " ", Request.Get("txtTagsTitle"));
                pv.Related = Request.Get("txtRelated");
                pv.Type = Request.Get("ckType").Replace(',', '#');
                pv.Status = Request.GetNumber("slStatus");
                pv.SlideImage = Request.Get("txtSlideImage");
                pv.SKU = Request.Get("txtSKU");
                pv.Quantity = Request.GetNumber("txtQuantity");
                pv.ImportPrice = Request.GetDecimal("txtImportPrice");
                pv.RealPrice = Request.GetDecimal("txtRealPrice");
                pv.SalePrice = Request.GetDecimal("txtSalePrice");
                pv.DiscountPercent = Request.GetNumber("txtDiscountPercent");
                pv.Model = Request.Get("txtModel");
                pv.Filter = Request.Get("ckFilter").Replace(',', '#');
                pv.ProductAttribute = Request.Get("txtProductAttribute");
                pv.TransportFee = Request.Get("txtTransportFee");
                pv.Promotion = Request.Get("txtPromotion");
                pv.Parameter = Request.Get("txtParameter");
                if (!Request.IsNull("txtOrder"))
                    pv.Order = Request.GetNumber("txtOrder");
                else
                    pv.Order = 999;
                if (!Request.IsNull("txtPollID"))
                    pv.PollID = new Guid(Request.Get("txtPollID"));
                if (Request.IsNull("ID"))
                {
                    if (per.IndexOf("I") < 0)
                        return View("NotPermission");
                    pv.Visitor = 0;
                    pv.TotalComment = 0;
                    pv.Created = DateTime.Now;
                    pv.ProductID = Guid.NewGuid();
                    pv.Insert();
                }
                else
                {
                    if (per.IndexOf("U") < 0)
                        return View("NotPermission");
                    if (!Request.IsNull("ckRefresh"))
                        pv.Created = DateTime.Now;
                    pv.ProductID = new Guid(Request.Get("ID"));
                    pv.Update();
                }

                return Redirect("/Webcms/Product/Add?ID=" + Request.Get("ID") + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Product/Add?ID=" + Request.Get("ID")+"&error=1");
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
                ProductViewModel pv = new ProductViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        pv.ProductID = new Guid(Request.Get("ckID-" + i.ToString()));
                        pv.Status = (Request.GetNumber("slStatus-" + i.ToString()));
                        if (!Request.IsNull("txtOrder-" + i.ToString()))
                            pv.Order = (Request.GetNumber("txtOrder-" + i.ToString()));
                        else
                            pv.Order = 999;
                        pv.Update();
                    }
                }

                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Product" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Product?error=1");
            }
        }

        [HttpPost]
        public ActionResult Delete()
        {
            try
            {
                if (per.IndexOf("D") < 0)
                    return View("NotPermission");
                ProductViewModel pv = new ProductViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        pv.ProductID = new Guid(Request.Get("ckID-" + i.ToString()));
                        pv.Delete();
                    }
                }

                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Product" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Product?error=1");
            }
        }


    }
}
