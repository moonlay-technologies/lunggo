using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using Lunggo.ApCommon.Identity.User;
using Lunggo.Framework.Extension;
using Lunggo.WebAPI.ApiSrc.v1.Accounts.Model;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Microsoft.AspNet.Identity;
using RestSharp;

namespace Lunggo.WebAPI.ApiSrc.v1.Accounts.Logic
{
    public static partial class AccountsLogic
    {
        public static ApiResponseBase ForgotPassword(ForgotPasswordApiRequest request, ApplicationUserManager userManager)
        {
            try
            {
                if (IsValid(request))
                {
                    return new ApiResponseBase
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERAFPW01"
                    };
                }

                var foundUser = userManager.FindByName(request.Email);
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

                var code = userManager.GeneratePasswordResetToken(foundUser.Id);
                //var callbackUrl = Url.Action("ResetPassword", "Account", new { code = code, email = request.Email },
                //    protocol: Request.Url.Scheme);
                //await UserManager.SendEmailAsync(foundUser.Id, "ForgotPasswordEmail", callbackUrl);
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.OK
                };
            }
            catch
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERAFPW99"
                };
            }
        }

        private static bool IsValid(ForgotPasswordApiRequest request)
        {
            var vc = new ValidationContext(request, null, null);
            return Validator.TryValidateObject(request, vc, null, true);
        }
    }
}