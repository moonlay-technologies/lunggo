using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public partial class FlightLogic
    {
        public static FlightCancelApiResponse CancelBooking(FlightCancelApiRequest request)
        {
            if (IsValid(request))
            {
                var cancelServiceRequest = PreprocessServiceRequest(request);
                var cancelServiceResponse = FlightService.GetInstance().CancelBooking(cancelServiceRequest);
                var apiResponse = AssembleApiResponse(cancelServiceResponse, request);
                return apiResponse;
            }
            else
            {
                return new FlightCancelApiResponse
                {
                    Result = "Failed",
                    BookingId = null,
                    OriginalRequest = request
                };
            }
        }

        private static CancelBookingInput PreprocessServiceRequest(FlightCancelApiRequest request)
        {
            return new CancelBookingInput
            {
                BookingId = request.BookingId
            };
        }

        private static FlightCancelApiResponse AssembleApiResponse(CancelBookingOutput cancelServiceResponse, FlightCancelApiRequest request)
        {
            if (cancelServiceResponse.IsSuccess && cancelServiceResponse.IsCancelSuccess)
            {
                return new FlightCancelApiResponse
                {
                    Result = "Success",
                    BookingId = cancelServiceResponse.BookingId,
                    OriginalRequest = request
                };
            }
            else
            {
                return new FlightCancelApiResponse
                {
                    Result = "Failed",
                    BookingId = null,
                    OriginalRequest = request
                };
            }
        }

        private static bool IsValid(FlightCancelApiRequest request)
        {
            return
                request.BookingId != null;
        }
    }
}