using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Lunggo.Framework.Config;

namespace Lunggo.ApCommon.Identity.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class Level2AuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");

            if (env == "production")
            {
                var identity = actionContext.RequestContext.Principal.Identity as ClaimsIdentity ?? new ClaimsIdentity();
                return base.IsAuthorized(actionContext) && identity.HasClaim(ClaimTypes.Authentication, "password");
            }
            else
            {
                return true;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class Level1AuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");

            if (env == "production")
            {
                return base.IsAuthorized(actionContext);
            }
            else
            {
                return true;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class Level0AuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return true;
        }
    }
}
