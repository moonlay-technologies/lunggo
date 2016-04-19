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
        public static ApiResponseBase Register(RegisterApiRequest request, ApplicationUserManager userManager)
        {
            try
            {
                if (!IsValid(request))
                {
                    return new ApiResponseBase
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERAREG01"
                    };
                }

                var foundUser = userManager.FindByEmail(request.Email);
                if (foundUser != null)
                {
                    return new ApiResponseBase
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = foundUser.EmailConfirmed
                            ? "ERAREG02"
                            : "ERAREG03"
                    };
                }

                var user = new User
                {
                    UserName = request.Email,
                    Email = request.Email
                };
                var result = userManager.Create(user);
                if (result.Succeeded)
                {
                    var code = userManager.GenerateEmailConfirmationToken(user.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code },
                    //    protocol: Request.Url.Scheme);
                    //await UserManager.SendEmailAsync(user.Id, "UserConfirmationEmail", callbackUrl);

                    return new ApiResponseBase
                    {
                        StatusCode = HttpStatusCode.OK
                    };
                }
                else
                {
                    return new ApiResponseBase
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrorCode = "ERAREG99"
                    };
                }
            }
            catch
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERAREG99"
                };
            }
        }

        private static bool IsValid(RegisterApiRequest request)
        {
            var vc = new ValidationContext(request, null, null);
            return Validator.TryValidateObject(request, vc, null, true);
        }
    }
}