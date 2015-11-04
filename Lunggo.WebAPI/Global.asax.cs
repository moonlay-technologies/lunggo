using System.Web.Http;

namespace Lunggo.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AppInitializer.Init();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
