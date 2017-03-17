using System.Net;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Payment.Model;

namespace Lunggo.WebAPI.ApiSrc.Payment.Logic
{
    public static partial class PaymentLogic
    {
        public static ApiResponseBase CheckPaymentDisabilityStatus(string userId)
        {
            var status = PaymentService.GetInstance().CheckPaymentDisabilityStatus(userId) ?? true;

            return new CheckPaymentDisabilityStatusResponse
            {
                IsPaymentDisabled = status,
                StatusCode = HttpStatusCode.OK,
            };
        }

    }
}