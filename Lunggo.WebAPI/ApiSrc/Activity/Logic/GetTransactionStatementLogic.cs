using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Activity.Logic
{
    public static partial class ActivityLogic
    {
        public static ApiResponseBase GetTransactionStatement(ApplicationUserManager userManager)
        {
            var user = HttpContext.Current.User;
            if (user == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERR_UNAUTHORIZED"
                };
            }
            var role = userManager.GetRoles(user.Identity.GetUser().Id).FirstOrDefault();
            if (role != "Operator")
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERR_UNAUTHORIZED"
                };
            }
            var operatorId = user.Identity.GetId();
            var output = ActivityService.GetInstance().GetTransactionStatement(operatorId);
            var apiResponse = AssembleApiResponse(output);
            return apiResponse;
        }

        public static GetTransactionStatementApiResponse AssembleApiResponse(GetTransactionStatementOutput output)
        {
            var transactionStatementsApiResponse = output.TransactionStatements.Select(transactionStatement => convertTransactionStatement(transactionStatement)).ToList();
            return new GetTransactionStatementApiResponse
            {
                TransactionStatements = transactionStatementsApiResponse,
                StatusCode = HttpStatusCode.OK
            };
        }

        public static TransactionStatementForDisplay convertTransactionStatement(TransactionStatement input)
        {
            return new TransactionStatementForDisplay
            {
                Amount = input.Amount,
                DateTime = input.DateTime,
                TrxNo = input.TrxNo,
                Remarks = input.Remarks
            };
        }
    }
}