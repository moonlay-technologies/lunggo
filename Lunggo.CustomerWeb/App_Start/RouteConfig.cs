using System.Web.Mvc;
using System.Web.Routing;

namespace Lunggo.CustomerWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Index",
                url: "{langCode}",
                defaults: new { controller = "Index", action = "Index", langCode = "id" }
            );

            routes.MapRoute(
                name: "FaqPage",
                url: "{langCode}/faqs",
                defaults: new { controller = "StaticPage", action = "Question", langCode = "id" }
            );

            routes.MapRoute(
                name: "TermPage",
                url: "{langCode}/terms",
                defaults: new { controller = "StaticPage", action = "Terms", langCode = "id" }
            );

            routes.MapRoute(
                name: "PrivacyPage",
                url: "{langCode}/privacy",
                defaults: new { controller = "StaticPage", action = "Privacy", langCode = "id" }
            );

            routes.MapRoute(
                name: "ContactPage",
                url: "{langCode}/contact",
                defaults: new { controller = "StaticPage", action = "Contact", langCode = "id" }
            );

            routes.MapRoute(
                name: "HowToOrderPage",
                url: "{langCode}/howtoorder",
                defaults: new { controller = "StaticPage", action = "HowToOrder", langCode = "id" }
            );

            routes.MapRoute(
                name: "HowToPayPage",
                url: "{langCode}/howtopay",
                defaults: new { controller = "StaticPage", action = "HowToPay", langCode = "id" }
            );

            routes.MapRoute(
                name: "toppagecampaign",
                url: "{langCode}/{destination}",
                defaults: new { controller = "Index", action = "Index", langCode = "id", destination = "" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{langCode}/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
