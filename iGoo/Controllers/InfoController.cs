using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iGoo.Helpers;
using iGoo.Models;
using System.Data;
using iGoo.Classes;
using System.Text.RegularExpressions;
using System.Web.Security;

using DotNetOpenAuth.OpenId.RelyingParty;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;

namespace iGoo.Controllers
{
    public class InfoController : DefaultController
    {
        public ActionResult Index()
        {
            CheckMember();
            ///Select default
            LoadDefault();

            CategoryViewModel cvm = new CategoryViewModel();
            cvm.Type = "LEFT";
            ViewBag.MenuLeft = cvm.SelectByType().AsEnumerable().ToList();

            //Select user
            UserViewModel uvm = new UserViewModel();
            uvm.UserID = new Guid(Session["MemberID"].ToString());
            ViewBag.Info = uvm.SelectOne().AsEnumerable().ToList();
            ViewBag.NotificationCount = uvm.SelectNotificationCountByUserID().Rows[0][0];
            //End select default

            return View();
        }

        public ActionResult Notification(int page)
        {
            CheckMember();
            ///Select default
            LoadDefault();

            QuestionViewModel qvm = new QuestionViewModel();
            qvm.Top = 5;
            ViewBag.QuestionNew = qvm.SelectByCreated().AsEnumerable().ToList();

            CategoryViewModel cvm = new CategoryViewModel();
            cvm.Type = "ANSWER";
            ViewBag.MenuAnswer = cvm.SelectByType().AsEnumerable().ToList();

            //Select user
            UserViewModel uvm = new UserViewModel();
            uvm.UserID = new Guid(Session["MemberID"].ToString());
            ViewBag.Info = uvm.SelectOne().AsEnumerable().ToList();
            ViewBag.NotificationCount = 0;
            uvm.PageIndex = page;
            uvm.PageSize = 20;
            List<DataRow> list = uvm.SelectNotificationByUserID().AsEnumerable().ToList();
            ViewBag.Notification = list;
            ViewBag.TotalPages = list.Count > 0 ? (int)Math.Ceiling(Convert.ToDouble(list[0]["TotalRows"]) / (double)uvm.PageSize) : 0;
            ViewBag.CategoryPage = page;
            //End select default

            return View();
        }

        public ActionResult Messenger(int page)
        {
            CheckMember();
            ///Select default
            LoadDefault();

            QuestionViewModel qvm = new QuestionViewModel();
            qvm.Top = 5;
            ViewBag.QuestionNew = qvm.SelectByCreated().AsEnumerable().ToList();

            CategoryViewModel cvm = new CategoryViewModel();
            cvm.Type = "ANSWER";
            ViewBag.MenuAnswer = cvm.SelectByType().AsEnumerable().ToList();

            //Select user
            UserViewModel uvm = new UserViewModel();
            uvm.UserID = new Guid(Session["MemberID"].ToString());
            ViewBag.Info = uvm.SelectOne().AsEnumerable().ToList();
            ViewBag.NotificationCount = 0;
            MessengerViewModel mvm = new MessengerViewModel();
            mvm.UserID = uvm.UserID;
            mvm.PageIndex = page;
            mvm.PageSize = 20;
            List<DataRow> list = mvm.SelectMessengerByUserID().AsEnumerable().ToList();
            ViewBag.Messenger = list;
            ViewBag.TotalPages = list.Count > 0 ? (int)Math.Ceiling(Convert.ToDouble(list[0]["TotalRows"]) / (double)mvm.PageSize) : 0;
            ViewBag.CategoryPage = page;
            //End select default

            return View();
        }

