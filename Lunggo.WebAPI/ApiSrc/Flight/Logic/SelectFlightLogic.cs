using System.Linq;
using System.Net;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Flight.Model;

namespace Lunggo.WebAPI.ApiSrc.Flight.Logic
{
    public static partial class FlightLogic
    {
        public static ApiResponseBase SelectFlight(FlightSelectApiRequest request)
        {
            try
            {
                if (IsValid(request))
                {
                    var service = FlightService.GetInstance();
                    var selectServiceRequest = PreprocessServiceRequest(request);
                    var selectServiceResponse = service.SelectFlight(selectServiceRequest);
                    var apiResponse = AssembleApiResponse(selectServiceResponse);
                    return apiResponse;
                }
                else
                {
                    return new FlightSelectApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERFSEL01"
                    };
                }
            }
            catch
            {
                return ApiResponseBase.Error500();
            }
        }

        private static bool IsValid(FlightSelectApiRequest request)
        {
            return
                request != null &&
                FlightService.GetInstance().IsSearchIdValid(request.SearchId) &&
                request.RegisterNumbers != null &&
                request.RegisterNumbers.Any();
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
                if (selectServiceResponse.Token != null)
                    return new FlightSelectApiResponse
                    {
                        Token = selectServiceResponse.Token,
                        StatusCode = HttpStatusCode.OK
                    };
                else
                    return new FlightSelectApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERFSEL01"
                    };
            }
            else
            {
                return new FlightSelectApiResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERRGEN99"
                };
            }
        }
    }
}