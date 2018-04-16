using Lunggo.ApCommon.Account.Model.Logic;
using Lunggo.ApCommon.Account.Service;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using static Lunggo.WebAPI.ApiSrc.Common.Model.ApiResponseBase;
using static System.Net.HttpStatusCode;
using static System.String;
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
            var account = AccountService.GetInstance();

            if (!ValidateInputFormat()) return Error(BadRequest, "ERR_INVALID_REQUEST");
            if (!ValidatePhoneNumberFormat()) return Error(BadRequest, "ERR_INVALID_FORMAT_PHONENUMBER");

            var user = HttpContext.Current.User.Identity.GetUser();
            if (!ValidateAuthorization()) return Error(Unauthorized, "ERR_UNAUTHORIZED");

            var isOtpExpired = !account.CheckExpireTime(apiRequest.Otp, apiRequest.CountryCallCd, apiRequest.PhoneNumber);
            if (isOtpExpired) return Error(BadRequest, "ERR_OTP_EXPIRED");

            var isOtpValid = account.CheckOtp(apiRequest.Otp, apiRequest.CountryCallCd, apiRequest.PhoneNumber);
            if (!isOtpValid) return Error(BadRequest, "ERR_OTP_NOT_VALID");

            var isSuccessVerify = account.VerifyPhoneWithOtp(apiRequest.Otp, user);
            if (!isSuccessVerify) return Error(InternalServerError, "ERR_INTERNAL");

            account.DeleteDataOtp(apiRequest.PhoneNumber);

            return new ApiResponseBase
            {
                StatusCode = OK
            };

            #region Validation

            bool ValidateInputFormat()
            {
                return !IsNullOrEmpty(apiRequest.PhoneNumber);
            }

            bool ValidatePhoneNumberFormat()
            {
                if (apiRequest.PhoneNumber.StartsWith("0"))
                {
                    apiRequest.PhoneNumber = apiRequest.PhoneNumber.Substring(1);
                }

                var isPhoneNumberFormatValid = account.CheckPhoneNumberFormat(apiRequest.PhoneNumber);

                if (!isPhoneNumberFormatValid)
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