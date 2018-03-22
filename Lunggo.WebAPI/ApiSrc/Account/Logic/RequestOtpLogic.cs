using Lunggo.WebAPI.ApiSrc.Account.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using Lunggo.ApCommon.Account.Model.Logic;
using Lunggo.ApCommon.Account.Model;
using Lunggo.ApCommon.Account.Service;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static RequestOtpApiResponse RequestOtp(RequestOtpApiRequest apiRequest)
        {
            var input = new RequestOtpInput();
            if (!string.IsNullOrEmpty(apiRequest.PhoneNumber) && !string.IsNullOrEmpty(apiRequest.Email)) 
            {
                apiRequest.Email = null;
            }

            var apiResponse = new RequestOtpApiResponse();

            if (!string.IsNullOrEmpty(apiRequest.PhoneNumber))
            {
                if (AccountService.GetInstance().CheckTimerSms(apiRequest.PhoneNumber, out int? resendCooldown) == false)
                {
                    return new RequestOtpApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERR_TOO_MANY_SEND_SMS_IN_A_TIME",
                        ResendCooldown = resendCooldown
                    };
                }

                if (apiRequest.PhoneNumber.StartsWith("0"))
                {
                    apiRequest.PhoneNumber = apiRequest.PhoneNumber.Substring(1);
                }

                if (AccountService.GetInstance().CheckPhoneNumberFormat(apiRequest.PhoneNumber) == false)
                {
                    return new RequestOtpApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERR_INVALID_FORMAT_PHONENUMBER"
                    };
                }

                input = PreProcess(apiRequest);
                if (AccountService.GetInstance().CheckContactData(input) == false)
                {
                    return new RequestOtpApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERR_PHONENUMBER_NOT_REGISTERED"
                    };
                }

                
            }

            if (!string.IsNullOrEmpty(apiRequest.Email))
            {
                if (AccountService.GetInstance().CheckTimerEmail(apiRequest.Email, out int? resendCooldown) == false)
                {
                    return new RequestOtpApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERR_TOO_MANY_SEND_EMAIL_IN_A_TIME",
                        ResendCooldown = resendCooldown
                    };
                }

                if (AccountService.GetInstance().CheckEmailFormat(apiRequest.Email) == false)
                {
                    return new RequestOtpApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERR_INVALID_FORMAT_EMAIL"
                    };
                }

                input = PreProcess(apiRequest);
                if (AccountService.GetInstance().CheckContactData(input) == false)
                {
                    return new RequestOtpApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERR_EMAIL_NOT_REGISTERED"
                    };
                }
            }
            else if(string.IsNullOrWhiteSpace(apiRequest.Email) && string.IsNullOrWhiteSpace(apiRequest.PhoneNumber))
            {
                return new RequestOtpApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"

                };
            }

            var output = AccountService.GetInstance().RequestOtp(input);
            apiResponse = AssambleApiResponse(output);
            return apiResponse;
        }

        public static RequestOtpInput PreProcess(RequestOtpApiRequest apiRequest)
        {
            return new RequestOtpInput
            {
                PhoneNumber = apiRequest.PhoneNumber,
                Email = apiRequest.Email,
                CountryCallCd = apiRequest.CountryCallCd
            };
        }

        public static RequestOtpApiResponse AssambleApiResponse(RequestOtpOutput output)
        {
            if (output.isSuccess == false)
            {
                return new RequestOtpApiResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERR_INTERNAL"
                };
            }
            return new RequestOtpApiResponse
            {
                CountryCallCd = output.CountryCallCd,
                PhoneNumber = output.PhoneNumber,
                Email = output.Email,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}