using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public partial class FlightLogic
    {
        public static FlightPaymentApiResponse SetPaymentFlight(FlightPaymentApiRequest request)
        {
            var payServiceRequest = PreprocessServiceRequest(request);
            var payServiceResponse = FlightService.GetInstance().SetPayment(payServiceRequest);
            var apiResponse = AssembleApiResponse(payServiceResponse, request);
            return apiResponse;
        }

        private static PayFlightInput PreprocessServiceRequest(FlightPaymentApiRequest request)
        {
            request.Payment.Medium = request.Payment.Method == PaymentMethod.BankTransfer
                ? PaymentMedium.Direct
                : PaymentMedium.Veritrans;

            var payServiceRequest = new PayFlightInput
            {
                RsvNo = request.RsvNo,
                Payment = request.Payment
            };
            return payServiceRequest;
        }

        private static FlightPaymentApiResponse AssembleApiResponse(PayFlightOutput payServiceResponse, FlightPaymentApiRequest request)
        {
            if (payServiceResponse.IsSuccess)
                return new FlightPaymentApiResponse
                {
                    IsSuccess = true,
                    RsvNo = payServiceResponse.RsvNo,
                    OriginalRequest = request
                };
            else
            {
                return new FlightPaymentApiResponse
                {
                    IsSuccess = false,
                    Error = FlightError.TechnicalError,
                    OriginalRequest = request
                };
            }
        }
    }
    
}