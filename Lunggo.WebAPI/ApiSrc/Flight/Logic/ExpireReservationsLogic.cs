using System.Net;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.Common.Model;

namespace Lunggo.WebAPI.ApiSrc.Flight.Logic
{
    public  partial class FlightLogic
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