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
    public class MemberController : DefaultController
    {
        private String per = Libs.GetPermission("MEMBER");
        public ActionResult Index()
        {
            LoadDefault();
            if (per.IndexOf("S") < 0)
                return View("NotPermission");
            /*
            CategoryViewModel cvm = new CategoryViewModel();
            cvm.Code = "CATEGORY_ANSWER";
            ViewBag.PerAnswer = cvm.SelectChild().AsEnumerable().ToList();
            cvm.Code = "CATEGORY_GOOGLE";
            ViewBag.PerNew = cvm.SelectChild().AsEnumerable().ToList();
            */

            //Slect group
            AttributeViewModel at = new AttributeViewModel();
            at.Code = "ATTRIBUTE_MEMBER_GROUP";
            ViewBag.GroupName = at.SelectChild().AsEnumerable().ToList();

            //Select user
            MemberViewModel mvm = new MemberViewModel();
            if (!Request.IsNull("txtKey"))
                mvm.UserName = Request.Get("txtKey");
            if (!Request.IsNull("slGroupID"))
                mvm.GroupID = new Guid(Request.Get("slGroupID"));
            if (!Request.IsNull("slSearchStatus"))
                mvm.Status = Request.GetNumber("slSearchStatus");

            mvm.PageIndex = Request.IsNull("page") ? 1 : Request.GetNumber("page");
            mvm.PageSize = Request.IsNull("show") ? 20 : Request.GetNumber("show");


            if (!Request.IsNull("OrderBy") && !Request.IsNull("Order"))
                mvm.OrderBy = Request.Get("OrderBy") + " " + Request.Get("Order");


            //Eidt
            if (!Request.IsNull("ID"))
            {
                mvm.UserID = new Guid(Request.Get("ID"));
                ViewBag.Edit = mvm.SelectOne().AsEnumerable().ToList();
            }

            //Page
            List<DataRow> list = mvm.SelectAll().AsEnumerable().ToList();
            ViewBag.Member = list;
            ViewBag.TotalPages = list.Count > 0 ? (int)Math.Ceiling(Convert.ToDouble(list[0]["TotalRows"]) / (double)mvm.PageSize) : 0;
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create()
        {
            try
            {
                if (per.IndexOf("U") < 0)
                    return View("NotPermission");
                MemberViewModel u = new MemberViewModel();
                u.UserID = new Guid(Request.Get("ID"));
                u.Status = Request.GetNumber("slStatus");
                if (u.Status != 2)
                {
                    u.Email = Request.Get("txtEmail");
                    u.UserName = Request.Get("txtUserName");
                    u.FullName = Request.Get("txtFullName");
                    u.GoogleID = Request.Get("txtGoogleID");
                    u.Permission = Request.Get("ckPermission").Replace(',', '#');
                    if (!Request.Get("txtPassword").Equals("password") && !Request.IsNull("txtPassword"))
                        u.Password = Libs.sMD5(Request.Get("txtPassword"));
                    u.Update();
                }
                else
                    u.UpdateMemberSpam();

                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Member" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Member?error=1");
            }
        }

        [HttpPost]
        public ActionResult Update()
        {
            try
            {
                if (per.IndexOf("U") < 0)
                    return View("NotPermission");
                MemberViewModel u = new MemberViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        u.Status = Request.GetNumber("slStatus-" + i.ToString());
                        if (u.Status == 2)
                            continue;
                        u.GroupID = new Guid(Request.Get("slGroup-" + i.ToString()));
                        u.UserID = new Guid(Request.Get("ckID-" + i.ToString()));
                        u.Update();
                    }
                }
                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Member" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Member?error=1");
            }
        }

        [HttpPost]
        public ActionResult Delete()
        {
            try
            {
                if (per.IndexOf("D") < 0)
                    return View("NotPermission");
                MemberViewModel u = new MemberViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        u.UserID = new Guid(Request.Get("ckID-" + i.ToString()));
                        u.Delete();
                    }
                }
                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Member" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Member?error=1");
            }
        }

    }
}
