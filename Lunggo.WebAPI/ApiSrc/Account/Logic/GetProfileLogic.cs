using System.Net;
using System.Web;

using Lunggo.ApCommon.Identity.Users;
using Lunggo.WebAPI.ApiSrc.Account.Model;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static GetProfileApiResponse GetProfile()
        {
            var user = HttpContext.Current.User;
            if (string.IsNullOrWhiteSpace(user.Identity.Name))
            {
                return new GetProfileApiResponse
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERR_UNDEFINED_USER"
                };
            }
            var foundUser = user.Identity.GetUser();
            string name;
            var first = foundUser.FirstName ?? "";
            var last = foundUser.LastName ?? "";
            var avatar = "http://www.personalbrandingblog.com/wp-content/uploads/2017/08/blank-profile-picture-973460_640-300x300.png";
            if (first == last)
                name = last;
            else
                name = first + " " + last;
            return new GetProfileApiResponse
            {
                Email = foundUser.Email ?? "",
                Name = name,
                CountryCallingCd = foundUser.CountryCallCd ?? "",
                PhoneNumber = foundUser.PhoneNumber ?? "",
                Avatar = avatar ?? "http://www.personalbrandingblog.com/wp-content/uploads/2017/08/blank-profile-picture-973460_640-300x300.png",
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}