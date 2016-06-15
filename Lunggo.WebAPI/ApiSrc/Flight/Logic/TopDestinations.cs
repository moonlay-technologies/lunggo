using System.Net;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Flight.Model;

namespace Lunggo.WebAPI.ApiSrc.Flight.Logic
{
    public static partial class FlightLogic
    {
        public static ApiResponseBase TopDestinations()
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
                return ApiResponseBase.Error500();
            }
        }
    }
}
