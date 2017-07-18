using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Lunggo.CustomerWeb.Attributes;
using Lunggo.Framework.Config;
using Lunggo.Framework.Filter;

namespace Lunggo.CustomerWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(CreateGlobalErrorHandler());
            filters.Add(new LanguageFilterAttribute());
            //filters.Add(new DeviceDetectionFilterAttribute());
            AddBasicAuthenticationFilterAttribute(filters);
            GlobalFilters.Filters.Add(new RequireHttpsProductionAttribute());
            GlobalFilters.Filters.Add(new PlatformFilter());
        }

        public class PlatformFilter : ActionFilterAttribute
        {
            public override void OnResultExecuting(ResultExecutingContext filterContext)
            {
                var data = filterContext.HttpContext.Request.Headers["X-Platform"];
                filterContext.Controller.ViewBag.Platform = data;
                filterContext.Controller.ViewBag.ClientId = data;
                filterContext.Controller.ViewBag.ClientSecret = data;
            }
        }

        private static HandleErrorAttribute CreateGlobalErrorHandler()
        {
            return new HandleErrorAttribute
            {
                Order = 1,
                View = "GlobalError"
            };
        }

        private static void AddBasicAuthenticationFilterAttribute(GlobalFilterCollection filters)
        {
            var basicAuthEnvList = new List<String>
            {
                
            };
            var environment = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            if (basicAuthEnvList.Any(p => p == environment))
            {
                var userName = ConfigManager.GetInstance().GetConfigValue("cw", "basicAuthenticationUser");
                var password = ConfigManager.GetInstance().GetConfigValue("cw", "basicAuthenticationPassword");
                var realm = ConfigManager.GetInstance().GetConfigValue("cw", "basicAuthenticationRealm");
                var filterAttribute = new BasicAuthenticationFilterAttribute(userName,password)
                {
                    BasicRealm = realm
                };
                filters.Add(filterAttribute);    
            }
        }
    }
}