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
            if (apiRequest == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }

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
                Otp = apiRequest.Otp
            };
        }


    }
}