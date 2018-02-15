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

                if (AccountService.GetInstance().CheckPhoneNumberFormat(apiRequest.PhoneNumber) == false)
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
                if (AccountService.GetInstance().CheckEmailFormat(apiRequest.Email) == false)
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
            if (AccountService.GetInstance().CheckExpireTime(input) == false)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_OTP_EXPIRED"
                };
            }

            if (AccountService.GetInstance().CheckOtp(input) == false)
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