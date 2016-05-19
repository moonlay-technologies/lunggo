using System.Net;
using System.Web;
using Lunggo.ApCommon.Identity.User;
using Lunggo.WebAPI.ApiSrc.Account.Model;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static GetProfileApiResponse GetProfile()
        {
            var user = HttpContext.Current.User;
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
                    CountryCallingCd = foundUser.CountryCd ?? "",
                    PhoneNumber = foundUser.PhoneNumber ?? ""
                };
            }
            catch
            {
                return new GetProfileApiResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERRGEN99"
                };
            }
        }
    }
}