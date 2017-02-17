using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Http.Results;
using Lunggo.ApCommon.Identity.Model;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.WebAPI.ApiSrc.Account.Model;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static GetUserResponse GetUser()
        {
            var userId = HttpContext.Current.User.Identity.GetUser().Id;
            var response = User.GetAllUserByCompanyId(userId);
            var roles = User.GetAllRoles();
            if(response == null)
                return new GetUserResponse
                {
                    StatusCode = HttpStatusCode.Accepted
                };
            return new GetUserResponse
            {
                StatusCode = HttpStatusCode.OK,
                Users = response,
                Roles = roles
            };
        }
    }
}