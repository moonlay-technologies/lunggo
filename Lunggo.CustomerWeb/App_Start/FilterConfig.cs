using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using Lunggo.CustomerWeb.Attributes;
using Lunggo.Framework.Config;
using Lunggo.Framework.Filter;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Product.Constant;

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
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var clientId = filterContext.HttpContext.Request.Headers["X-Client-ID"];
                var clientSecret = filterContext.HttpContext.Request.Headers["X-Client-Secret"];
                var identity = filterContext.HttpContext.User.Identity as ClaimsIdentity ?? new ClaimsIdentity();
                if (clientId != null && clientId != "")
                {
                    identity.AddClaim(new Claim("Client ID", clientId));
                    filterContext.Controller.ViewBag.Platform = Client.GetPlatformType(clientId);
                }
                else
                {
                    var mobileUrl = ConfigManager.GetInstance().GetConfigValue("general", "mobileUrl");
                    if (filterContext.HttpContext.Request.Url.Host == mobileUrl)
                    {
                        identity.AddClaim(new Claim("Client ID", "WWxoa2VrOXFSWFZOUXpSM1QycEpORTB5U1RWT1IxcHNXVlJOTTFsWFZYaE5hbVJwVFVSSk5FOUVTbWxOUkVVMFRrUlNhVmxxVlhwT01sbDNUbXBvYkUxNlJUMD0="));
                        filterContext.Controller.ViewBag.Platform = PlatformType.MobileWebsite;
                    }
                    else
                    {
                        identity.AddClaim(new Claim("Client ID", "V2toa2VrOXFSWFZOUXpSM1QycEZlRTlIVlhwYWFrVjVUVVJrYlZsVVp6Vk5WRlp0VGtSR2FrOUhSWGhhYWsweFRucGpNRTE2U1RCT2VtTjNXbTFKZDFwcVFUMD0="));
                        filterContext.Controller.ViewBag.Platform = PlatformType.DesktopWebsite;
                    }
                }
                filterContext.Controller.ViewBag.ClientId = clientId;
                filterContext.Controller.ViewBag.ClientSecret = clientSecret;
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