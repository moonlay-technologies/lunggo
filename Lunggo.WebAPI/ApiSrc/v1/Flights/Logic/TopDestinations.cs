using System.Net;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public static partial class FlightLogic
    {
        public static TopDestinationsApiResponse TopDestinations()
        {
            var flight = FlightService.GetInstance();
            return new TopDestinationsApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Success.",
                TopDestinationList = flight.GetTopDestination()
            };
        }
    }
}
