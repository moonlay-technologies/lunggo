using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Log;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Hotel.Model;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Logic
{
    public static partial class HotelLogic
    {
        public static ApiResponseBase GetAvailableRates(HotelAvailableRateApiRequest request)
        {
            if (!IsValid(request))
                return new HotelSearchApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERHSEA01"
                };
            var availableRateRequest = PreprocessServiceRequest(request);
            var availableRateResponse = HotelService.GetInstance().GetAvailableRates(availableRateRequest);
            var apiResponse = AssembleApiResponse(availableRateResponse);
            return apiResponse;
        }
        private static bool IsValid(HotelAvailableRateApiRequest request)
        {
            if (request == null || request.Occupancies == null)
                return false;

            return
                request.Occupancies.TrueForAll(data => data.AdultCount > 0) &&
                request.Occupancies.TrueForAll(data => data.RoomCount > 0) &&
                request.Nights > 0 &&
                request.HotelCode > 0 &&
                request.CheckIn >= DateTime.UtcNow.Date;
        }

        private static AvailableRatesInput PreprocessServiceRequest(HotelAvailableRateApiRequest request)
        {
            var availableRatesRequest = new AvailableRatesInput
            {
                HotelCode = request.HotelCode,
                Nights = request.Nights,
                CheckIn = request.CheckIn,
                Checkout = request.Checkout,
                Occupancies = request.Occupancies
            };
            availableRatesRequest.Occupancies.ForEach(o => o.ChildrenAges = o.ChildrenAges.Take(o.ChildCount).ToList());
            return availableRatesRequest;
        }

        private static HotelAvailableRateApiResponse AssembleApiResponse(AvailableRatesOutput availableRatesOutput)
        {
            if (availableRatesOutput.IsSuccess)
            {
                return new HotelAvailableRateApiResponse
                {
                    Id = availableRatesOutput.Id,
                    Total = availableRatesOutput.Total,
                    Rooms =  availableRatesOutput.Rooms,
                    ExpiryTime = availableRatesOutput.ExpiryTime.TruncateMilliseconds()
                };
            }
            else
            {
                if (availableRatesOutput.Errors == null && availableRatesOutput.ErrorMessages == null)
                    return new HotelAvailableRateApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERHSEA99"
                    };

                if (availableRatesOutput.Errors != null)
                {
                    switch (availableRatesOutput.Errors[0])
                    {
                        case HotelError.TechnicalError:
                            return new HotelAvailableRateApiResponse
                            {
                                StatusCode = HttpStatusCode.InternalServerError,
                                ErrorCode = "ERHAR01"
                            };
                        default:
                            return new HotelAvailableRateApiResponse
                            {
                                StatusCode = HttpStatusCode.InternalServerError,
                                ErrorCode = "ERRGEN99"
                            };
                    }
                }
                return new HotelAvailableRateApiResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERRGEN99"
                };
            }
        }
    }

}