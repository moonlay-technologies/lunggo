using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lunggo.ApCommon.Identity.Model;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.CustomerWeb.Models;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using RestSharp;

namespace Lunggo.CustomerWeb.Utils
{
    internal static class B2BUtil
    {
        internal static User GetB2BUser(HttpRequestBase rq)
        {
            var apiUrl = ConfigManager.GetInstance().GetConfigValue("api", "apiUrl");
            var client = new RestClient(apiUrl);

            var request = new RestRequest("/v1/b2b/userid", Method.GET);
            var key = rq.Cookies["authkey"];
            if (key == null)
                return null;

            request.AddHeader("Authorization", "Bearer " + key.Value);
            // execute the request
            var response = client.Execute(request);
            var content = response.Content.Deserialize<dynamic>();
            if (response.StatusCode != HttpStatusCode.OK || content.status != "200") 
                return null;

            User user = User.GetFromDb((string) content.id);
            return user.UserName.StartsWith("b2b:") ? user : null;
        }

        internal static bool IsB2BAuthorized(HttpRequestBase rq)
        {
            if (!IsB2BDomain(rq))
                return true;
            var baseUrl = ConfigManager.GetInstance().GetConfigValue("api", "apiUrl");
            var client = new RestClient(baseUrl);

            var request = new RestRequest("/v1/profile", Method.GET);
            var key = rq.Cookies["authkey"];
            if (key == null)
                return false;

            request.AddHeader("Authorization", "Bearer " + key.Value);
            // execute the request
            var response = client.Execute<GetProfileModel>(request);
            if (response.Data == null || response.Data.UserName == null) return false;
            if (response.Data.UserName.Contains("b2b:"))
                return true;
            return false;
        }

        internal static bool IsB2BDomain(HttpRequestBase rq)
        {
            if (rq.Url != null)
            {
                var host = rq.Url.Host;
                var b2bDesktop = ConfigManager.GetInstance().GetConfigValue("general", "b2bRootUrl");
                var b2bMobile = ConfigManager.GetInstance().GetConfigValue("general", "b2bMobileUrl");
                if (host == b2bDesktop || host == b2bMobile)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}