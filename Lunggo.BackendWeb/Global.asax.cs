using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Currency.Service;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Sequence;
using Lunggo.ApCommon.Travolutionary.WebService.Hotel;
using Lunggo.Framework.Encoder;

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
            FlightService.GetInstance().CommenceSearchFlight("BDOTKG140516-321y".Base64Encode(), 5);
            //FlightService.GetInstance().RevalidateFareInternal(new RevalidateConditions{FareId = "LIONPUBkjbcxz"});
            //FlightService.GetInstance().OrderTicketInternal("LIONPUBMAZUYR", true);
            //FlightService.GetInstance().is
            
        }
    }
}
