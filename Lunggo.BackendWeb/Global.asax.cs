using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Lunggo.ApCommon.Autocomplete;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.SnowMaker;
using Microsoft.WindowsAzure.Storage;

namespace Lunggo.BackendWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            InitDb();
            FlightService.GetInstance().Init("MCN004085", "GOAXML", "GA2014_xml", TargetServer.Test);
        }
        private static void InitDb()
        {
            const string connString = @"Data Source=""playdb.cloudapp.net, 63778"";Initial Catalog=Travorama;Persist Security Info=True;User ID=developer;Password=Standar1234";
            DbService.GetInstance().Init(connString);
        }
    }
}
