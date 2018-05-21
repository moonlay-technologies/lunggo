using System;
using System.Net;
using System.Security.Principal;
using System.Web;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Payment.Model;
using Microsoft.AspNet.Identity;

namespace Lunggo.WebAPI.ApiSrc.Payment.Logic
{
    public static partial class PaymentLogic
    {
        public static ApiResponseBase GetUserBankAccounts()
        {
            var userId = HttpContext.Current?.User?.Identity?.GetUser()?.Id;
            if (string.IsNullOrWhiteSpace(userId))
                return ApiResponseBase.Error(HttpStatusCode.Unauthorized, "ERR_UNAUTHORIZED");

            var accounts = new PaymentService().GetUserBankAccounts(userId);
            return new GetUserBankAccountApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                BankAccounts = accounts
            };
        }
    }
}