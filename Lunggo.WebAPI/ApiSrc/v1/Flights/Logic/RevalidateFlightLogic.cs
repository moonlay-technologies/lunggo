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
                Token = request.Token
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
                        IsValid = true,
                        IsOtherFareAvailable = null,
                        NewFare = null,
                        StatusCode = HttpStatusCode.OK
                    };
                }
                else
                {
                    if (revalidateServiceResponse.NewFare != null)
                    {
                        return new FlightRevalidateApiResponse
                        {
                            IsValid = false,
                            IsOtherFareAvailable = true,
                            NewFare = revalidateServiceResponse.NewFare,
                            StatusCode = HttpStatusCode.OK
                        };
                    }
                    else
                    {
                        return new FlightRevalidateApiResponse
                        {
                            IsValid = false,
                            IsOtherFareAvailable = false,
                            NewFare = null,
                            StatusCode = HttpStatusCode.OK
                        };
                    }
                }
            }
            else
            {
                return new FlightRevalidateApiResponse
                {
                    IsValid = false,
                    IsOtherFareAvailable = false,
                    NewFare = 0,
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERFREV01"
                };
            }
        }
    }
}