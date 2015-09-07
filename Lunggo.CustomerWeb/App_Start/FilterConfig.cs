using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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
            filters.Add(new DeviceDetectionFilterAttribute());
            AddBasicAuthenticationFilterAttribute(filters);
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
                "dv1"
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