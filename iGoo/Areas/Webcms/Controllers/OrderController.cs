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
    public class OrderController : DefaultController
    {
        private String per = Libs.GetPermission("ORDER");
        public ActionResult Index()
        {
            LoadDefault();

            if (per.IndexOf("S") < 0)
                return View("NotPermission");
            //Slect group
            CategoryViewModel ct = new CategoryViewModel();
            ct.Code = "CATEGORY_PRODUCT";
            ViewBag.GroupProduct = ct.SelectChild().AsEnumerable().ToList();

            AttributeViewModel at = new AttributeViewModel();
            at.Code = "ATTRIBUTE_PRODUCT_STATUS";
            ViewBag.GroupType = at.SelectChild().AsEnumerable().ToList();

            //Select news
            OrderViewModel ov = new OrderViewModel();
            if (!Request.IsNull("txtKey"))
                ov.OrderCode = Request.Get("txtKey");
            if (!Request.IsNull("slSearchStatus"))
                ov.Status = Request.GetNumber("slSearchStatus");

            ov.PageIndex = Request.IsNull("page") ? 1 : Request.GetNumber("page");
            ov.PageSize = Request.IsNull("show") ? 20 : Request.GetNumber("show");


            if (!Request.IsNull("OrderBy") && !Request.IsNull("Order"))
                ov.OrderBy = Request.Get("OrderBy") + " " + Request.Get("Order");

            //Eidt
            if (!Request.IsNull("ID"))
            {
                ov.OrderID = new Guid(Request.Get("ID"));
                ViewBag.Edit = ov.SelectOne().AsEnumerable().ToList();
            }

            //Page
            List<DataRow> list = ov.SelectAll().AsEnumerable().ToList();
            ViewBag.Order = list;
            ViewBag.TotalPages = list.Count > 0 ? (int)Math.Ceiling(Convert.ToDouble(list[0]["TotalRows"]) / (double)ov.PageSize) : 0;
            return View();
        }

        public ActionResult ProcessOrder()
        {
            LoadDefault();

            if (per.IndexOf("S") < 0)
                return View("NotPermission");
            AttributeViewModel at = new AttributeViewModel();
            at.Code = "ATTRIBUTE_PAYMENT";
            ViewBag.Payment = at.SelectChild().AsEnumerable().ToList();

            OrderDetailViewModel odv = new OrderDetailViewModel();
            odv.OrderID = new Guid(Request.Get("ID"));
            ViewBag.OrderDetails = odv.SelectOrderDetailByOrderID().AsEnumerable().ToList();

            OrderViewModel ov = new OrderViewModel();
            if (!Request.IsNull("ID"))
            {
                ov.OrderID = new Guid(Request.Get("ID"));
                ViewBag.Edit = ov.SelectOne().AsEnumerable().ToList();
            }

            return View();
        }

        [HttpPost]
        public ActionResult Update()
        {
            try
            {
                if (per.IndexOf("U") < 0)
                    return View("NotPermission");
                OrderViewModel ov = new OrderViewModel();
                OrderDetailViewModel odv = new OrderDetailViewModel();
                ov.Status = Request.GetNumber("slStatus");
                if (ov.Status == 3 || ov.Status == 4)
                {
                    for (int i = 1; i <= Request.GetNumber("count"); i++)
                    {
                        odv.OrderDetailID = new Guid(Request.Get("hID-" + i.ToString()));
                        odv.CancelOrder();
                    }
                }
                else
                {
                    ov.TotalPrice = 0;
                    for (int i = 1; i <= Request.GetNumber("count"); i++)
                    {
                        odv.Quantity = (Request.GetNumber("txtQuantity-" + i.ToString()));
                        odv.Price = (Request.GetDecimal("txtPrice-" + i.ToString()));
                        //ov.TotalPrice += odv.Quantity * odv.Price;
                        if (!Request.IsNull("ckID-" + i.ToString()))
                        {
                            odv.OrderDetailID = new Guid(Request.Get("ckID-" + i.ToString()));
                            odv.Update();
                        }
                    }
                }
                ov.OrderID = new Guid(Request.Get("OrderID"));
                ov.SaleID = new Guid(Session["UserID"].ToString());
                ov.Comment = Request.Get("txtComment");
                ov.Tax = Request.GetNumber("txtTax");
                ov.TransportFee = Request.GetDecimal("txtTransportFee");
                ov.Discount = Request.GetDecimal("txtDiscount");
                ov.OrderSend = DateTime.Now;
                if (!Request.IsNull("slPayment"))
                    ov.PaymentID = new Guid(Request.Get("slPayment"));
                ov.UpdateOrderByOrderDetail();

                return Redirect("/Webcms/Order/ProcessOrder?ID=" + Request.Get("OrderID") + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Order/ProcessOrder?ID=" + Request.Get("OrderID") + "&error=1");
            }
        }

        [HttpPost]
        public ActionResult Delete()
        {
            try
            {
                if (per.IndexOf("D") < 0)
                    return View("NotPermission");
                OrderViewModel ov = new OrderViewModel();
                int count = Request.GetNumber("count");
                for (int i = 1; i <= count; i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        ov.OrderID = new Guid(Request.Get("ckID-" + i.ToString()));
                        ov.Delete();
                    }
                }

                //OrderDetailViewModel odv = new OrderDetailViewModel();
                //for (int i = 1; i <= Request.GetNumber("count"); i++)
                //{
                //    if (!Request.IsNull("ckID-" + i.ToString()))
                //    {
                //        odv.OrderDetailID = new Guid(Request.Get("ckID-" + i.ToString()));
                //        odv.Delete();
                //    }
                //}

                //OrderViewModel ov = new OrderViewModel();
                //ov.OrderID = new Guid(Request.Get("ID"));
                //ov.SaleID = new Guid(Session["UserID"].ToString());
                //ov.Comment = Request.Get("txtComment");
                //ov.Tax = Request.GetNumber("txtTax");
                //ov.TransportFee = Request.GetDecimal("txtTransportFee");
                //ov.Discount = Request.GetDecimal("txtDiscount");
                //ov.Status = Request.GetNumber("slStatus");
                //ov.OrderSend = DateTime.Now;
                //ov.UpdateOrderByOrderDetail();

                return Redirect("/Webcms/Order?ID=" + Request.Get("OrderID") + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Order?error=1");
            }
        }

        [HttpPost]
        public bool DeleteOrderDetail()
        {
            try
            {
                OrderDetailViewModel odv = new OrderDetailViewModel();
                odv.OrderDetailID = new Guid(Request.Get("id"));
                odv.Delete();

                return true;
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return false;
            }
        }

    }
}
