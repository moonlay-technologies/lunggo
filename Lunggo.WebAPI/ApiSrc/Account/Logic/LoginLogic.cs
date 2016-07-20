using System;
using System.Net;
using System.Net.Http;
using System.Web;
using Lunggo.Framework.Extension;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using RestSharp;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static LoginApiResponse Login(LoginApiRequest request)
        {
            if (request.RefreshToken != null && (request.UserName != null || request.Password != null))
                return new LoginApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERALOG01"
                };

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
            var tokenData = tokenResponse.Content.Deserialize<TokenData>();
            if (tokenData.Error == "invalid_grant")
                return new LoginApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERALOG02"
                };
            if (tokenData.Error == "not_active")
                return new LoginApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERALOG03"
                };
            if (tokenData.Error == "invalid_clientId")
                return new LoginApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERALOG04"
                };
            if (tokenData.Error == "not_registered")
                return new LoginApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERALOG05"
                };
            return new LoginApiResponse
            {
                AccessToken = tokenData.AccessToken,
                RefreshToken = tokenData.RefreshToken,
                ExpiryTime = tokenData.ExpiryTime,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}