using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Areas.UW400
{
    public class UW400AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "UW400";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "UW400_default",
                "UW400/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}