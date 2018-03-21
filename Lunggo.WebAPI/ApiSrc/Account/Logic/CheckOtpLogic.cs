using Lunggo.ApCommon.Account.Model.Logic;
using Lunggo.ApCommon.Account.Service;
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
        public static ApiResponseBase CheckOtp(CheckOtpApiRequest apiRequest)
        {
            var accountService = AccountService.GetInstance();
            if (!IsNullOrEmpty(apiRequest.PhoneNumber) && !IsNullOrEmpty(apiRequest.Email))
            {
                apiRequest.Email = null;
            }

            if (!IsNullOrEmpty(apiRequest.PhoneNumber))
            {
                if (apiRequest.PhoneNumber.StartsWith("0"))
                {
                    apiRequest.PhoneNumber = apiRequest.PhoneNumber.Substring(1);
                }
                var isValidFormatNumber = accountService.CheckPhoneNumberFormat(apiRequest.PhoneNumber);
                if (isValidFormatNumber == false)
                {
                    return Error(BadRequest, "ERR_INVALID_FORMAT_PHONENUMBER");
                }
            }

            else if (!IsNullOrEmpty(apiRequest.Email))
            {
                var isValidEmailFormat = accountService.CheckEmailFormat(apiRequest.Email);
                if (isValidEmailFormat == false)
                {
                    return Error(BadRequest, "ERR_INVALID_FORMAT_EMAIL");
                }
            }

            else
            {
                return Error(BadRequest, "ERR_INVALID_REQUEST");
            }

            if (AccountService.GetInstance().CheckExpireTime(apiRequest.Otp, apiRequest.CountryCallCd, apiRequest.PhoneNumber) == false)
            {
                return Error(BadRequest, "ERR_OTP_EXPIRED");
            }

            var isOtpValid = accountService.CheckOtp(apiRequest.Otp, apiRequest.CountryCallCd, apiRequest.PhoneNumber);
            if (isOtpValid == false)
            {
                return Error(BadRequest, "ERR_OTP_NOT_VALID");
            }
            return new ApiResponseBase
            {
               StatusCode = OK
            };
        }
    }
}