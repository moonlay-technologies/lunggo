using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Microsoft.AspNet.Identity;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static ApiResponseBase DeleteUserLogic(DeleteUserApiRequest request, ApplicationUserManager userManager)
        {
            if(request == null || string.IsNullOrEmpty(request.Email))
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERRDELU01"
                };

            var foundUser = userManager.FindByEmail(request.Email);
            if(foundUser == null)
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERRDELU02"
                };
            var roles = userManager.GetRoles(foundUser.Id);
            foreach (var role in roles)
            {
                userManager.RemoveFromRole(foundUser.Id, role);
            }
            var isDeleted = userManager.Delete(foundUser);
            if(isDeleted.Succeeded)
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.OK
                };

            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERRDELU03"
            };
        }
    }
}