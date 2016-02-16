using System.Net;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;
using FlightService = Lunggo.ApCommon.Flight.Service.FlightService;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public static partial class FlightLogic
    {
        public static FlightRevalidateApiResponse RevalidateFlight(FlightRevalidateApiRequest request)
        {
            var service = FlightService.GetInstance();
            var revalidateServiceRequest = PreprocessServiceRequest(request);
            var revalidateServiceResponse = service.RevalidateFlight(revalidateServiceRequest);
            var apiResponse = AssembleApiResponse(revalidateServiceResponse, request);
            return apiResponse;
        }

        private static RevalidateFlightInput PreprocessServiceRequest(FlightRevalidateApiRequest request)
        {
            return new RevalidateFlightInput
            {
                SearchId = request.SearchId,
                ItinIndices = request.ItinIndices,
                RequestId = request.SecureCode
            };
        }

        private static FlightRevalidateApiResponse AssembleApiResponse(RevalidateFlightOutput revalidateServiceResponse, FlightRevalidateApiRequest request)
        {
            if (revalidateServiceResponse.IsSuccess)
            {
                if (revalidateServiceResponse.IsValid)
                {
                    return new FlightRevalidateApiResponse
                    {
                        Token = revalidateServiceResponse.Token,
                        IsValid = true,
                        IsOtherFareAvailable = null,
                        NewFare = null,
                        StatusCode = HttpStatusCode.OK,
                        StatusMessage = "Success.",
                        OriginalRequest = request
                    };
                }
                else
                {
                    if (revalidateServiceResponse.NewFare != null)
                    {
                        return new FlightRevalidateApiResponse
                        {
                            Token = revalidateServiceResponse.Token,
                            IsValid = false,
                            IsOtherFareAvailable = true,
                            NewFare = revalidateServiceResponse.NewFare,
                            StatusCode = HttpStatusCode.OK,
                            StatusMessage = "Success.",
                            OriginalRequest = request
                        };
                    }
                    else
                    {
                        return new FlightRevalidateApiResponse
                        {
                            Token = null,
                            IsValid = false,
                            IsOtherFareAvailable = false,
                            NewFare = null,
                            StatusCode = HttpStatusCode.OK,
                            StatusMessage = "Success.",
                            OriginalRequest = request
                        };
                    }
                }
            }
            else
            {
                return new FlightRevalidateApiResponse
                {
                    Token = null,
                    IsValid = false,
                    IsOtherFareAvailable = false,
                    NewFare = 0,
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusMessage = "There is a problem in revalidating fare, please try again later.",
                    OriginalRequest = request
                };
            }
        }
    }
}