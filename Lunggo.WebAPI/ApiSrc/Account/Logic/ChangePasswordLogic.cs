using System.Net;
using System.Web;
using Lunggo.ApCommon.Identity.User;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Microsoft.AspNet.Identity;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static ApiResponseBase ChangePassword(ChangePasswordApiRequest request, ApplicationUserManager userManager)
        {
            try
            {
                if (request.NewPassword != request.ConfirmPassword)
                    return new ApiResponseBase
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERACHP01"
                    };

                var user = HttpContext.Current.User;
                var result = userManager.ChangePassword(user.Identity.GetUserId(), request.OldPassword, request.NewPassword);
                if (result.Succeeded)
                {
                    return new ApiResponseBase
                    {
                        StatusCode = HttpStatusCode.OK
                    };
                }
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERRGEN99"
                };
            }
            catch
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERRGEN99"
                };
            }
        }
    }
}