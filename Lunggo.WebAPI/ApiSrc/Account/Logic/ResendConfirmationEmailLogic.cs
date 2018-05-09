using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.Framework.Environment;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Microsoft.AspNet.Identity;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static ApiResponseBase ResendConfirmationEmail(ResendConfirmationEmailApiRequest request, ApplicationUserManager userManager)
        {
            if (!IsValid(request))
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERARCE01"
                };
            }

            var foundUser = userManager.FindByEmail(request.Email);
            if (foundUser == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERARCE02"
                };
            }

            if (foundUser.EmailConfirmed)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERARCE03"
                };
            }

            var code = HttpUtility.UrlEncode(userManager.GenerateEmailConfirmationToken(foundUser.Id));
            var host = EnvVariables.Get("general", "rootUrl");
            var apiUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            var callbackUrl = host + "/id/Account/ConfirmEmail?userId=" + foundUser.Id + "&code=" + code + "&apiUrl=" + apiUrl;
            userManager.SendEmailAsync(foundUser.Id, "UserConfirmationEmail", callbackUrl).Wait();

            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.OK
            };
        }

        private static bool IsValid(ResendConfirmationEmailApiRequest request)
        {
            var vc = new ValidationContext(request, null, null);
            return Validator.TryValidateObject(request, vc, null, true);
        }
    }
}