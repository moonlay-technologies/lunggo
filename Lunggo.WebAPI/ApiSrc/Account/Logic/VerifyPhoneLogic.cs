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
            if (!ValidateInputFormat()) return ApiResponseBase.Error(HttpStatusCode.BadRequest, "ERR_INVALID_REQUEST");
            if (!ValidatePhoneNumberFormat()) return ApiResponseBase.Error(HttpStatusCode.BadRequest, "ERR_INVALID_FORMAT_PHONENUMBER");

            var user = HttpContext.Current.User.Identity.GetUser();
            if (!ValidateAuthorization()) return ApiResponseBase.Error(HttpStatusCode.Unauthorized, "ERR_UNAUTHORIZED");

            var isOtpExpired = !AccountService.GetInstance().CheckExpireTime(apiRequest.Otp, apiRequest.CountryCallCd, apiRequest.PhoneNumber);
            if (isOtpExpired) return ApiResponseBase.Error(HttpStatusCode.BadRequest, "ERR_OTP_EXPIRED");

            var isOtpValid = AccountService.GetInstance().CheckOtp(apiRequest.Otp, apiRequest.CountryCallCd, apiRequest.PhoneNumber);
            if (!isOtpValid) return ApiResponseBase.Error(HttpStatusCode.BadRequest, "ERR_OTP_NOT_VALID");

            var isSuccessVerify = AccountService.GetInstance().VerifyPhoneWithOtp(apiRequest.Otp, user);
            if (!isSuccessVerify) return ApiResponseBase.Error(HttpStatusCode.InternalServerError, "ERR_INTERNAL");

            AccountService.GetInstance().DeleteDataOtp(apiRequest.PhoneNumber);

            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.OK
            };

            #region Validation

            bool ValidateInputFormat()
            {
                return !string.IsNullOrEmpty(apiRequest.PhoneNumber);
            }

            bool ValidatePhoneNumberFormat()
            {
                if (apiRequest.PhoneNumber.StartsWith("0"))
                {
                    apiRequest.PhoneNumber = apiRequest.PhoneNumber.Substring(1);
                }

                if (AccountService.GetInstance().CheckPhoneNumberFormat(apiRequest.PhoneNumber) == false)
                {
                    return false;
                }

                return true;
            }

            bool ValidateAuthorization()
            {
                if (user.CountryCallCd != apiRequest.CountryCallCd || user.PhoneNumber != apiRequest.PhoneNumber)
                {
                    return false;
                }
                return true;
            }

            #endregion
        }
    }
}