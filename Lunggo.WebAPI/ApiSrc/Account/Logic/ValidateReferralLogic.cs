using Lunggo.ApCommon.Account.Service;
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
        public static ApiResponseBase ValidateReferral(ValidateReferralApiRequest apiRequest)
        {
            if(apiRequest == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }

            var output = AccountService.GetInstance().GetReferralCodeDataFromDb(apiRequest.ReferralCode);
            if (output == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_REFERRAL_NOT_VALID"
                };
            }
            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}