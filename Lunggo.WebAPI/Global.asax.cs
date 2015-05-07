using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Redis;
using FlightService = Lunggo.ApCommon.Flight.Service.FlightService;

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
