using System;
using System.Net;
using System.Net.Http;
using System.Web;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Log;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using RestSharp;
using Lunggo.ApCommon.Log;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static LoginApiResponse Login(LoginApiRequest request)
        {
            var TableLog = new GlobalLog();

            TableLog.PartitionKey = "TOKEN ERROR LOG";

            if (request.UserName != null && request.UserName.StartsWith("0"))
            {
                request.UserName = request.UserName.Substring(1);
            }
            if (request.RefreshToken != null && (!string.IsNullOrEmpty(request.UserName) || !string.IsNullOrEmpty(request.Password)))
                return new LoginApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_FORM_EMPTY" //ERALOG01
                };
            if(!string.IsNullOrEmpty(request.UserName))
            {
                long result;
                if (!Int64.TryParse(request.UserName, out result))
                    if (!request.UserName.Contains("@"))
                        return new LoginApiResponse
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            ErrorCode = "ERR_USER_FORMAT"
                        };
            }
            
            var tokenClient = new RestClient(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority));
            var tokenRequest = new RestRequest("/oauth/token", Method.POST);
            var postData =
                "client_id=" + request.ClientId +
                "&client_secret=" + request.ClientSecret;
            if (request.DeviceId != null)
                postData += "&device_id=" + request.DeviceId;
            if (request.RefreshToken != null)
            {
                postData +=
                    "&grant_type=refresh_token" +
                    "&refresh_token=" + request.RefreshToken;
            }
            else if (request.UserName != null || request.Password != null)
            {
                postData +=
                    "&grant_type=password" +
                    "&username=" + request.UserName +
                    "&password=" + request.Password;
            }
            else
            {
                postData +=
                    "&grant_type=client_credentials";
            }
            tokenRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
            var tokenResponse = tokenClient.Execute(tokenRequest);
            try
            {
                var tokenData = tokenResponse.Content.Deserialize<TokenData>();
                if (tokenData.Error == "invalid_password")
                    return new LoginApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERR_INVALID_PASSWORD"
                    };
                if (tokenData.Error == "invalid_grant")
                    return new LoginApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERR_INVALID" //ERALOG02
                    };
                if (tokenData.Error == "not_active")
                    return new LoginApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERR_NOT_ACTIVE" //ERALOG03
                    };
                if (tokenData.Error == "invalid_clientId")
                    return new LoginApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERR_INVALID_CLIENTID" //ERALOG04
                    };
                if (tokenData.Error == "not_registered")
                    return new LoginApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERR_NOT_REGISTERED" //ERALOG05
                    };
                return new LoginApiResponse
                {
                    AccessToken = tokenData.AccessToken,
                    RefreshToken = tokenData.RefreshToken,
                    ExpiryTime = tokenData.ExpiryTime,
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch
            {
                var log = LogService.GetInstance();
                var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
                TableLog.Log = "```Token Error " + env.ToUpper() + "```\n"
                    + tokenResponse.Content;
                log.Post(TableLog.Log
                    ,
                    env == "production" ? "#logging-prod" : "#logging-dev");
                TableLog.Logging();
                throw;
            }
        }
    }
}