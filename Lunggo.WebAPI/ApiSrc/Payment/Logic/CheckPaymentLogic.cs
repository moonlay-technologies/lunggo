using System.Net;
using System.Security.Principal;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Payment.Model;

namespace Lunggo.WebAPI.ApiSrc.Payment.Logic
{
    public static partial class PaymentLogic
    {
        public static ApiResponseBase CheckPayment(string rsvNo, IPrincipal user)
        {
            var rsv = FlightService.GetInstance().GetReservationForDisplay(rsvNo);
            var apiResponse = AssembleApiResponse(rsv);
            return apiResponse;
        }

        private static PaymentApiResponse AssembleApiResponse(FlightReservationForDisplay rsv)
        {
            return new PaymentApiResponse
            {
                PaymentStatus = rsv.Payment.Status,
                Method = rsv.Payment.Method,
                RedirectionUrl = rsv.Payment.RedirectionUrl,
                TransferAccount = rsv.Payment.TransferAccount,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}