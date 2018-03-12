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
        public static ApiResponseBase GetReferralDetail()
        {
            var user = HttpContext.Current.User;
            if(user == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERR_UNAUTHORIZED"
                };
            }
            var output = AccountService.GetInstance().GetReferralDetail(user.Identity.GetId());
            var response = AssambleApiResponse(output);
            return response;
        }

        public static GetReferralDetailApiResponse AssambleApiResponse(GetReferralDetailOutput output)
        {
            var response = AccountService.GetInstance().ConvertToReferralHistoryModelDisplay(output.ReferralDetail);
            return new GetReferralDetailApiResponse
            {
                ReferralDetail = response,
                StatusCode = HttpStatusCode.OK,
            };
        }
    }
}