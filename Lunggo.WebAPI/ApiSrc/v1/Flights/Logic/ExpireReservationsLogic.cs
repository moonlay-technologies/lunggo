using System.Net;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;
using FlightService = Lunggo.ApCommon.Flight.Service.FlightService;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public static partial class FlightLogic
    {
        public static ApiResponseBase ExpireReservations()
        {
            var flight = FlightService.GetInstance();
            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}