        public ActionResult ViewMessenger(int page)
        {
            CheckMember();
            ///Select default
            LoadDefault();

            QuestionViewModel qvm = new QuestionViewModel();
            qvm.Top = 5;
            ViewBag.QuestionNew = qvm.SelectByCreated().AsEnumerable().ToList();

            CategoryViewModel cvm = new CategoryViewModel();
            cvm.Type = "ANSWER";
            ViewBag.MenuAnswer = cvm.SelectByType().AsEnumerable().ToList();

            //Select user
            UserViewModel uvm = new UserViewModel();
            uvm.UserID = new Guid(Session["MemberID"].ToString());
            ViewBag.Info = uvm.SelectOne().AsEnumerable().ToList();
            ViewBag.NotificationCount = 0;
            MessengerViewModel mvm = new MessengerViewModel();
            mvm.UserID = uvm.UserID;
            if (!Request.IsNull("id"))
                mvm.MessengerID = new Guid(Request.Get("id"));
            ViewBag.ViewMessenger = mvm.SelectMessengerByUserAndMessengerID().AsEnumerable().ToList();
            ViewBag.Receipt = mvm.SelectMessengerUsersByReceipt().AsEnumerable().ToList();

            return View();
        }

        public ActionResult SendMessenger(int page)
        {
            CheckMember();
            ///Select default
            LoadDefault();

            QuestionViewModel qvm = new QuestionViewModel();
            qvm.Top = 5;
            ViewBag.QuestionNew = qvm.SelectByCreated().AsEnumerable().ToList();

            CategoryViewModel cvm = new CategoryViewModel();
            cvm.Type = "ANSWER";
            ViewBag.MenuAnswer = cvm.SelectByType().AsEnumerable().ToList();

            //Select user
            UserViewModel uvm = new UserViewModel();
            uvm.UserID = new Guid(Session["MemberID"].ToString());
            ViewBag.Info = uvm.SelectOne().AsEnumerable().ToList();
            ViewBag.NotificationCount = 0;
            if (!Request.IsNull("id"))
            {
                MessengerViewModel mvm = new MessengerViewModel();
                mvm.MessengerID = new Guid(Request.Get("id"));
                ViewBag.SendMessenger = mvm.SelectMessengerByMessengerID().AsEnumerable().ToList();
            }

            return View();
        }

        public ActionResult Login()
        {
            if (Session["MemberID"] != null)
                return RedirectToAction(string.Empty);

            LoadDefault();

            AttributeViewModel avm = new AttributeViewModel();
            avm.Code = "STATIC_TERM";
            ViewBag.StaticTerm = avm.SelectByCode().AsEnumerable().ToList();

            CategoryViewModel cvm = new CategoryViewModel();
            cvm.Type = "LEFT";
            ViewBag.MenuLeft = cvm.SelectByType().AsEnumerable().ToList();
            return View();
        }

        public ActionResult Forgot()
        {
            LoadDefault();

            CategoryViewModel cvm = new CategoryViewModel();
            cvm.Type = "LEFT";
            ViewBag.MenuLeft = cvm.SelectByType().AsEnumerable().ToList();
            return View();
        }

