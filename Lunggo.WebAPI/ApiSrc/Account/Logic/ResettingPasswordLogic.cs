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
        public static ApiResponseBase ResettingPassword(ResettingPasswordApiRequest apiRequest, ApplicationUserManager userManager)
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
            var checkOtpInput = new CheckOtpInput
            {
                PhoneNumber = input.PhoneNumber,
                Otp = input.Otp
            };

            if (AccountService.GetInstance().CheckOtp(checkOtpInput) == false)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_OTP_NOT_VALID"
                };
            }

            if (AccountService.GetInstance().CheckExpireTime(checkOtpInput) == false)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_OTP_EXPIRED"
                };
            }

            var userIds = AccountService.GetInstance().GetIdsByPhoneNumber(input);
            foreach (var userId in userIds)
            {
               userManager.RemovePasswordAsync(userId).Wait();
               userManager.AddPasswordAsync(userId, input.NewPassword).Wait();
            }

            AccountService.GetInstance().DeleteDataOtp(input.PhoneNumber);
            
            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.OK
            };
        }


        public static ResettingPasswordInput PreProcess(ResettingPasswordApiRequest apiRequest)
        {
            return new ResettingPasswordInput
            {
                PhoneNumber = apiRequest.PhoneNumber,
                Otp = apiRequest.Otp,
                NewPassword = apiRequest.NewPassword
            };
        }
    }
}