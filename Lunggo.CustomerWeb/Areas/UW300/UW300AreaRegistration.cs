using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Areas.UW300
{
    public class UW300AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "UW300";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //context.MapRoute(
            //    "UW300_default",
            //    "UW300/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}