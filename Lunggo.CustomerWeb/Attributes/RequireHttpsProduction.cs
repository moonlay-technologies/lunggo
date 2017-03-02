using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Properties;
using Lunggo.Framework.Config;

namespace Lunggo.CustomerWeb.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class RequireHttpsProductionAttribute : FilterAttribute, IAuthorizationFilter
    {
        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            if (env == "production" && !filterContext.HttpContext.Request.IsSecureConnection)
                    HandleNonHttpsRequest(filterContext);
            if (env != "production" && filterContext.HttpContext.Request.IsSecureConnection)
                    HandleNonHttpRequest(filterContext);
        }
        protected virtual void HandleNonHttpsRequest(AuthorizationContext filterContext)
        {
            if (!string.Equals(filterContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                return;
            var url = "https://" + filterContext.HttpContext.Request.Url.Host +
                         filterContext.HttpContext.Request.RawUrl;
            filterContext.Result = new RedirectResult(url);
        }

        protected virtual void HandleNonHttpRequest(AuthorizationContext filterContext)
        {
            if (!string.Equals(filterContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                return;
            var url = "http://" + filterContext.HttpContext.Request.Url.Host +
                         filterContext.HttpContext.Request.RawUrl;
            filterContext.Result = new RedirectResult(url);
        }
    }
}