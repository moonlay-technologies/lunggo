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
        public static ApiResponseBase InsertTransactionStatement(InsertTransactionStatementApiRequest apiRequest, ApplicationUserManager userManager)
        {
            if (apiRequest == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };                
            }
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
            if (role != "Admin")
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    ErrorCode = "ERR_UNAUTHORIZED"
                };
            }
            var input = PreProcess(apiRequest);
            var isSuccess = ActivityService.GetInstance().InsertTransactionStatement(input);
            if (isSuccess)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }
        }

        public static InsertTransactionStatementInput PreProcess(InsertTransactionStatementApiRequest apiRequest)
        {
            return new InsertTransactionStatementInput
            {
                Amount = apiRequest.Amount,
                OperatorId = apiRequest.OperatorId,
                Remarks = apiRequest.Remarks,
                DateTime = apiRequest.DateTime
            };
        }
    }
}