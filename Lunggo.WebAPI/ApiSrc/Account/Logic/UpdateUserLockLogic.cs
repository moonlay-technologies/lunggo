using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Microsoft.AspNet.Identity;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static ApiResponseBase UpdateUserLockLogic(UpdateUserLockRequest request, ApplicationUserManager userManager)
        {
            if (string.IsNullOrEmpty(request.UserId))
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERRDEUS01"
                };
            }
            var foundUser = userManager.FindById(request.UserId);
            if (foundUser == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.Accepted,
                    ErrorCode = "ERRDEUS02"
                };
            }

            var isEnabled = User.UpdateUserLock(foundUser.Id, request.IsLocked);
            //userManager.SetLockoutEnabledAsync(foundUser.Id, true);
            if(isEnabled)
                return new B2BUpdateReservationResponse
                {
                    StatusCode = HttpStatusCode.OK
                };
            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.Accepted,
                ErrorCode = "ERRDEUS03"
            };
        }

    }
}