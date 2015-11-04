using System.Web.Http;

namespace Lunggo.BackendWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{prefix}",
                defaults: new { prefix = RouteParameter.Optional }
            );
        }
    }
}
