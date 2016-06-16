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
            var user = HttpContext.Current.User;
            var userId = user.Identity.GetUser().Id;
            var result = userManager.ChangePassword(userId, request.OldPassword, request.NewPassword);
            return result.Succeeded
                ? new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.OK
                }
                : new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERACHP01"
                };
        }
    }
}