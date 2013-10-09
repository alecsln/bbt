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
    public class MessengerController : DefaultController
    {
        private String per = Libs.GetPermission("MESSENGER");

        public ActionResult Index()
        {
            LoadDefault();
            //End select default

            if (per.IndexOf("S") < 0)
                return View("NotPermission");

            AttributeViewModel at = new AttributeViewModel();
            at.Code = "ATTRIBUTE_MEMBER_GROUP";
            ViewBag.GroupMember = at.SelectChild().AsEnumerable().ToList();
            at.Code = "ATTRIBUTE_ADMIN_GROUP";
            ViewBag.GroupAdmin = at.SelectChild().AsEnumerable().ToList();

            //Select Category
            MessengerViewModel vm = new MessengerViewModel();
            if (!Request.IsNull("txtKey"))
                vm.Title = Request.Get("txtKey");
            if (!Request.IsNull("slSearchType"))
                vm.Type = Request.GetNumber("slSearchType");

            vm.Status = Request.GetNumber("slSearchStatus");
            vm.UserID = new Guid(Session["UserID"].ToString());

            vm.PageIndex = Request.IsNull("page") ? 1 : Request.GetNumber("page");
            vm.PageSize = Request.IsNull("show") ? 20 : Request.GetNumber("show");
            
            //Eidt
            if (!Request.IsNull("ID"))
            {
                vm.MessengerID = new Guid(Request.Get("ID"));
                ViewBag.Edit = vm.SelectOne().AsEnumerable().ToList();
            }

            //Page
            List<DataRow> list = vm.SelectAll().AsEnumerable().ToList();
            ViewBag.Messenger = list;
            ViewBag.TotalPages = list.Count > 0 ? (int)Math.Ceiling(Convert.ToDouble(list[0]["TotalRows"]) / (double)vm.PageSize) : 0;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create()
        {
            try
            {
                MessengerViewModel mv = new MessengerViewModel();
                mv.Title = Request.Get("txtTitle");
                mv.Type = 0;
                mv.Content = Request.Get("txtContent");
                if (Request.IsNull("ID"))
                {
                    if (per.IndexOf("I") < 0)
                        return View("NotPermission");
                    mv.TypeMessengerUser = Request.GetNumber("slTypeSend");
                    if (mv.TypeMessengerUser == 1)
                    {
                        if (!Request.IsNull("slSendGroupMember"))
                            mv.SendGroupID = new Guid(Request.Get("slSendGroupMember"));
                    }
                    else
                    {
                        if (!Request.IsNull("slSendGroupAdmin"))
                            mv.SendGroupID = new Guid(Request.Get("slSendGroupAdmin"));
                    }
                    mv.SendTo = Request.Get("txtTo");
                    mv.Status = 0;
                    mv.UserID = new Guid(Session["UserID"].ToString());
                    mv.MessengerID = Guid.NewGuid();
                    mv.Created = DateTime.Now;
                    mv.Insert();
                }
                else
                {
                    if (per.IndexOf("U") < 0)
                        return View("NotPermission");
                    mv.MessengerID = new Guid(Request.Get("ID"));
                    mv.Update();
                }

                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Messenger" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Messenger?error=1");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete()
        {
            try
            {
                if (per.IndexOf("D") < 0)
                    return View("NotPermission");
                MessengerViewModel mv = new MessengerViewModel();
                mv.UserID = new Guid(Session["UserID"].ToString());
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        mv.MessengerID = new Guid(Request.Get("ckID-" + i.ToString()));
                        mv.Delete();
                    }
                }
                string returnUrl = Request.Get("returnUrl");
                return Redirect("/Webcms/Messenger" + returnUrl + "&result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/Messenger?error=1");
            }
        }
    }
}
