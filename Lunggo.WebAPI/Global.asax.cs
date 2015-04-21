using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Config;
using FlightService = Lunggo.ApCommon.Flight.Service.FlightService;

namespace Lunggo.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AppInitializer.Init();
            FlightService.GetInstance().Init("MCN004085", "GOAXML", "GA2014_xml", TargetServer.Test);
            InitDictionaryService();
        }

        private static void InitDictionaryService()
        {
            var dictionaryService = DictionaryService.GetInstance();
            var airlineFileName = ConfigManager.GetInstance().GetConfigValue("general", "airlineFileName");
            var airlineFilePath = Path.Combine(HttpContext.Current.Server.MapPath(@"~/Config/"), airlineFileName);
            var airportFileName = ConfigManager.GetInstance().GetConfigValue("general", "airportFileName");
            var airportFilePath = Path.Combine(HttpContext.Current.Server.MapPath(@"~/Config/"), airportFileName);
            dictionaryService.Init(airlineFilePath, airportFilePath);
        }
    }
}
