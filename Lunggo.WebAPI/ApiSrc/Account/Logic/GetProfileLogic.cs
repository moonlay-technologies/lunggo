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
            var first = foundUser.FirstName ?? "";
            var last = foundUser.LastName ?? "";
            if (first == last)
                name = last;
            else
                name = first + " " + last;
            return new GetProfileApiResponse
            {
                Email = foundUser.Email ?? "",
                Name = name,
                CountryCallingCd = foundUser.CountryCd ?? "",
                PhoneNumber = foundUser.PhoneNumber ?? "",
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}