using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;
using Microsoft.WindowsAzure.Storage;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public partial class FlightLogic
    {
        public static FlightDetailsApiResponse GetTripDetails(FlightDetailsApiRequest request)
        {
            if (IsValid(request))
            {
                var getDetailsServiceRequest = PreprocessServiceRequest(request);
                var getDetailsServiceResponse = FlightService.GetInstance().GetDetails(getDetailsServiceRequest);
                var apiResponse = AssembleApiResponse(getDetailsServiceResponse, request);
                return apiResponse;
            }
            else
            {
                return new FlightDetailsApiResponse
                {
                    BookingId = null,
                    FlightSegmentCount = 0,
                    TripDetails = null,
                    BookingNotes = null,
                    OriginalRequest = request
                };
            }
        }

        private static GetDetailsInput PreprocessServiceRequest(FlightDetailsApiRequest request)
        {
            return new GetDetailsInput { BookingId = request.BookingId };
        }

        private static FlightDetailsApiResponse AssembleApiResponse(GetDetailsOutput getDetailsServiceResponse, FlightDetailsApiRequest request)
        {
            if (getDetailsServiceResponse.IsSuccess)
            {
                return new FlightDetailsApiResponse
                {
                    BookingId = getDetailsServiceResponse.FlightDetails.BookingId,
                    FlightSegmentCount = getDetailsServiceResponse.FlightDetails.FlightSegmentCount,
                    TripDetails = getDetailsServiceResponse.FlightDetails.FlightItineraryDetails,
                    BookingNotes = getDetailsServiceResponse.FlightDetails.BookingNotes,
                    OriginalRequest = request
                };
            }
            else
            {
                return new FlightDetailsApiResponse
                {
                    BookingId = null,
                    FlightSegmentCount = 0,
                    TripDetails = null,
                    BookingNotes = null,
                    OriginalRequest = request
                };
            }
        }

        private static bool IsValid(FlightDetailsApiRequest request)
        {
            return
                request.BookingId != null;
        }
    }
}