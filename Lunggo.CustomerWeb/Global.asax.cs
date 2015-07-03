using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.WebPages;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.BrowserDetection;
using Lunggo.Framework.Constant;
using Lunggo.Framework.Database;
using Lunggo.Framework.Http;
using WURFL;
using HttpRequest = System.Web.HttpRequest;

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

            DisplayModeProvider.Instance.Modes.RemoveAt(0);
            DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("mobile")
            {
                ContextCondition = context => 
                                context.Request.Url.Host == "dv1.travorama.com"
            });


        }

        void Session_Start(object sender, EventArgs e)
        {
            // Redirect mobile users to the mobile home page
            HttpRequest httpRequest = HttpContext.Current.Request;
            if (httpRequest.Browser.IsMobileDevice)
            {
                string host = httpRequest.Url.Host;
                string path = httpRequest.Url.PathAndQuery;
                var userAgent = httpRequest.UserAgent;
                var browserDetectionService = BrowserDetectionService.GetInstance();
                var device = browserDetectionService.GetDevice(userAgent);
                var isSmartphone = bool.Parse(device.GetCapability("is_smartphone"));
                bool isOnMobilePage = host == "dv1.travorama.com" && isSmartphone;
                if (!isOnMobilePage)
                {
                    string redirectTo = "http://dv1.travorama.com" + path;

                    // Could also add special logic to redirect from certain 
                    // recognized pages to the mobile equivalents of those 
                    // pages (where they exist). For example,
                    // if (HttpContext.Current.Handler is UserRegistration)
                    //     redirectTo = "~/Mobile/Register.aspx";

                    HttpContext.Current.Response.Redirect(redirectTo);
                }
            }
        }
    }
}
