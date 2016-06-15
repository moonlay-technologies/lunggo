using System.Net;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Flight.Model;

namespace Lunggo.WebAPI.ApiSrc.Flight.Logic
{
    public static partial class FlightLogic
    {
        public static ApiResponseBase RevalidateFlight(FlightRevalidateApiRequest request)
        {
            try
            {
                if (IsValid(request))
                {
                    var service = FlightService.GetInstance();
                    var revalidateServiceRequest = PreprocessServiceRequest(request);
                    var revalidateServiceResponse = service.RevalidateFlight(revalidateServiceRequest);
                    var apiResponse = AssembleApiResponse(revalidateServiceResponse);
                    return apiResponse;
                }
                else
                {
                    return new FlightRevalidateApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERFREV01"
                    };
                }
            }
            catch
            {
                return ApiResponseBase.Error500();
            }
        }

        private static bool IsValid(FlightRevalidateApiRequest request)
        {
            return
                request != null &&
                request.Token != null;
        }

        private static RevalidateFlightInput PreprocessServiceRequest(FlightRevalidateApiRequest request)
        {
            return new RevalidateFlightInput
            {
                Token = request.Token
            };
        }

        private static FlightRevalidateApiResponse AssembleApiResponse(RevalidateFlightOutput revalidateServiceResponse)
        {
            if (revalidateServiceResponse.IsSuccess)
            {
                return new FlightRevalidateApiResponse
                {
                    IsValid = revalidateServiceResponse.IsValid,
                    IsItineraryChanged = revalidateServiceResponse.IsItineraryChanged,
                    IsPriceChanged = revalidateServiceResponse.IsPriceChanged,
                    NewItinerary = revalidateServiceResponse.NewItinerary,
                    NewPrice = revalidateServiceResponse.NewPrice,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                return new FlightRevalidateApiResponse
                {
                    StatusCode = HttpStatusCode.Accepted,
                    ErrorCode = "ERFREV01"
                };
            }
        }
    }
}