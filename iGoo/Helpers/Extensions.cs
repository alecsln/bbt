using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.WebPages.Html;
using Microsoft.Security.Application;
using System.Web.Mvc;

namespace iGoo.Helpers
{
    public static class Extensions
    {
        public static string Truncate(this string s, int maxLength)
        {
            if (string.IsNullOrEmpty(s) || maxLength <= 0)
                return string.Empty;
            else if (s.Length > maxLength)
                return s.Substring(0, maxLength) + "...";
            else
                return s;
        }

        // IsNumeric Function
        public static bool IsNumeric(this string Expression)
        {
            bool isNum;

            double retNum;

            isNum = Double.TryParse(Expression, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        public static string Get(this HttpRequestBase s, string p)
        {
            try
            {
                //return HttpUtility.HtmlDecode(AntiXss.HtmlEncode(HttpContext.Current.Request[p].Trim()));
                return HttpContext.Current.Request[p].Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static int GetNumber(this HttpRequestBase s, string p)
        {
            try
            {
                return Convert.ToInt32(HttpContext.Current.Request[p].Trim());
            }
            catch
            {
                return 0;
            }
        }

        public static decimal GetDecimal(this HttpRequestBase s, string p)
        {
            try
            {
                return Convert.ToDecimal(HttpContext.Current.Request[p].Trim());
            }
            catch
            {
                return 0;
            }
        }

        public static bool IsNull(this HttpRequestBase s, string p)
        {
            try
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request[p]);
            }
            catch
            {
                return true;
            }
        }

        public static string Query(this HttpRequestBase s, string p)
        {
            try
            {
                string v = string.Empty;
                var q = HttpContext.Current.Request.QueryString;
                foreach (string url in q)
                {
                    if (!url.Equals("result") && !url.Equals("error") && !url.Equals("ID") && !url.Equals("Action") && p.IndexOf(url) < 0)
                        v += "&" + url + "=" + q[url];
                }
                if(!p.Equals(string.Empty))
                    v +="&" + p;
                v = "?"+v.Substring(1, v.Length - 1);
                if (!v.Equals("?"))
                    return v;
                else
                    return String.Empty;
            }
            catch
            {
                return "?" + p;
            }
        }

        static Regex LineEnding = new Regex(@"(\r\n|\r|\n)+");
        public static MvcHtmlString Nl2br(this System.Web.Mvc.HtmlHelper html, string text)
        {
            return MvcHtmlString.Create(
                    LineEnding.Replace(HttpUtility.HtmlEncode(text), "<br />")
                );
        }
    }
}