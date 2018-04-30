using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.BrowserDetection;
using Lunggo.Framework.Config;
using Lunggo.Framework.Encoder;
using Lunggo.ApCommon.Log;

namespace Lunggo.CustomerWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            AppInitializer.Init();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
            // Redirect mobile users to the mobile home page

            var httpRequest = Request;
            var mobileUrl = EnvVariables.Get("general", "mobileUrl");
            var host = httpRequest.Url.Host;
            var path = httpRequest.Url.PathAndQuery;
            var browserDetectionService = BrowserDetectionService.GetInstance();
            var isSmartphone = browserDetectionService.IsRequestFromAndroidOrIphone(httpRequest);
            var isOnMobilePage = host == mobileUrl || host == "192.168.0.139";
            if (!isOnMobilePage && isSmartphone)
            {
                string redirectTo = "http://" + mobileUrl + path;

                // Could also add special logic to redirect from certain 
                // recognized pages to the mobile equivalents of those 
                // pages (where they exist). For example,
                // if (HttpContext.Current.Handler is UserRegistration)
                //     redirectTo = "~/Mobile/Register.aspx";

                Response.Redirect(redirectTo);
            }

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (exception != null)
            {
                var log = "Exception : " + exception.Message
                    + "\nStackTrace : \n" + exception.StackTrace;
                var TableLog = new GlobalLog();
                TableLog.PartitionKey = "GLOBAL UNHANDLED EXCEPTION LOG";
                TableLog.Log = log;
                TableLog.Logging();
            }
        }

    }
}
