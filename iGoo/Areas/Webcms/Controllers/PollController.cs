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
    public class PollController : DefaultController
    {
        private String per = Libs.GetPermission("POLL");

        public ActionResult Index()
        {
            LoadDefault();
            if (per.IndexOf("S") < 0)
                return View("NotPermission");
            //Select Poll
            PollViewModel pv = new PollViewModel();
            if (!Request.IsNull("txtKey"))
                pv.Title = Request.Get("txtKey");
            if (!Request.IsNull("slSearchStatus"))
                pv.Status = Request.GetNumber("slSearchStatus");

            pv.PageIndex = Request.IsNull("page") ? 1 : Request.GetNumber("page");
            pv.PageSize = Request.IsNull("show") ? 20 : Request.GetNumber("show");

            //Eidt
            if (!Request.IsNull("ID"))
            {
                pv.PollID = new Guid(Request.Get("ID"));
                ViewBag.Edit = pv.SelectOne().AsEnumerable().ToList();
            }

            //Page
            List<DataRow> list = pv.SelectAll().AsEnumerable().ToList();
            ViewBag.Poll = list;
            ViewBag.TotalPages = list.Count > 0 ? (int)Math.Ceiling(Convert.ToDouble(list[0]["TotalRows"]) / (double)pv.PageSize) : 0;
            return View();
        }

        public ActionResult Search()
        {
            CheckUser();
            if (per.IndexOf("S") < 0)
                return View("NotPermission");
            //Select default
            //Select user
            ViewBag.LoginUserID = Session["UserID"];
            ViewBag.LoginFullName = Session["FullName"];
            
            //Select Poll
            PollViewModel pv = new PollViewModel();
            try
            {
                pv.PollID = new Guid(Request.Get("txtKey"));
            }
            catch (Exception ex)
            {
                pv.Title = Request.Get("txtKey");
            }
            pv.Status = Request.GetNumber("slSearchType");
            List<DataRow> list = pv.SelectRelated().AsEnumerable().ToList();
            ViewBag.Poll = list;

            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create()
        {
            try
            {
                PollViewModel pv = new PollViewModel();
                pv.Title = Request.Get("txtTitle");
                pv.Code = Request.Get("txtCode");

                pv.Response1 = Request.Get("txtResponse-1");
                pv.Vote1 = Request.GetNumber("txtVote-1");
                pv.Response2 = Request.Get("txtResponse-2");
                pv.Vote2 = Request.GetNumber("txtVote-2");
                pv.Response3 = Request.Get("txtResponse-3");
                pv.Vote3 = Request.GetNumber("txtVote-3");
                pv.Response4 = Request.Get("txtResponse-4");
                pv.Vote4 = Request.GetNumber("txtVote-4");
                pv.Response5 = Request.Get("txtResponse-5");
                pv.Vote5 = Request.GetNumber("txtVote-5");
                pv.Response6 = Request.Get("txtResponse-6");
                pv.Vote6 = Request.GetNumber("txtVote-6");
                pv.Response7 = Request.Get("txtResponse-7");
                pv.Vote7 = Request.GetNumber("txtVote-7");
                pv.Response8 = Request.Get("txtResponse-8");
                pv.Vote8 = Request.GetNumber("txtVote-8");
                pv.Response9 = Request.Get("txtResponse-9");
                pv.Vote9 = Request.GetNumber("txtVote-9");
                pv.Response10 = Request.Get("txtResponse-10");
                pv.Vote10 = Request.GetNumber("txtVote-10");

                pv.TotalVote = pv.Vote1 + pv.Vote2 + pv.Vote3 + pv.Vote4 + pv.Vote5 + 
                                pv.Vote6 + pv.Vote7 + pv.Vote8 + pv.Vote9 + pv.Vote10;

                pv.Status = Request.GetNumber("slStatus");

                if (Request.IsNull("ID"))
                {
                    pv.Created = DateTime.Now;
                    pv.PollID = Guid.NewGuid();
                    pv.Insert();
                }
                else
                {
                    pv.PollID = new Guid(Request.Get("ID"));
                    pv.Update();
                }

                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Poll" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Poll?error=1");
            }
        }

        [HttpPost]
        public ActionResult Update()
        {
            try
            {
                PollViewModel pv = new PollViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        pv.Status = (Request.GetNumber("slStatus-" + i.ToString()));
                        pv.PollID = new Guid(Request.Get("ckID-" + i.ToString()));
                        pv.Update();
                    }
                }

                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Poll" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Poll?error=1");
            }
        }

        [HttpPost]
        public ActionResult Delete()
        {
            try
            {
                PollViewModel pv = new PollViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        pv.PollID = new Guid(Request.Get("ckID-" + i.ToString()));
                        pv.Delete();
                    }
                }

                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Poll" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Poll?error=1");
            }
        }

        public ActionResult Permission()
        {
            Libs.CheckUser();
            //if (per.IndexOf("P") < 0)
            //    return View("NotPermission");
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
