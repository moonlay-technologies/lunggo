using Lunggo.ApCommon.Log;
using Newtonsoft.Json.Serialization;
using System;
using System.Diagnostics;
using System.Web;
using System.Web.Http;

namespace Lunggo.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AppInitializer.Init();
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            var log = "Exception : " + ex.Message
                    + "\n StackTrace : " + ex.StackTrace;
            var TableLog = new GlobalLog();
            TableLog.PartitionKey = "GLOBAL EXCEPTION LOG";
            TableLog.Log = log;
            TableLog.Logging();
        }

    }

}
