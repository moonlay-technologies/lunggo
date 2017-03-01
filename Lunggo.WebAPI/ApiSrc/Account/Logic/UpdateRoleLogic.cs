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
        public static ApiResponseBase UpdateRoleLogic(UpdateRoleRequest request,ApplicationUserManager userManager)
        {
            if (!IsValid(request))
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERRUR01"
                };
            }
            var foundUser = userManager.FindByEmail(request.UserName);
            if (foundUser == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.Accepted,
                    ErrorCode = "ERRUR02"
                };
            }

            var roles = userManager.GetRoles(foundUser.Id);
            foreach (var role in roles)
            {
                userManager.RemoveFromRole(foundUser.Id, role);
            }
            userManager.AddToRole(foundUser.Id, request.Role);
            return new B2BUpdateReservationResponse
            {
                StatusCode = HttpStatusCode.OK
            };
        }

        private static bool IsValid(UpdateRoleRequest request)
        {
            return !string.IsNullOrEmpty(request.UserName) && !string.IsNullOrEmpty(request.Role);
        }
    }
}