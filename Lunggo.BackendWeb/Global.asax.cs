using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds;

namespace Lunggo.BackendWeb
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AppInitializer.Init();
            var hb = new HotelBedsCheckRate();
            hb.CheckRateHotel(new HotelRate
            {
                Price = 2564794,
                RateKey = "20161108|20161110|W|1|1067|DBL.VM|ID_B2B_24|RO|O45I|1~2~1|8|N@E1B537EDBCFE49DF802AB180E0D87D38"
            });
        }
    }
}
