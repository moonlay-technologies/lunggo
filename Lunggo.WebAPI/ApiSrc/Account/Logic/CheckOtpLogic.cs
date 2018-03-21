using Lunggo.ApCommon.Account.Model.Logic;
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
        public static ApiResponseBase CheckOtp(CheckOtpApiRequest apiRequest)
        {
            var accountService = AccountService.GetInstance();
            if (!string.IsNullOrEmpty(apiRequest.PhoneNumber) && !string.IsNullOrEmpty(apiRequest.Email))
            {
                apiRequest.Email = null;
            }
                        
            if (!string.IsNullOrEmpty(apiRequest.PhoneNumber))
            {
                if (apiRequest.PhoneNumber.StartsWith("0"))
                {
                    apiRequest.PhoneNumber = apiRequest.PhoneNumber.Substring(1);
                }
                var isValidFormatNumber = accountService.CheckPhoneNumberFormat(apiRequest.PhoneNumber);
                if (isValidFormatNumber == false)
                {
                    return new ApiResponseBase
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERR_INVALID_FORMAT_PHONENUMBER"
                    };
                }               
            }

            else if (!string.IsNullOrEmpty(apiRequest.Email))
            {
                var isValidEmailFormat = accountService.CheckEmailFormat(apiRequest.Email);
                if (isValidEmailFormat == false)
                {
                    return new ForgetPasswordApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERR_INVALID_FORMAT_EMAIL"
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

            var input = PreProcess(apiRequest);
            var isOtpExpired = accountService.CheckExpireTime(input);
            if (isOtpExpired == false)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_OTP_EXPIRED"
                };
            }

            var isOtpValid = accountService.CheckOtp(input);
            if (isOtpValid == false)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_OTP_NOT_VALID"
                };
            }
            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.OK
            };

        }


        public static CheckOtpInput PreProcess(CheckOtpApiRequest apiRequest)
        {
            return new CheckOtpInput
            {
                PhoneNumber = apiRequest.PhoneNumber,
                Email = apiRequest.Email,
                Otp = apiRequest.Otp
            };
        }


    }
}