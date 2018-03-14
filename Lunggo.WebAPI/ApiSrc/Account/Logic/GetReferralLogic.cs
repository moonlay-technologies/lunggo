using Lunggo.ApCommon.Account.Model.Logic;
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
        public static ApiResponseBase GetReferral()
        {
            var user = HttpContext.Current.User;
            var a = user.Identity.GetId();
            if(a == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERR_UNAUTHORIZED"
                };
            }
            var output = AccountService.GetInstance().GetReferral(user.Identity.GetId());
            var response = AssambleApiResponse(output);
            return response;
        }

        public static GetReferralApiResponse AssambleApiResponse(GetReferralOutput getReferralOutput)
        {
            if (getReferralOutput == null)
            {
                return new GetReferralApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_NO_REFERRALCODE"
                };
            }

            return new GetReferralApiResponse
            {
                ReferralCode = getReferralOutput.ReferralCode,
                ReferralCredit = getReferralOutput.ReferralCredit,
                ExpDate = getReferralOutput.ExpDate,
                ShareableLink = getReferralOutput.ShareableLink,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}