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
        public static ApiResponseBase VerifyPhone(VerifyPhoneApiRequest apiRequest)
        {
            if (!string.IsNullOrEmpty(apiRequest.PhoneNumber))
            {
                if (apiRequest.PhoneNumber.StartsWith("0"))
                {
                    apiRequest.PhoneNumber = apiRequest.PhoneNumber.Substring(1);
                }

                if (AccountService.GetInstance().CheckPhoneNumberFormat(apiRequest.PhoneNumber) == false)
                {
                    return new ApiResponseBase
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERR_INVALID_FORMAT_PHONENUMBER"
                    };
                }
            }

            else
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }

            var user = HttpContext.Current.User.Identity.GetUser();
            if (user.CountryCallCd != apiRequest.CountryCallCd || user.PhoneNumber != apiRequest.PhoneNumber)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERR_UNAUTHORIZED"
                };
            }

            if (AccountService.GetInstance().CheckExpireTime(apiRequest.Otp, apiRequest.CountryCallCd, apiRequest.PhoneNumber) == false)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_OTP_EXPIRED"
                };
            }

            if (AccountService.GetInstance().CheckOtp(apiRequest.Otp, apiRequest.CountryCallCd, apiRequest.PhoneNumber) == false)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_OTP_NOT_VALID"
                };
            }

            if (AccountService.GetInstance().VerifyPhoneWithOtp(apiRequest.Otp, user) == false)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERR_INTERNAL"
                };
            }

            AccountService.GetInstance().DeleteDataOtp(apiRequest.PhoneNumber);

            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.OK
            };

        }
    }
}