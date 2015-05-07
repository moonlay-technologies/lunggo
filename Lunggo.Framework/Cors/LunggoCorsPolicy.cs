using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;
using Lunggo.Framework.Config;

namespace Lunggo.Framework.Cors
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class LunggoCorsPolicy : Attribute, ICorsPolicyProvider
    {
        private readonly CorsPolicy _policy;

        public LunggoCorsPolicy()
        {
            // Create a CORS policy.
            _policy = GetInitializedCorsPolicy();
            SetCorsOriginDomains(_policy);
        }

        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_policy);
        }

        private CorsPolicy GetInitializedCorsPolicy()
        {
            return new CorsPolicy
            {
                AllowAnyHeader = true,
                AllowAnyMethod = true
            };
        }

        private void SetCorsOriginDomains(CorsPolicy policy)
        {
            var configManager = ConfigManager.GetInstance();
            var origins = configManager.GetConfigValue("general", "corsAllowedDomains").Split(',');

            // Add allowed origins.
            foreach (var origin in origins)
            {
                _policy.Origins.Add(origin);
            }
        }
    }
}
