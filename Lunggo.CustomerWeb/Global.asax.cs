using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Database;

namespace Lunggo.CustomerWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            
           
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AppInitializer.Init();
            FlightService.GetInstance().Init("MCN004085", "GOAXML", "GA2014_xml", TargetServer.Test);
            InitDB();
         }

        private void InitDB()
        {
            const string connString = @"Data Source=""playdb.cloudapp.net, 63778"";Initial Catalog=Travorama;Persist Security Info=True;User ID=developer;Password=Standar1234";
            DbService.GetInstance().Init(connString);
        }

    }
}
