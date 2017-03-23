using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Http.Results;
using Lunggo.ApCommon.Identity.Model;
using Lunggo.ApCommon.Identity.Roles;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using ServiceStack.Text;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static ApiResponseBase SaveBookingNotesLogic(SaveBookingNotesApiRequest request)
        {
            var userId = HttpContext.Current.User.Identity.GetUser().Id;
            User.InsertBookingNotes(userId, request.Title, request.Description);
            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.OK
            };
        }

    }
}