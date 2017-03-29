using System.Net;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Payment.Model;

namespace Lunggo.WebAPI.ApiSrc.Payment.Logic
{
    public static partial class PaymentLogic
    {
        public static ApiResponseBase SetPaymentDisabilityStatus(string userId, bool status)
        {
            PaymentService.GetInstance().SetPaymentDisabilityStatus(userId, status);

            return new CheckPaymentDisabilityStatusResponse
            {
                StatusCode = HttpStatusCode.OK,
            };
        }

    }
}