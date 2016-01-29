using System.Net;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;
using FlightService = Lunggo.ApCommon.Flight.Service.FlightService;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public static partial class FlightLogic
    {
        public static ExpireReservationsApiResponse ExpireReservations()
        {
            var flight = FlightService.GetInstance();
            return new ExpireReservationsApiResponse()
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Success."
            };
        }
    }
}