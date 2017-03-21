using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public static ApiResponseBase AddUser(AddUserApiRequest request, ApplicationUserManager userManager)
        {
            if (!IsValid(request))
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERRAU01"
                };
            }
            var foundUser = userManager.FindByName("b2b:" + request.Email);
            if (foundUser != null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.Accepted,
                    ErrorCode = foundUser.EmailConfirmed
                        ? "ERRAU02"
                        : "ERRAU03"
                };
            }

            string first, last;
            if (request.Name == null)
            {
                first = "";
                last = "";
            }
            else
            {
                var splittedName = request.Name.Split(' ');
                if (splittedName.Length == 1)
                {
                    first = request.Name;
                    last = request.Name;
                }
                else
                {
                    first = request.Name.Substring(0, request.Name.LastIndexOf(' '));
                    last = splittedName[splittedName.Length - 1];
                }
            }

            var recentUser = HttpContext.Current.User.Identity.GetUser().Id;
            var companyId = User.GetCompanyIdByUserId(recentUser);
            var user = new User
            {
                UserName = "b2b:" + request.Email,
                CompanyId = companyId,
                FirstName = first,
                LastName = last,
                Email = request.Email,
                CountryCallCd = request.CountryCallCd,
                PhoneNumber = request.Phone,
                Position = request.Position,
                Department = request.Department,
                Branch = request.Branch,
                ApproverId = request.ApproverId
            };
            var result = userManager.Create(user);
            if (result.Succeeded)
            {
                //Add Role
                if (request.Role != null)
                {
                    request.Role = request.Role.Where(x => !string.IsNullOrEmpty(x)).ToList();
                    userManager.AddToRolesAsync(user.Id, request.Role.ToArray());
                }
                var code = HttpUtility.UrlEncode(userManager.GenerateEmailConfirmationToken(user.Id));
                var host = ConfigManager.GetInstance().GetConfigValue("general", "rootUrl");
                var apiUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                var callbackUrl = host + "/id/Account/ConfirmEmail?userId=" + user.Id + "&code=" + code + "&isAgentType=true" + "&apiUrl=" + apiUrl;
                userManager.SendEmailAsync(user.Id, "UserConfirmationEmail", callbackUrl).Wait();

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
                    ErrorCode = "ERRGEN99"
                };
            }
        }

        private static bool IsValid(AddUserApiRequest request)
        {
            return !string.IsNullOrEmpty(request.Email);
        }
    }
}