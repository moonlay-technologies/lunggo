using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Http.Results;
using Lunggo.ApCommon.Identity.Model;
using Lunggo.ApCommon.Identity.Users;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static UserData GetUser()
        {
            var user = HttpContext.Current.User.Identity;
            var userId = HttpContext.Current.User.Identity.GetUser().Id;
            return new UserData();
        }
    }
}