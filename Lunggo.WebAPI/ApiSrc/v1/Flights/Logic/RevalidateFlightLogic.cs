using System.Linq;
using System.Net;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;
using FlightService = Lunggo.ApCommon.Flight.Service.FlightService;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public static partial class FlightLogic
    {
        public static FlightRevalidateApiResponse RevalidateFlight(FlightRevalidateApiRequest request)
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
                return new FlightRevalidateApiResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERFREV99"
                };
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
                if (revalidateServiceResponse.Sets == null || !revalidateServiceResponse.Sets.Any())
                {
                    return new FlightRevalidateApiResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERFREV01"
                    };
                }
                if (revalidateServiceResponse.IsValid)
                {
                    return new FlightRevalidateApiResponse
                    {
                        NewFare = revalidateServiceResponse.NewFare,
                        StatusCode = HttpStatusCode.OK
                    };
                }
                else
                {
                    if (revalidateServiceResponse.NewFare != null)
                    {
                        return new FlightRevalidateApiResponse
                        {
                            NewFare = revalidateServiceResponse.NewFare,
                            StatusCode = HttpStatusCode.OK
                        };
                    }
                    else
                    {
                        return new FlightRevalidateApiResponse
                        {
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
                    StatusCode = HttpStatusCode.Accepted,
                    ErrorCode = "ERFREV01"
                };
            }
        }
    }
}