using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.Framework.Config;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Microsoft.AspNet.Identity;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static ApiResponseBase ResetPassword(ResetPasswordApiRequest request, ApplicationUserManager userManager)
        {
            if (!IsValid(request))
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERARST01"
                };

            var user = userManager.FindByNameAsync(request.UserName).Result;
            if (user == null)
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERARST02"
                };

            var result = string.IsNullOrEmpty(request.Code)
                ? userManager.AddPasswordAsync(user.Id, request.Password).Result
                : userManager.ResetPasswordAsync(user.Id, request.Code, request.Password).Result;
            return result.Succeeded
                ? new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.OK
                }
                : new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERARST03"
                };
        }

        private static bool IsValid(ResetPasswordApiRequest request)
        {
            var vc = new ValidationContext(request, null, null);
            return Validator.TryValidateObject(request, vc, null, true);
        }
    }
}