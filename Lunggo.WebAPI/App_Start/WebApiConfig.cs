using System.Web.Http;
using Lunggo.ApCommon.Identity.Auth;
using static Lunggo.WebAPI.App_Start.FilterConfig;
using System.Web.Http.ExceptionHandling;
using Lunggo.WebAPI.App_Start;

namespace Lunggo.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Filters.Add(new FunctionFilter());
            config.Filters.Add(new ExceptionFilter());

            // Enable Cors
            config.EnableCors();

            // Web API routes (Enable Attribute Routing)
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
