using Lunggo.WebAPI.ApiSrc.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.WebAPI.ApiSrc.Activity.Model;

namespace Lunggo.WebAPI.ApiSrc.Activity.Logic
{
    public static partial class ActivityLogic
    {
        public static ApiResponseBase GetPendingPayment()
        {
            var output = ActivityService.GetInstance().GetPendingPaymentForAdmin();
            var response = AssembleApiResponse(output);
            return response;
        }

        public static GetPendingPaymentApiResponse AssembleApiResponse(List<PendingPayment> pendingPaymentList)
        {
            var displays = pendingPaymentList.Select(pendingPayment =>
                ActivityService.GetInstance().ConvertToPendingPaymentForDisplay(pendingPayment)).ToList();
            return new GetPendingPaymentApiResponse
            {
                PendingPaymentList = displays,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}