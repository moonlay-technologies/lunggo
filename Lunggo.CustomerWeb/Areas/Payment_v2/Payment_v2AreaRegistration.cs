using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Areas.Payment_v2
{
    public class Payment_v2AreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Payment_v2";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Payment_v2_default",
                "Payment_v2/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}