        public ActionResult UserName(string title, int page)
        {
            LoadDefault();

            //Select user
            UserViewModel uvm = new UserViewModel();
            uvm.UserName = title;
            //Select Info
            ViewBag.Info = uvm.SelectByUserName().AsEnumerable().ToList();
            //Select History
            ViewBag.HistoryCount = 0;
            uvm.PageIndex = page;
            uvm.PageSize = 10;
            List<DataRow> list = uvm.SelectHistoryByUserName().AsEnumerable().ToList();
            ViewBag.History = list;
            ViewBag.TotalPages = list.Count > 0 ? (int)Math.Ceiling(Convert.ToDouble(list[0]["TotalRows"]) / (double)uvm.PageSize) : 0;
            ViewBag.CategoryPage = page;
            //End select default

            return View();
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChangeConfig()
        {
            try
            {
                CheckMember();
                if (Request.IsNull("txtUserName") || Request.Get("txtUserName").Trim() == String.Empty)
                    return Redirect("?UpdateError=1");
                UserViewModel u = new UserViewModel();
                u.UserID = new Guid(Session["MemberID"].ToString());
                u.UserName = Request.Get("txtUserName");
                u.FullName = Request.Get("txtFullName");
                u.Brithday = Convert.ToDateTime(Request.Get("txtBrithday"));
                u.Phone = Request.Get("txtPhone");
                u.Address = Request.Get("txtAddress");
                u.Signature = Libs.InputSignature(Request.Get("txtSignature"));
                u.GoogleID = Request.Get("txtGoogleID");
                if (!"".Equals(u.GoogleID))
                {
                    String pt = @"width=""200"" height=""200"" src=""(?<Url>.*?)"" />";
                    Regex rg = new Regex(pt, RegexOptions.Singleline);
                    Match m = rg.Match(Libs.GetHtml("https://plus.google.com/" + u.GoogleID.ToString() + "/posts"));
                    u.Image = m.Groups["Url"].Value;
                }
                u.Update();
                return Redirect("?UpdateResult=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("?UpdateError=1");
            }
        }

        [HttpPost]
        public ActionResult ChangePass()
        {
            try
            {
                CheckMember();
                UserViewModel u = new UserViewModel();
                u.UserID = new Guid(Session["MemberID"].ToString());
                u.Password = Libs.sMD5(Request.Get("txtPassword"));
                u.PasswordNew = Libs.sMD5(Request.Get("txtPasswordNew"));
                u.ChangePass();
                return Redirect("?changePassResult=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("?changePassError=1");
            }
        }

        [HttpPost]
        public ActionResult CheckLogin()
        {
            UserViewModel u = new UserViewModel();
            u.Email = Request.Get("txtEmail").Trim();
            u.Password = Libs.sMD5(Request.Get("txtPassword"));

            DataTable dt = u.CheckLogin();
            if (dt.Rows.Count > 0)
            {
                Session["MemberID"] = dt.Rows[0]["UserID"].ToString();
                Session["FullName"] = dt.Rows[0]["FullName"].ToString();
                Session["Permission"] = dt.Rows[0]["Permission"].ToString();
                return Redirect("/");
            }
            else
                return Redirect("/info/login?loginError=1");
        }
        
        public string CheckEmail()
        {
            UserViewModel u = new UserViewModel();
            u.Email = Request.Get("txtEmail").Trim();
            if (u.SelectByUserName().Rows.Count > 0)
                return "false";
            else
                return "true";
        }

        public string CheckUserName()
        {
            UserViewModel u = new UserViewModel();
            u.UserName = Regex.Replace(Request.Get("txtUserName").ToLower(), "\\W", String.Empty);
            if (u.SelectByUserName().Rows.Count > 0)
                return "false";
            else
                return "true";
        }

        [HttpPost]
        public ActionResult ForgotAction()
        {
            try
            {
                UserViewModel u;
                DataTable dt = new DataTable();
                u = new UserViewModel();
                u.Email = Request.Get("txtEmail").Trim();
                dt = u.SelectByUserName();
                if (dt.Rows.Count > 0)
                {
                    u = new UserViewModel();
                    String pass = Libs.RandomNumber(6); ;
                    u.Password = Libs.sMD5(pass);

                    String mgr = "<p><strong>Mật khẩu mới của bạn là: " + pass + "</strong></p>";

                    //Send mail
                    if (Libs.SendMail(Request.Get("txtEmail").Trim(), "Mật khẩu mới từ iGoo.vn", mgr))
                    {
                        u.UserID = new Guid(dt.Rows[0]["UserID"].ToString());
                        u.Update();
                        return Redirect("forgot?forgotResult=1");
                    }
                    else
                        return Redirect("forgot?forgotError=2");
                }
                else
                    return Redirect("forgot?forgotError=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("forgot?forgotError=1");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register()
        {
            try
            {
                UserViewModel u = new UserViewModel();
                u.UserID = Guid.NewGuid();
                if (Request.IsNull("txtUserName") || Request.IsNull("txtPassword"))
                    return Redirect("?registerError=1");
                u.UserName = Regex.Replace(Request.Get("txtUserName").ToLower(), "\\W", String.Empty);
                u.Password = Libs.sMD5(Request.Get("txtPassword"));
                u.Email = Request.Get("txtEmail");
                u.FullName = Request.Get("txtEmail");
                u.Status = 1;
                u.TotalAnswer = 0;
                u.Views = 0;
                u.Created = DateTime.Now;
                u.Insert();
                //Session["GoogleUserName"] = null;
                //Session["GoogleFirstName"] = null;
                //Session["GoogleLastName"] = null;
                //Session["GoogleEmail"] = null;
                return Redirect("/info/login?registerResult=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/info/login?registerError=1");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMail()
        {
            try
            {
                MessengerViewModel mvm = new MessengerViewModel();
                mvm.TypeMessengerUser = Request.GetNumber("slTypeSend");
                if (mvm.TypeMessengerUser == 1)// thanh vien
                {
                    if (Request.IsNull("txtSendTo"))
                        return Redirect("SendMessenger?Error=0");
                    mvm.SendTo = Request.Get("txtSendTo");
                }

                if (Request.IsNull("txtContent"))
                    return Redirect("SendMessenger?Error=1");

                mvm.UserID = new Guid(Session["MemberID"].ToString());
                
                mvm.Type = 1;
                mvm.MessengerID = Guid.NewGuid();
                mvm.Title = Request.Get("txtTitle");
                mvm.Content = Request.Get("txtContent");
                mvm.Created = DateTime.Now;
                mvm.Insert();
                return Redirect("/info/Messenger?sendMailResult=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/info/Messenger?sendMailError=1");
            }
        }

        [HttpPost]
        public String DeleteMessenger()
        {
            try
            {
                if (!Request.IsNull("id"))
                {
                    MessengerViewModel mvm = new MessengerViewModel();
                    mvm.MessengerID = new Guid(Request.Get("id"));
                    mvm.UserID = new Guid(Session["MemberID"].ToString());
                    mvm.Delete();
                }

                return "1";
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return "0";
            }
        }

        private static OpenIdRelyingParty Openid = new OpenIdRelyingParty();
        [ValidateInput(false)]
        public ActionResult Authenticate(string OpenidUrl)
        {
            var response = Openid.GetResponse();
            if (response == null)
            {
                // User submitting Identifier
                Identifier id;
                if (Identifier.TryParse(OpenidUrl, out id))
                {
                    try
                    {
                        var request = Openid.CreateRequest(OpenidUrl);
                        var fetch = new FetchRequest();
                        fetch.Attributes.AddRequired(WellKnownAttributes.Contact.Email);
                        fetch.Attributes.AddRequired(WellKnownAttributes.Name.First);
                        fetch.Attributes.AddRequired(WellKnownAttributes.Name.Last);
                        request.AddExtension(fetch);
                        return request.RedirectingResponse.AsActionResult();
                    }
                    catch (ProtocolException ex)
                    {
                        return RedirectToAction("Index");
                    }
                }
                return RedirectToAction("Index");
            }

            // Openid Provider sending assertion response
            switch (response.Status)
            {
                case AuthenticationStatus.Authenticated:
                    var fetch = response.GetExtension<FetchResponse>();
                    string firstName = "unknown";
                    string lastName = "unknown";
                    string email = "unknown";
                    if (fetch != null)
                    {
                        firstName = fetch.GetAttributeValue(WellKnownAttributes.Name.First);
                        lastName = fetch.GetAttributeValue(WellKnownAttributes.Name.Last);
                        email = fetch.GetAttributeValue(WellKnownAttributes.Contact.Email);
                    }
                    return CreateUser(response.ClaimedIdentifier, firstName, lastName, email);
                case AuthenticationStatus.Canceled:
                    return RedirectToAction("Index");
                case AuthenticationStatus.Failed:
                    return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        private ActionResult CreateUser(string userName, string firstName, string lastName, string email)
        {
            //Session["GoogleUserName"] = userName;
            Session["GoogleFirstName"] = firstName;
            Session["GoogleLastName"] = lastName;
            Session["GoogleEmail"] = email;
            UserViewModel u = new UserViewModel();
            u.Email = Session["GoogleEmail"].ToString();
            DataTable dt = u.SelectByUserName();
            if(dt.Rows.Count>0)
            {
                Session["MemberID"] = dt.Rows[0]["UserID"].ToString();
                Session["FullName"] = dt.Rows[0]["FullName"].ToString();
                Session["Permission"] = dt.Rows[0]["Permission"].ToString();
            }
            return RedirectToAction("login", "Info");
        }

        public ActionResult Logout()
        {
            Session["MemberID"] = null;
            Session["FullName"] = null;
            Session["Permission"] = null;

            Session["GoogleFirstName"] = null;
            Session["GoogleLastName"] = null;
            Session["GoogleEmail"] = null;
            return Redirect("/");
        }
    }
}
