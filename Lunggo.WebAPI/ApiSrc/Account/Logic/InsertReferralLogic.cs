using Lunggo.ApCommon.Account.Service;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static ApiResponseBase InsertReferral(InsertReferralApiRequest apiRequest)
        {
            var user = HttpContext.Current.User;
            var userId = user.Identity.GetId();

            if (userId == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERR_UNAUTHORIZED"
                };
            }

            if (apiRequest == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }

            var refer = AccountService.GetInstance().GetReferral(userId);
            if(refer == null || refer.UserId == userId)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_REFERRAL_NOT_VALID"
                };
            }
            AccountService.GetInstance().UpdateReferrerCode(userId, apiRequest.ReferralCode);
            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}