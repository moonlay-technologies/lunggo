using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Lunggo.WebAPI.ApiSrc.v1.Payment.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Payment.Logic
{
    public static partial class PaymentLogic
    {
        public static PaymentApiResponse CheckPayment(string rsvNo, IPrincipal user)
        {
            try
            {
                var rsv = FlightService.GetInstance().GetReservationForDisplay(rsvNo);
                var apiResponse = AssembleApiResponse(rsv);
                return apiResponse;
            }
            catch
            {
                return new PaymentApiResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERPPAY99"
                };
            }
        }

        private static PaymentApiResponse AssembleApiResponse(FlightReservationForDisplay rsv)
        {
            return new PaymentApiResponse
            {
                PaymentStatus = rsv.Payment.Status,
                Method = rsv.Payment.Method,
                RedirectionUrl = rsv.Payment.Url,
                TransferAccount = rsv.Payment.TargetAccount,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}