using System;
using System.Diagnostics;
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
            var request = new SearchHotelCondition();
            request.CheckIn = DateTime.Now.AddDays(10);
            request.Checkout = DateTime.Now.AddDays(13);
            request.Rooms = 2;
            request.AdultCount = 2;
            request.Location = "JAV";
            request.Zone = 1;
            HotelBedsSearchHotel hotel = new HotelBedsSearchHotel();
            var response = hotel.SearchHotel(request);
            Debug.Print("Response : " + response);
        }
    }
}
