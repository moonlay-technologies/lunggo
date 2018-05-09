using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web;
using Lunggo.Framework.Environment;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Microsoft.AspNet.Identity;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static ApiResponseBase ForgotPassword(ForgotPasswordApiRequest request, ApplicationUserManager userManager)
        {
            if (!IsValid(request))
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERAFPW01"
                };
            }

            var foundUser = userManager.FindByName(request.UserName);
            if (foundUser == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.Accepted,
                    ErrorCode = "ERAFPW02"
                };
            }
            if (!userManager.IsEmailConfirmed(foundUser.Id))
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.Accepted,
                    ErrorCode = "ERAFPW03"
                };
            }

            var code = HttpUtility.UrlEncode(userManager.GeneratePasswordResetToken(foundUser.Id));
            var host = EnvVariables.Get("general", "rootUrl");
            var apiUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            var callbackUrl = host + "/id/Account/ResetPassword?code=" + code + "&email=" + request.UserName + "&apiUrl=" + apiUrl;
            userManager.SendEmailAsync(foundUser.Id, "ForgotPasswordEmail", callbackUrl).Wait();
            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.OK
            };
        }

        private static bool IsValid(ForgotPasswordApiRequest request)
        {
            var vc = new ValidationContext(request, null, null);
            return Validator.TryValidateObject(request, vc, null, true);
        }
    }
}