using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
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
        public static ApiResponseBase ChangeProfile(ChangeProfileApiRequest request, ApplicationUserManager userManager, IPrincipal user)
        {
            try
            {
                var updatedUser = user.Identity.GetUser();
                updatedUser.FirstName = request.FirstName ?? updatedUser.FirstName;
                updatedUser.LastName = request.LastName ?? updatedUser.LastName;
                updatedUser.CountryCd = request.CountryCallingCd ?? updatedUser.CountryCd;
                updatedUser.PhoneNumber = request.PhoneNumber ?? updatedUser.PhoneNumber;
                updatedUser.Address = request.Address ?? updatedUser.Address;
                var result = userManager.Update(updatedUser);
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
                    ErrorCode = "ERACPR99"
                };
            }
            catch
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERACPR99"
                };
            }
        }
    }
}