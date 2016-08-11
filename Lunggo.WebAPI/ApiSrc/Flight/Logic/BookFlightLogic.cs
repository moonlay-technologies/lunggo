using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Context;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Flight.Model;

namespace Lunggo.WebAPI.ApiSrc.Flight.Logic
{
    public static partial class FlightLogic
    {
        public static ApiResponseBase BookFlight(FlightBookApiRequest request)
        {
            if (IsValid(request))
            {
                OnlineContext.SetActiveLanguageCode(request.LanguageCode);
                var bookServiceRequest = PreprocessServiceRequest(request);
                var bookServiceResponse = FlightService.GetInstance().BookFlight(bookServiceRequest);
                var apiResponse = AssembleApiResponse(bookServiceResponse, request.Test);
                return apiResponse;
            }
            else
            {
                return new FlightBookApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERFBOO01"
                };
            }
        }

        private static bool IsValid(FlightBookApiRequest request)
        {
            return
                request != null &&
                request.LanguageCode != null &&
                request.Contact != null &&
                request.Contact.Title != Title.Undefined &&
                request.Contact.Name != null &&
                request.Contact.Phone != null &&
                request.Contact.Email != null &&
                request.Contact.CountryCallingCode != null &&
                request.Passengers != null &&
                request.Passengers.TrueForAll(p => p.Name != null) &&
                request.Passengers.TrueForAll(p => p.Title != Title.Undefined) &&
                request.Passengers.TrueForAll(p => p.Type != PaxType.Undefined);
        }

        private static FlightBookApiResponse AssembleApiResponse(BookFlightOutput bookServiceResponse, bool test)
        {
            if (bookServiceResponse.IsSuccess)
            {
                if (!test)
                {
                    return new FlightBookApiResponse
                    {
                        RsvNo = bookServiceResponse.RsvNo,
                        TimeLimit = bookServiceResponse.TimeLimit,
                        StatusCode = HttpStatusCode.OK
                    };
                }
                else
                {
                    return new FlightBookApiResponse
                    {
                        IsValid = true,
                        IsItineraryChanged = false,
                        IsPriceChanged = true,
                        NewPrice = bookServiceResponse.NewPrice,
                        StatusCode = HttpStatusCode.OK
                    };
                }
            }
            else if (bookServiceResponse.Errors == null || !bookServiceResponse.Errors.Any())
            {
                return new FlightBookApiResponse
                {
                    IsValid = bookServiceResponse.IsValid,
                    IsItineraryChanged = bookServiceResponse.IsItineraryChanged,
                    IsPriceChanged = bookServiceResponse.IsPriceChanged,
                    NewItinerary = bookServiceResponse.NewItinerary,
                    NewPrice = bookServiceResponse.NewPrice,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                if (bookServiceResponse.Errors[0] == FlightError.PartialSuccess)
                    bookServiceResponse.Errors.RemoveAt(0);
                switch (bookServiceResponse.Errors[0])
                {
                    case FlightError.InvalidInputData:
                        return new FlightBookApiResponse
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            ErrorCode = "ERFBOO01"
                        };
                    case FlightError.AlreadyBooked:
                        return new FlightBookApiResponse
                        {
                            StatusCode = HttpStatusCode.Accepted,
                            ErrorCode = "ERFBOO02"
                        };
                    case FlightError.BookingIdNoLongerValid:
                    case FlightError.FareIdNoLongerValid:
                        return new FlightBookApiResponse
                        {
                            StatusCode = HttpStatusCode.Accepted,
                            ErrorCode = "ERFBOO03"
                        };
                    case FlightError.FailedOnSupplier:
                    case FlightError.TechnicalError:
                    default:
                        return new FlightBookApiResponse
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            ErrorCode = "ERFBOO04"
                        };
                }
            }
        }

        private static BookFlightInput PreprocessServiceRequest(FlightBookApiRequest request)
        {
            var bookServiceRequest = new BookFlightInput
            {
                Token = request.Token,
                Contact = request.Contact,
                Passengers = FlightService.GetInstance().ConvertToPax(request.Passengers)
            };
            return bookServiceRequest;
        }
    }
}