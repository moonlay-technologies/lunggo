using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Areas.UW100
{
    public class UW100AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "UW100";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //context.MapRoute(
            //    "UW100_default",
            //    "UW100/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}