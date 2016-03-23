using System.Net;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;
using FlightService = Lunggo.ApCommon.Flight.Service.FlightService;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public static partial class FlightLogic
    {
        public static FlightSelectApiResponse SelectFlight(FlightSelectApiRequest request)
        {
            var service = FlightService.GetInstance();
            var selectServiceRequest = PreprocessServiceRequest(request);
            var selectServiceResponse = service.SelectFlight(selectServiceRequest);
            var apiResponse = AssembleApiResponse(selectServiceResponse);
            return apiResponse;
        }

        private static SelectFlightInput PreprocessServiceRequest(FlightSelectApiRequest request)
        {
            return new SelectFlightInput
            {
                SearchId = request.SearchId,
                RegisterNumbers = request.RegisterNumbers
            };
        }

        private static FlightSelectApiResponse AssembleApiResponse(SelectFlightOutput selectServiceResponse)
        {
            if (selectServiceResponse.IsSuccess)
            {
                return new FlightSelectApiResponse
                {
                    Token = selectServiceResponse.Token,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new FlightSelectApiResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERFSEL01"
                };
            }
        }
    }
}