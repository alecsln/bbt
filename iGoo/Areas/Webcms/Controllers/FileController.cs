using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using iGoo.Helpers;
using iGoo.Classes;
using iGoo.Areas.Webcms.Models;

namespace iGoo.Areas.Webcms.Controllers
{
    public class FileController : DefaultController
    {
        public ActionResult Index()
        {
            CheckUser();
            //Select default
            //Select user
            ViewBag.LoginUserID = Session["UserID"];
            ViewBag.LoginFullName = Session["FullName"];

            //Slect group
            AttributeViewModel at = new AttributeViewModel();
            at.Code = "ATTRIBUTE_UPLOAD";
            ViewBag.GroupName = at.SelectChild().AsEnumerable().ToList();

            FileViewModel fv = new FileViewModel();
            if (!Request.IsNull("txtKey"))
                fv.Name = Request.Get("txtKey");
            if (!Request.IsNull("slSearchGroup"))
                fv.AttributeID = new Guid(Request.Get("slSearchGroup"));
            
            fv.PageIndex = Request.IsNull("page") ? 1 : Request.GetNumber("page");
            fv.PageSize = Request.IsNull("show") ? 20 : Request.GetNumber("show");

            List<DataRow> list = fv.SelectAll().AsEnumerable().ToList();
            ViewBag.File = list;
            ViewBag.TotalPages = list.Count > 0 ? (int)Math.Ceiling(Convert.ToDouble(list[0]["TotalRows"]) / (double)fv.PageSize) : 0;

            return View();
        }

        [HttpPost]
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> files)
        {
            try
            {
                foreach (var file in files)
                {
                    if (file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        FileViewModel fv = new FileViewModel();
                        fv.AttributeID = new Guid(Request.Get("slSearchGroup"));
                        fv.Name = fileName;
                        var ext = Path.GetExtension(file.FileName).ToLower();
                        fileName = Libs.sKhongDau(fileName);
                        string dd = DateTime.Today.ToString("ddMMyyyy").Replace("-", string.Empty);

                        String edit = Request.Get("txtEdit");
                        string dir;
                        if (String.IsNullOrEmpty(edit))
                        {
                            dir = Server.MapPath("~/Uploads/" + dd);
                            if (!Directory.Exists(dir))
                                Directory.CreateDirectory(dir);
                            int count = checkExistFile(dir, fileName, ext, 0);
                            var path = Path.Combine(dir, fileName + ext);
                            if (count > 0)
                            {
                                path = Path.Combine(dir, fileName + "-" + count + ext);
                                fv.Src = dd + "/" + fileName + "-" + count + ext;
                            }
                            else
                                fv.Src = dd + "/" + fileName + ext;

                            file.SaveAs(path);
                            fv.Size = file.ContentLength;
                            fv.Type = ext;
                            fv.Created = DateTime.Now;
                            fv.FileID = Guid.NewGuid();
                            fv.Insert();
                        }
                        else
                        {
                            dir = Server.MapPath("~/Uploads/" + edit);
                            file.SaveAs(dir);

                        }
                       
                    }
                }
                return Redirect("/Webcms/File");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/File?error=1");
            }
        }

        private int checkExistFile(String path, String fileName, String ext, int count)
        {
            if (System.IO.File.Exists(path + "/" + fileName + ext))
               return checkExistFile(path, fileName + "-" + count, ext, ++count);
            return count;
        }

        [HttpPost]
        public ActionResult Delete()
        {
            try
            {
                FileViewModel fv = new FileViewModel();
                for (int i = 1; i <= Request.GetNumber("count"); i++)
                {
                    if (!Request.IsNull("ckID-" + i.ToString()))
                    {
                        fv.FileID = new Guid(Request.Get("ckID-" + i.ToString()));
                        fv.Delete();
                        string src = Server.MapPath("~/Uploads/" + Request.Get("hdSrc-" + i.ToString()));
                        if (System.IO.File.Exists(src))
                            System.IO.File.Delete(src);
                    }
                }
                return Redirect("/Webcms/File?result=1");
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return Redirect("/Webcms/File?error=1");
            }
        }
    }
}
