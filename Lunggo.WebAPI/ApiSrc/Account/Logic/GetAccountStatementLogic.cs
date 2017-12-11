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
        public static ApiResponseBase GetAccountStatement(GetAccountStatementApiRequest request)
        {
            DateTime fromDate;
            DateTime toDate;
            var fromDateOk = DateTime.TryParse(request.FromDate, out fromDate);
            var toDateOk = DateTime.TryParse(request.ToDate, out toDate);
            if (!fromDateOk || !toDateOk)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }

            var transactions = PaymentService.GetInstance().GetTransactions(fromDate, toDate);
            var apiResponse = AssembleApiResponse(transactions);
            return apiResponse;
        }

        private static ApiResponseBase AssembleApiResponse(List<Transaction> transactions)
        {
            return new GetAccountStatementApiResponse
            {
                Transactions = transactions
            };
        }
    }
}