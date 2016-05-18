using System.Net;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.Flight.Model;

namespace Lunggo.WebAPI.ApiSrc.Flight.Logic
{
    public static partial class FlightLogic
    {
        public static TopDestinationsApiResponse TopDestinations()
        {
            try
            {
                var flight = FlightService.GetInstance();
                return new TopDestinationsApiResponse
                {
                    StatusCode = HttpStatusCode.OK,
                    TopDestinationList = flight.GetTopDestination()
                };
            }
            catch
            {
                return new TopDestinationsApiResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERFTOP99"
                };
            }
        }
    }
}
