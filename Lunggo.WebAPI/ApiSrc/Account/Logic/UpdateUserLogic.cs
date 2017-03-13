using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lunggo.ApCommon.Identity.Roles;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.Framework.Config;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Microsoft.AspNet.Identity;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static ApiResponseBase UpdateUser(AddUserApiRequest request, ApplicationUserManager userManager)
        {
            if (!IsValid(request))
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERRUPS01"
                };
            }
            var foundUser = userManager.FindByName("b2b:" + request.Email);
            if (foundUser == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.Accepted,
                    ErrorCode = "ERRUPS02"
                };
            }
            string first, last;
            if (request.Name == null)
            {
                first = foundUser.FirstName;
                last = foundUser.LastName;
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
            foundUser.FirstName = first;
            foundUser.LastName = last;

            var user = new User
            {
                Id = foundUser.Id,
                FirstName = foundUser.FirstName,
                LastName = foundUser.LastName,
                Email = request.Email,
                CountryCallCd = request.CountryCallCd,
                PhoneNumber = request.Phone,
                Position = request.Position,
                Department = request.Department,
                Branch = request.Branch,
                ApproverId = request.ApproverId
            };
            var isUpdated = User.UpdateUser(user);
            if (isUpdated)
            {
                if (request.Role != null)
                {
                    request.Role = request.Role.Where(x=> !string.IsNullOrEmpty(x)).ToList();
                    var prevUserRole = Role.GetFromDb(user.Id);
                    if (prevUserRole != null)
                    {
                        foreach (var prevRole in prevUserRole)
                        {
                            userManager.RemoveFromRole(user.Id, prevRole);
                        }
                    }
                    userManager.AddToRolesAsync(user.Id, request.Role.ToArray());
                }

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
    }
}