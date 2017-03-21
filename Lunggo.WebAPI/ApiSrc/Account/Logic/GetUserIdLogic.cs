using System.Collections.Generic;
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
        public static GetUserIdApiResponse GetUserId(ApplicationUserManager userManager)
        {
            var user = HttpContext.Current.User;
            if (user == null)
            {
                return new GetUserIdApiResponse
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERAGUI01"
                };
            }
            var foundId = user.Identity.GetUserId();
            return new GetUserIdApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                Id = foundId
            };
        }
    }
}