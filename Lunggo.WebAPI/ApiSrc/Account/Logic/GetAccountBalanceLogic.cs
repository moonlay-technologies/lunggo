using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static ApiResponseBase GetAccountBalance()
        {
            var balance = new PaymentService().GetBalance();
            var apiResponse = AssembleApiResponse(balance);
            return apiResponse;
        }

        private static ApiResponseBase AssembleApiResponse(AccountBalance balance)
        {
            return new GetAccountBalanceApiResponse
            {
                Balance = balance.Balance,
                Withdrawable = balance.Withdrawable
            };
        }
    }
}