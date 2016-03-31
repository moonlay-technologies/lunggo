using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public static partial class FlightLogic
    {
        public static FlightIssuanceApiResponse CheckFlightIssuance(FlightIssuanceApiRequest request)
        {
            try
            {
                if (IsValid(request))
                {
                    var bookingStatus = FlightService.GetInstance().GetAllBookingStatus(request.RsvNo);
                    var apiResponse = AssembleApiResponse(bookingStatus);
                    return apiResponse;
                }
                else
                {
                    return new FlightIssuanceApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERFCHI01"
                    };
                }
            }
            catch
            {
                return new FlightIssuanceApiResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERFCHI99"
                };
            }
        }

        private static bool IsValid(FlightIssuanceApiRequest request)
        {
            return 
                request != null &&
                request.RsvNo != null;
        }

        private static FlightIssuanceApiResponse AssembleApiResponse(List<Tuple<FlightTripForDisplay, BookingStatus>> list)
        {
            if (list == null)
                return new FlightIssuanceApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERFCHI02"
                };
            return new FlightIssuanceApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                TripIssuances = list.Select(e => new TripIssuance
                {
                    Origin = e.Item1.OriginAirport,
                    Destination = e.Item1.DestinationAirport,
                    Date = e.Item1.DepartureDate,
                    Status = MapBookingStatus(e.Item2)
                }).ToList()
            };
        }

        private static int MapBookingStatus(BookingStatus status)
        {
            switch (status)
            {
                case BookingStatus.Ticketing:
                    return 1;
                case BookingStatus.Ticketed:
                case BookingStatus.ScheduleChanged:
                    return 2;
                case BookingStatus.Failed:
                    return 3;
                default:
                    return 0;
            }
        }
    }
}