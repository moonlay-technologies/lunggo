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
        public static GetProfileApiResponse GetProfile(IPrincipal user)
        {
            try
            {
                if (user == null)
                {
                    return new GetProfileApiResponse
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        ErrorCode = "ERAGPR01"
                    };
                }
                var foundUser = user.Identity.GetUser();
                return new GetProfileApiResponse
                {
                    Email = foundUser.Email ?? "",
                    FirstName = foundUser.FirstName ?? "",
                    LastName = foundUser.LastName ?? "",
                    CountryCd = foundUser.CountryCd ?? "",
                    PhoneNumber = foundUser.PhoneNumber ?? "",
                    Address = foundUser.Address ?? ""
                };
            }
            catch
            {
                return new GetProfileApiResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERAGPR99"
                };
            }
        }
    }
}