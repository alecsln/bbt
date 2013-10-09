using System.Web.Mvc;

namespace iGoo.Areas.Webcms
{
    public class WebcmsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Webcms";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Webcms_default",
                "Webcms/{controller}/{action}/{id}",
                new { controller = "Login", action = "Index", id = UrlParameter.Optional},
                new string[] { "iGoo.Areas.Webcms.Controllers" }
            );
        }
    }
}
