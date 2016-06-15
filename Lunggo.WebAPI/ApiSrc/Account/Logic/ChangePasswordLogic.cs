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
                var user = HttpContext.Current.User;
                var result = userManager.ChangePassword(user.Identity.GetUserId(), request.OldPassword, request.NewPassword);
                if (result.Succeeded)
                {
                    return new ApiResponseBase
                    {
                        StatusCode = HttpStatusCode.OK
                    };
                }

                return ApiResponseBase.Error500();
            }
            catch
            {
                return ApiResponseBase.Error500();
            }
        }
    }
}