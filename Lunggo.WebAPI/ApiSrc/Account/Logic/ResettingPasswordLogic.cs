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
            if (!string.IsNullOrEmpty(apiRequest.PhoneNumber) && !string.IsNullOrEmpty(apiRequest.Email))
            {
                apiRequest.Email = null;
            }

            string contact = null;
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
                contact = apiRequest.PhoneNumber;
            }

            else if (!string.IsNullOrEmpty(apiRequest.Email))
            {
                if (AccountService.GetInstance().CheckEmailFormat(apiRequest.Email) == false)
                {
                    return new RequestOtpApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERR_INVALID_FORMAT_EMAIL"
                    };
                }
                contact = apiRequest.Email;
            }

            else
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }

            if (!string.IsNullOrEmpty(apiRequest.PhoneNumber))
            {

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
            }
            else
            {
                if (AccountService.GetInstance().CheckExpireTime(apiRequest.Otp, apiRequest.Email) == false)
                {
                    return new ApiResponseBase
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERR_OTP_EXPIRED"
                    };
                }

                if (AccountService.GetInstance().CheckOtp(apiRequest.Otp, apiRequest.Email) == false)
                {
                    return new ApiResponseBase
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERR_OTP_NOT_VALID"
                    };
                }
            }

            var input = PreProcess(apiRequest);
            var userIds = AccountService.GetInstance().GetIdsByContact(input);
            foreach (var userId in userIds)
            {
                userManager.RemovePasswordAsync(userId).Wait();
                userManager.AddPasswordAsync(userId, input.NewPassword).Wait();
            }

            AccountService.GetInstance().DeleteDataOtp(contact);

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
                Email = apiRequest.Email,
                CountryCallCd = apiRequest.CountryCallCd,
                Otp = apiRequest.Otp,
                NewPassword = apiRequest.NewPassword
            };
        }
    }
}