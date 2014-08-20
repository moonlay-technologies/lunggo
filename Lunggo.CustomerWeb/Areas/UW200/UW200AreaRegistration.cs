using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Areas.UW200
{
    public class UW200AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "UW200";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //context.MapRoute(
            //    "UW200_default",
            //    "UW200/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}