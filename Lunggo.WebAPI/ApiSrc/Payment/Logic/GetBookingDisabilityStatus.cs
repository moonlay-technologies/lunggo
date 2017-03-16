using System.Net;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Payment.Model;

namespace Lunggo.WebAPI.ApiSrc.Payment.Logic
{
    public static partial class PaymentLogic
    {
        public static ApiResponseBase GetBookingDisabilityStatus(string userId)
        {
            var status = PaymentService.GetInstance().CheckBookingDisabilityStatus(userId) ?? true;

            return new GetBookingDisabilityStatusResponse
            {
                IsBookingDisabled = status,
                StatusCode = HttpStatusCode.OK,
            };
        }

    }
}