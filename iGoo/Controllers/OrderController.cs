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
    public class OrderController : DefaultController
    {

        public ActionResult Index()
        {
            LoadDefault();

            AttributeViewModel avm = new AttributeViewModel();
            avm.Code = "STATIC_GUIDE";
            ViewBag.StaticGuide = avm.SelectByCode().AsEnumerable().ToList();

            UserViewModel uvm = new UserViewModel();
            if (Session["MemberID"] != null)
            {
                uvm.UserID = new Guid(Session["MemberID"].ToString());
                ViewBag.User = uvm.SelectOne().AsEnumerable().ToList();
            }

            ViewBag.OrderDetails = getProducts();

            return View();
        }

        public ActionResult MyCart()
        {
            CheckMember();
            ///Select default
            LoadDefault();

            UserViewModel uvm = new UserViewModel();
            if (Session["MemberID"] != null)
            {
                uvm.UserID = new Guid(Session["MemberID"].ToString());
                ViewBag.User = uvm.SelectOne().AsEnumerable().ToList();
            }

            OrderViewModel ov = new OrderViewModel();
            ov.UserID = uvm.UserID;

            ViewBag.Order = ov.SelectByUserID().AsEnumerable().ToList();

            return View();
        }

        public ActionResult Bill()
        {
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
        [ValidateInput(false)]
        public ActionResult SendOrder()
        {
            try
            {
                OrderViewModel ov = new OrderViewModel();
                List<ShoppingCart> lstSC = getProducts();
                ov.OrderID = Guid.NewGuid();
                if (!Request.IsNull("txtUserID"))
                    ov.UserID = new Guid(Request.Get("txtUserID"));
                ov.FullName = Request.Get("txtFullNameRec");
                ov.Email = Request.Get("txtEmailRec");
                ov.Address = Request.Get("txtAddressRec");
                ov.Phone = Request.Get("txtPhoneRec");
                ov.Request = Request.Get("txtRequest");
                ov.Voucher = Request.Get("txtVoucher");
                ov.Status = 0;
                ov.Created = DateTime.Now;
                ov.TotalPrice = lstSC.Sum(x => x.TotalPrice);
                ov.Insert();

                OrderDetailViewModel odv = new OrderDetailViewModel();
                foreach (ShoppingCart sc in lstSC)
                {
                    odv.OrderDetailID = Guid.NewGuid();
                    odv.OrderID = ov.OrderID;
                    odv.ProductID = new Guid(sc.ProductID);
                    odv.Price = sc.SalePrice;
                    odv.Quantity = sc.Quantity;
                    odv.ProductType = sc.Attribute;
                    odv.Created = DateTime.Now;
                    odv.Insert();
                }

                return Redirect("/Order?result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("Order?urlError=0");
            }

        }

        [HttpPost]
        public ActionResult Delete()
        {
            try
            {
                OrderDetailViewModel odv = new OrderDetailViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        odv.ProductID = new Guid(Request.Get("ckID-" + i.ToString()));
                        odv.Delete();
                    }
                }
                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Category" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Category?error=1");
            }
        }

        private List<ShoppingCart> getProducts()
        {
            //String value = "$$4554cc06-597c-4d9b-a47a-20aed6d235fb#mau:Xanh#2$$4554cc06-597c-4d9b-a47a-20aed6d235fc#mau:do#3";
            String value = Libs.ReadCookie("shopping_cart");
            List<ShoppingCart> lstSC = new List<ShoppingCart>();

            if (!String.IsNullOrEmpty(value))
            {
                String[] row = Regex.Split(value, "\\$\\$");
                String id = "";
                for (int i = 1; i < row.Length; i++)
                {
                    ShoppingCart sc = new ShoppingCart();
                    String[] arr = row[i].Split('#');
                    sc.ProductID = arr[0];
                    sc.Attribute = arr[1];
                    sc.Quantity = Convert.ToInt16(arr[2]);
                    lstSC.Add(sc);
                    id += arr[0] + ",";
                }
                if (id.IndexOf(',') > -1)
                    id = id.Substring(0, id.Length - 1);

                ProductViewModel pv = new ProductViewModel();
                if (!"".Equals(id))
                {
                    pv.Title = id;
                    foreach (DataRow row2 in pv.SelectProductInOrder().Rows)
                    {
                        ShoppingCart sc2 = lstSC.Find(x => x.ProductID == row2["ProductID"].ToString());

                        if (sc2 != null)
                        {
                            sc2.Title = row2["Title"].ToString();
                            sc2.SEOName = row2["SEOName"].ToString();
                            sc2.Image = row2["Image"].ToString();
                            sc2.SalePrice = Convert.ToDecimal(row2["SalePrice"].ToString());
                            sc2.TotalPrice = sc2.SalePrice * sc2.Quantity;
                        }
                    }
                }
            }
            return lstSC;
        }
    }

    public class ShoppingCart
    {
        public String ProductID { get; set; }

        public String Attribute { get; set; }

        public int Quantity { get; set; }

        public String Title { get; set; }

        public String SEOName { get; set; }

        public String Image { get; set; }

        public Decimal SalePrice { get; set; }

        public Decimal TotalPrice { get; set; }
 
    }
}
