using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace iGoo.Helpers
{
    public class Libs
    {
        public static void CheckUser()
        {
            if (HttpContext.Current.Session["UserID"] == null)
                HttpContext.Current.Response.Redirect("/Webcms");
            return;
        }

        public static string GetPermission(String Code)
        {
            String per = String.Empty;
            if (HttpContext.Current.Session["Permission"] != null && Code != String.Empty)
            {
                String[] arr = HttpContext.Current.Session["Permission"].ToString().Split('$');
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i].IndexOf(Code) >= 0)
                        per += arr[i].Substring(Code.Length + 1, arr[i].Length - Code.Length - 1) + ",";
                }
            }
            return per;
        }

        public static String UrlSEO(String seoName)
        {
            try
            {
                string pattern = @"\W";
                Regex myRegex = new Regex(pattern);
                String str = myRegex.Replace(seoName.ToLower(), "-");
                str = str.Replace("_", "-");
                str = ReplaceSpace("--", "-", str);
                pattern = @"^-|-$";
                myRegex = new Regex(pattern);
                str = myRegex.Replace(str, "");
                return str;
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return seoName;
            }
        }

        public static String ReplaceSpace(String strOld, String strNew, String str)
        {
            if (str.IndexOf(strOld) > -1)
            {
                str = str.Replace(strOld, strNew);
                return ReplaceSpace(strOld, strNew, str);
            }
            return str;
        }

        public static String sKhongDau(String sCoDau)
        {
            try
            {
                const string TextToFind = "áàảãạâấầẩẫậăắằẳẵặđéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴ";
                const string TextToReplace = "aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY";

                int index = -1;
                while ((index = sCoDau.IndexOfAny(TextToFind.ToCharArray())) != -1)
                {
                    int index2 = TextToFind.IndexOf(sCoDau[index]);
                    sCoDau = sCoDau.Replace(sCoDau[index], TextToReplace[index2]);
                }

                string pattern = @"\W";
                Regex myRegex = new Regex(pattern);
                String str = myRegex.Replace(sCoDau.ToLower(), "-");
                str = str.Replace("_", "-");
                str = ReplaceSpace("--","-",str);
                pattern = @"^-|-$";
                myRegex = new Regex(pattern);
                str = myRegex.Replace(str, "");
                return str;
            }
            catch (Exception ex)
            {
                Libs.WriteError(ex.ToString());
                return sCoDau;
            }
        }
        
        public static String InputSignature(String Signature)
        {
            if (Signature.Length == 0)
                return String.Empty;
            int indexS, indexE = -1;
            if ((indexS = Signature.IndexOf("<a")) > -1 && (indexE = Signature.IndexOf("</a>")) > -1)
            {
                if (indexS < indexE)
                {
                   // Signature = Signature.Substring(indexS, indexE + 4);
                    Regex rx = new Regex(@"(?<befor>.*?)<a title=""(?<title>.*?)"" href=""(?<href>.*?)"" target=""_blank"">(?<content>.*?)</a>(?<after>.*?)", RegexOptions.Singleline);
                    Match m = rx.Match(Signature);
                    String strTitle = m.Groups["title"].Value;
                    String strHref = m.Groups["href"].Value;
                    String strContent = m.Groups["content"].Value;
                    if (strContent.Length > 50)
                        throw new Exception("Chữ kỹ bạn nhập quán dài, vui lòng nhập phần content ngắn hơn!");
                    if ( strTitle.IndexOf("\"") > -1 || strContent.IndexOf("<") > -1 || strHref.IndexOf("\"") > -1 ||
                        strHref.ToLower().IndexOf("javascript") > -1)
                        throw new Exception("Chữ kỹ bạn nhập không đúng định dạng!");
                    return "<a title=\"" + strTitle + "\" href=\"" + strHref + "\" target=\"_blank\">" + strContent + "</a>";
                }
                else
                    throw new Exception("Chữ kỹ bạn nhập không đúng định dạng!");
            }
            else if ((indexS = Signature.IndexOf("<")) > -1)
                throw new Exception("Chữ kỹ bạn nhập không được phép có dấu \"<\"");
            if (Signature.Length > 50)
                throw new Exception("Chữ kỹ bạn nhập quán dài, vui lòng nhập phần content ngắn hơn!");
            return Signature;
        }

        public static String BBCodeToHTML(String strQ)
        {
            strQ = HttpUtility.HtmlEncode(strQ);
            Regex rx = new Regex(@"\[video\](?<urlLink>.*?)\[/video\]");
            MatchCollection oMatchCollection = rx.Matches(strQ);

            foreach (Match m in oMatchCollection)
            {
                String urlLink = m.Groups["urlLink"].Value;
                if (urlLink.StartsWith("http://youtu.be"))
                    urlLink = urlLink.Replace("http://youtu.be", "http://www.youtube.com/embed");
                if (urlLink.StartsWith("http://www.youtube.com/watch?v="))
                    urlLink = urlLink.Replace("http://www.youtube.com/watch?v=", "http://www.youtube.com/embed/");
                //strQ = strQ.Replace("[video]" + m.Groups["urlLink"].Value + "[/video]", "<iframe width=\"500\" height=\"405\" src=\"" + urlLink + "\" frameborder=\"0\" allowfullscreen></iframe>");
                strQ = strQ.Replace(m.Value, "<iframe width=\"500\" height=\"405\" src=\"" + urlLink + "\" frameborder=\"0\" allowfullscreen></iframe>");
            }

            rx = new Regex(@"\[img\](.+?)\[/img\]");
            strQ = rx.Replace(strQ, "<img src=\"$1\" />");

            rx = new Regex(@"\[b\](.+?)\[/b\]");
            strQ = rx.Replace(strQ, "<strong>$1</strong>");

            rx = new Regex(@"\[code\](.+?)\[/code\]");
            strQ = rx.Replace(strQ, "<per>$1</per>");

            rx = new Regex(@"\[i\](.+?)\[/i\]");
            strQ = rx.Replace(strQ, "<em>$1</em>");

            rx = new Regex(@"\[u\](.+?)\[/u\]");
            strQ = rx.Replace(strQ, "<u>$1</u>");

            rx = new Regex(@"(\r\n|\r|\n)");
            strQ = rx.Replace(strQ, "<br />");

            return strQ;
        }

        public static string RemoveTags(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }

        public static string sMD5(string sPassword)
        {
            try
            {
                Byte[] originalBytes;
                Byte[] encodedBytes;
                MD5 md5;
                md5 = new MD5CryptoServiceProvider();
                originalBytes = ASCIIEncoding.Default.GetBytes(sPassword + sApp("salt"));
                encodedBytes = md5.ComputeHash(originalBytes);
                return BitConverter.ToString(encodedBytes);
            }
            catch (Exception ex)
            {
                throw new Exception("sMD5 error: " + ex.ToString());
            }
        }

        public static string ToPrettyDate(DateTime date)
        {
            var timeSince = DateTime.Now.Subtract(date);
            if (timeSince.TotalMilliseconds < 1) return "chưa có";
            if (timeSince.TotalMinutes < 1) return "mới đăng";
            if (timeSince.TotalMinutes < 2) return "1 phút trước";
            if (timeSince.TotalMinutes < 60) return string.Format("{0} phút trước", timeSince.Minutes);
            if (timeSince.TotalMinutes < 120) return "1 giờ trước";
            if (timeSince.TotalHours < 24) return string.Format("{0} giờ trước", timeSince.Hours);
            if (timeSince.TotalDays == 1) return "ngày hôm qua";
            if (timeSince.TotalDays < 7) return string.Format("{0} ngày trước", timeSince.Days);
            if (timeSince.TotalDays < 14) return "1 tuần trước";
            if (timeSince.TotalDays < 21) return "2 tuần trước";
            if (timeSince.TotalDays < 28) return "3 tuần trước";
            if (timeSince.TotalDays < 60) return "1 tháng trước";
            if (timeSince.TotalDays < 365) return string.Format("{0} tháng trước", Math.Round(timeSince.TotalDays / 30));
            if (timeSince.TotalDays < 730) return "1 năm trước";
            return string.Format("{0} năm trước", Math.Round(timeSince.TotalDays / 365));
        }
        public static string ToDateString(string sFormat, DateTime date)
        {
            string datetime = String.Format("{0:" + sFormat + "}", date);
            if (sFormat.IndexOf("dddd") != -1)
            {
                datetime = datetime.Replace("Monday", "Thứ hai");
                datetime = datetime.Replace("Tuesday", "Thứ ba");
                datetime = datetime.Replace("Wednesday", "Thứ tư");
                datetime = datetime.Replace("Thursday", "Thứ năm");
                datetime = datetime.Replace("Friday", "Thứ sáu");
                datetime = datetime.Replace("Saturday", "Thứ bảy");
                datetime = datetime.Replace("Sunday", "Chủ nhật");
            }
            return datetime;
        }

        public static string sApp(string sKey)
        {
            try
            {
                return ConfigurationSettings.AppSettings[sKey].Trim();
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }

        public static void WriteError(string errorMessage)
        {
            try
            {
                string path = "~/Logs/" + DateTime.Today.ToString("dd-MM-yyyy") + ".log";
                if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
                }
                using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    w.WriteLine("{0}", DateTime.Now.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    string err = "Error in: " + System.Web.HttpContext.Current.Request.Url.ToString() +
                                  ". Error Message:" + errorMessage;
                    w.WriteLine(err);
                    w.Flush();
                    w.Close();
                }
            }
            catch{}

        }

        public static void WriteGooglebot(string url)
        {
            try
            {
                string path = "~/Logs/Googlebot/" + DateTime.Today.ToString("dd-MM-yyyy") + ".log";
                if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
                }
                using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    w.WriteLine("{0}", DateTime.Now.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    w.WriteLine(url);
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);
            }

        }

        public static string GetHtml(string url)
        {
            string result = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 5000;
                request.Method = "GET";
                request.Accept = "application/xml";
                response = (HttpWebResponse)request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                result = reader.ReadToEnd();
                return result;
            }
            catch (Exception ex)
            {
                // handle error
                return ex.Message;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (response != null)
                    response.Close();
            }
        }

        public static string ThumbName(string file ,string thumb)
        {
            string ext =Path.GetExtension(file);
            if (string.IsNullOrEmpty(file))
                return "/0/NoImage-" + thumb + ".jpg";
            else
                return file.Substring(0, file.Length - ext.Length) + "-" + thumb + ext;
        }

        public static bool SendMail(string to, string Subject, string Content)
        {
            Chilkat.MailMan mailman = new Chilkat.MailMan();
            bool success;
            success = mailman.UnlockComponent("MAIL12345678_08E661AFG83H");
            if (!success)
                return success;

            Chilkat.Mht mht = new Chilkat.Mht();
            success = mht.UnlockComponent("MHT12345678_2C794D39HV5X");
            if (!success)
                return success;
            mht.UseCids = true;

            //  Use the GMail SMTP server
            mailman.SmtpHost = sApp("MAIL_SMTP");
            mailman.SmtpPort = Convert.ToInt16(sApp("MAIL_POP3_POST"));
            mailman.SmtpSsl = true;

            //  Set the SMTP login/password.
            mailman.SmtpUsername = sApp("MAIL_USERNAME");
            mailman.SmtpPassword = sApp("MAIL_PASSWORD");

            //  Create a new email object
            Chilkat.Email email = new Chilkat.Email();
            string mine = mht.HtmlToEML(Content);
            email.SetFromMimeText(mine);

            email.Subject = Subject;
            //email.Body = Content;
            email.ReplyTo = "Cộng đồng Google Việt Nam <igoovn@gmail.com>";
            email.From = "Cộng đồng Google Việt Nam <igoovn@gmail.com>";
            email.AddTo("Bạn thân", to);

            success = mailman.SendEmail(email);
            return success;
        }

        public static string RandomNumber(int n)
        {
            string randomStr = String.Empty;
            int[] myIntArray = new int[n];
            int x;
            //That is to create the random # and add it to our string 
            Random autoRand = new Random();
            for (x = 0; x < n; x++)
            {
                myIntArray[x] = System.Convert.ToInt32(autoRand.Next(0, 9));
                randomStr += (myIntArray[x].ToString());
            }
            return randomStr;
        }

        public static string sCurrency(Decimal number)
        {
            try
            {
                String s = String.Format("{0:0,0}", number);
                s = s.Replace(".00", "");
                return s;
            }
            catch (Exception ex)
            {
                throw new Exception("sCurrency error: " + ex.ToString());
            }
        }
        public static string sNumber(Int64 number)
        {
            try
            {
                return String.Format("{0:0,0}", number);
            }
            catch (Exception ex)
            {
                throw new Exception("sNumber error: " + ex.ToString());
            }
        }

        public static void WriteCookie(string name, string value)
        {
            var cookie = new HttpCookie(name, value);
            HttpContext.Current.Response.Cookies.Set(cookie);
        }

        public static string ReadCookie(string name)
        {
            if (HttpContext.Current.Response.Cookies.AllKeys.Contains(name))
            {
                var cookie = HttpContext.Current.Response.Cookies[name];
                return cookie.Value;
            }

            if (HttpContext.Current.Request.Cookies.AllKeys.Contains(name))
            {
                var cookie = HttpContext.Current.Request.Cookies[name];
                return cookie.Value;
            }

            return null;
        }
    }
}