using System.Linq;
using System.Net;
using System.Web;

using Lunggo.ApCommon.Identity.Users;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using Microsoft.AspNet.Identity;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static GetProfileApiResponse GetProfile(ApplicationUserManager userManager)
        {
            var user = HttpContext.Current.User;
            if (user == null)
            {
                return new GetProfileApiResponse
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERAGPR01"
                };
            }
            var foundUser = user.Identity.GetUser();
            string name;
            var userRole = userManager.GetRoles(foundUser.Id).ToList();
            var first = foundUser.FirstName ?? "";
            var last = foundUser.LastName ?? "";
            if (first == last)
                name = last;
            else
                name = first + " " + last;
            return new GetProfileApiResponse
            {
                UserName = foundUser.UserName ?? "",
                Email = foundUser.Email ?? "",
                Name = name,
                CountryCallingCd = foundUser.CountryCallCd ?? "",
                PhoneNumber = foundUser.PhoneNumber ?? "",
                RoleName = userRole,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}