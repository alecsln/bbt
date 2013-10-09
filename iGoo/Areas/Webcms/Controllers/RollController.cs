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
    public class RollController : DefaultController
    {
        private String per = Libs.GetPermission("ROLL");

        public ActionResult Index()
        {
            Libs.CheckUser();

            if (per.IndexOf("S") < 0)
                return View("NotPermission");

            //Select default
            //Select user
            ViewBag.LoginUserID = Session["UserID"];
            ViewBag.LoginFullName = Session["FullName"];

            //Select menu
            ModuleViewModel mv = new ModuleViewModel();
            mv.MenuAll = mv.SelectAll();
            ViewBag.Menu = mv.SelectMenu(Guid.Empty);
            //End select default

            //Select Roll
            RollViewModel rv = new RollViewModel();
            if (!Request.IsNull("txtKey"))
                rv.Name = Request.Get("txtKey");
            if (!Request.IsNull("slSearchStatus"))
                rv.Status = Request.GetNumber("slSearchStatus");

            rv.PageIndex = Request.IsNull("page") ? 1 : Request.GetNumber("page");
            rv.PageSize = Request.IsNull("show") ? 20 : Request.GetNumber("show");


            if (!Request.IsNull("OrderBy") && !Request.IsNull("Order"))
                rv.OrderBy = Request.Get("OrderBy") + " " + Request.Get("Order");

            //Eidt
            if (!Request.IsNull("ID"))
            {
                rv.RollID = new Guid(Request.Get("ID"));
                ViewBag.Edit = rv.SelectOne().AsEnumerable().ToList();
            }

            //Page
            List<DataRow> list = rv.SelectAll().AsEnumerable().ToList();
            ViewBag.Roll = list;
            ViewBag.TotalPages = list.Count > 0 ? (int)Math.Ceiling(Convert.ToDouble(list[0]["TotalRows"]) / (double)rv.PageSize) : 0;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create()
        {
            try
            {
                RollViewModel r = new RollViewModel();
                r.Name = Request.Get("txtName");
                r.Description = Request.Get("txtDescription");
                r.Status = Request.GetNumber("slStatus");

                if (Request.IsNull("ID"))
                {
                    if (per.IndexOf("I") < 0)
                        return View("NotPermission");
                    r.Created = DateTime.Now;
                    r.RollID = Guid.NewGuid();
                    r.Insert();
                }
                else
                {
                    if (per.IndexOf("U") < 0)
                        return View("NotPermission");
                    r.RollID = new Guid(Request.Get("ID"));
                    r.Update();
                }

                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Roll" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Roll?error=1");
            }
        }

        [HttpPost]
        public ActionResult Update()
        {
            try
            {
                if (per.IndexOf("U") < 0)
                    return View("NotPermission");
                RollViewModel r = new RollViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        r.RollID = new Guid(Request.Get("ckID-" + i.ToString()));
                        r.Status = (Request.GetNumber("slStatus-" + i.ToString()));
                        r.Update();
                    }
                }

                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Roll" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Roll?error=1");
            }
        }

        [HttpPost]
        public ActionResult Delete()
        {
            try
            {
                if (per.IndexOf("D") < 0)
                    return View("NotPermission");
                RollViewModel r = new RollViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        r.RollID = new Guid(Request.Get("ckID-" + i.ToString()));
                        r.Delete();
                    }
                }

                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Roll" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Roll?error=1");
            }
        }

        public ActionResult Permission()
        {
            Libs.CheckUser();
            if (per.IndexOf("P") < 0)
                return View("NotPermission");
            ///Select default
            //Select user
            ViewBag.LoginUserID = Session["UserID"];
            ViewBag.LoginFullName = Session["FullName"];

            //Select menu
            ModuleViewModel mv = new ModuleViewModel();
            mv.MenuAll = mv.SelectAll();
            ViewBag.Menu = mv.SelectMenu(Guid.Empty);
            //End select default

            //Slect permission
            AttributeViewModel at = new AttributeViewModel();
            at.Code = "ATTRIBUTE_PERMISSION_ADMIN";
            ViewBag.Permission = at.SelectChild().AsEnumerable().ToList();

            mv.RollID = new Guid(Request.Get("ID"));
            ViewBag.Module = mv.SelectModule().AsEnumerable().ToList();

            return View();
        }

        [HttpPost]
        public ActionResult UpdatePermission()
        {
            try
            {
                if (per.IndexOf("P") < 0)
                    return View("NotPermission");
                Guid rollID = new Guid(Request.Get("RollID"));
                //Select RollModules by RollID
                RollViewModel rvm = new RollViewModel();
                rvm.RollID = rollID;
                var list = rvm.SelectRollModulesByRollID().AsEnumerable();

                String[] p = Request.Get("hPermission").Split(',');
                String[] id = Request.Get("hID").Split(',');

                clsADM_RollModules rm = new clsADM_RollModules();

                for (int i = 0; i < id.Length; i++)
                {
                    String pm = String.Empty;
                    for (int j = 0; j < p.Length; j++)
                    {
                        if (!String.Empty.Equals(Request.Get(p[j] + "_" + id[i])))
                            pm += Request.Get(p[j] + "_" + id[i]) + ",";
                    }
                    if (!String.Empty.Equals(pm))
                    {
                        pm = pm.Substring(0, pm.Length - 1);
                    }
                    if (list.Where(a => a["ModuleID"].ToString() == id[i]).Count() > 0)
                    {
                        if (list.Where(a => a["ModuleID"].ToString() == id[i] && a["Permission"].ToString() == pm).Count() == 0)
                        {
                            rvm.ModuleID = new Guid(id[i]);
                            rvm.Permission = pm;
                            rvm.RollID = rollID;
                            rvm.UpdateRollModules();
                        }
                    }
                    else
                    {
                        rm.RollID = rollID;
                        rm.ModuleID = new Guid(id[i]);
                        rm.Permission = pm;
                        rm.Insert();
                    }
                }
                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Roll?result=1");
            }
            catch (Exception ex)
            {
                return Redirect("/Webcms/Roll?error=1");
            }
        }
    }
}
