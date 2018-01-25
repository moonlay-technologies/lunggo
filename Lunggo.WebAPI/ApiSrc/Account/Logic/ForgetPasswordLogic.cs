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
        public static ForgetPasswordApiResponse ForgetPassword(ForgetPasswordApiRequest apiRequest)
        {
            if (apiRequest == null)
            {
                return new ForgetPasswordApiResponse
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
                return new ForgetPasswordApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_FORMAT_PHONENUMBER"
                };
            }

            var input = PreProcess(apiRequest);
            if (AccountService.GetInstance().CheckPhoneNumber(input) == false)
            {
                return new ForgetPasswordApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_PHONENUMBER_NOT_REGISTERED"
                };
            }



            var output = AccountService.GetInstance().ForgetPassword(input);
            var apiResponse = AssambleApiResponse(output);
            return apiResponse;
        }

        public static ForgetPasswordInput PreProcess(ForgetPasswordApiRequest apiRequest)
        {
            return new ForgetPasswordInput
            {
                PhoneNumber = apiRequest.PhoneNumber
            };
        }

        public static ForgetPasswordApiResponse AssambleApiResponse(ForgetPasswordOutput output)
        {
            return new ForgetPasswordApiResponse
            {
                CountryCallCd = output.CountryCallCd,
                PhoneNumber = output.PhoneNumber,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}