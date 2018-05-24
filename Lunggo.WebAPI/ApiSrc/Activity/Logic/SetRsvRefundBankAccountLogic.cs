using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Activity.Logic
{
    public static partial class ActivityLogic
    {
        public static ApiResponseBase SetRsvRefundBankAccount(SetRsvRefundBankAccountApiRequest apiRequest)
        {
            if (apiRequest == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }

            var userId = HttpContext.Current.User.Identity.GetId();
            if (userId == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_USER_UNDEFINED"
                };
            }

            var rsvValid = PreProcess(apiRequest.RsvNo);
            if (!rsvValid)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_RSVNO"
                };
            }

            var isSuccess = ActivityService.GetInstance().SetRsvRefundBankAccount(apiRequest.RsvNo, apiRequest.BankAccount);
            return new ApiResponseBase
            {
                StatusCode = isSuccess ? HttpStatusCode.OK : HttpStatusCode.InternalServerError
            };
        }

        public static bool PreProcess(string rsvNo)
        {
            var rsvNoCheck = ActivityService.GetInstance().GetMyBookingDetail(new GetMyBookingDetailInput { RsvNo = rsvNo });
            return rsvNoCheck != null;
        }
    }
}