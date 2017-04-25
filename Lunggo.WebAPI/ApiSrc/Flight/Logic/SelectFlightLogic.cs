using System;
using System.Linq;
using System.Net;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Extension;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Flight.Model;

namespace Lunggo.WebAPI.ApiSrc.Flight.Logic
{
    public partial class FlightLogic
    {
        public static ApiResponseBase SelectFlight(FlightSelectApiRequest request)
        {
            if (IsValid(request))
            {
                var service = FlightService.GetInstance();
                var selectServiceRequest = PreprocessServiceRequest(request);
                var selectServiceResponse = service.SelectFlight(selectServiceRequest);
                var expiryTime = FlightService.GetInstance().GetItineraryExpiry(selectServiceResponse.Token);
                var apiResponse = AssembleApiResponse(selectServiceResponse, expiryTime);
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
                RegisterNumbers = request.RegisterNumbers,
                EnableCombo = request.EnableCombo
            };
        }

        private static FlightSelectApiResponse AssembleApiResponse(SelectFlightOutput selectServiceResponse, DateTime? expiryTime)
        {
            if (selectServiceResponse.IsSuccess)
            {
                if (selectServiceResponse.Token != null)
                    return new FlightSelectApiResponse
                    {
                        Token = selectServiceResponse.Token,
                        ExpiryTime = expiryTime.TruncateMilliseconds(),
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