using Lunggo.WebAPI.ApiSrc.Activity.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lunggo.ApCommon.Activity.Service;

namespace Lunggo.WebAPI.ApiSrc.Activity.Logic
{
    public partial class ActivityLogic
    {
        public static ApiResponseBase MarkPaymentAsFailed(MarkPaymentAsFailedApiRequest apiRequest)
        {
            if (apiRequest == null)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERR_INVALID_REQUEST"
                };
            }

            var output = ActivityService.GetInstance()
                .MarkPendingPaymentAsFailed(apiRequest.RsvNo, apiRequest.PendingPaymentStatus);

            if (output)
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
    }
}