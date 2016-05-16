using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Principal;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.ProductBase.Constant;
using Lunggo.Framework.Context;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public static partial class FlightLogic
    {
        public static FlightBookApiResponse BookFlight(FlightBookApiRequest request)
        {
            try
            {
            if (IsValid(request))
            {
                OnlineContext.SetActiveLanguageCode(request.Language);
                var bookServiceRequest = PreprocessServiceRequest(request);
                var bookServiceResponse = FlightService.GetInstance().BookFlight(bookServiceRequest);
                    var apiResponse = AssembleApiResponse(bookServiceResponse);
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
            catch
            {
                return new FlightBookApiResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERFBOO99"
                };
            }
        }

        private static bool IsValid(FlightBookApiRequest request)
        {
            return
                request.Contact != null &&
                request.Contact.Name != null &&
                request.Contact.Phone != null &&
                request.Contact.Email != null &&
                request.Contact.CountryCallingCode != null &&
                request.Passengers != null &&
                request.Passengers.TrueForAll(p => p.FirstName != null) &&
                request.Passengers.TrueForAll(p => p.LastName != null) &&
                request.Passengers.TrueForAll(p => p.Title != Title.Undefined) &&
                request.Passengers.TrueForAll(p => p.Type != PassengerType.Undefined);
        }

        private static FlightBookApiResponse AssembleApiResponse(BookFlightOutput bookServiceResponse)
        {
            if (bookServiceResponse.IsSuccess)
                return new FlightBookApiResponse
                {
                    RsvNo = bookServiceResponse.RsvNo,
                    PaymentUrl = bookServiceResponse.PaymentUrl,
                    TimeLimit = bookServiceResponse.TimeLimit,
                    StatusCode = HttpStatusCode.OK
                };
            else if (!bookServiceResponse.Errors.Any())
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
                        return new FlightBookApiResponse
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            ErrorCode = "ERFBOO04"
                        };
                    case FlightError.TechnicalError:
                        return new FlightBookApiResponse
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            ErrorCode = "ERFBOO99"
                    };
                    default:
                        return new FlightBookApiResponse
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            ErrorCode = "ERFBOO99"
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
                Passengers = MapPassengers(request.Passengers)
            };
            return bookServiceRequest;
        }

        private static List<FlightPassenger> MapPassengers(IEnumerable<Passenger> passengers)
        {
            return passengers.Select(passenger => new FlightPassenger
            {
                Type = passenger.Type,
                Title = passenger.Title,
                FirstName = passenger.FirstName,
                LastName = passenger.LastName,
                Gender = passenger.Title == Title.Mister ? Gender.Male : Gender.Female,
                DateOfBirth = passenger.BirthDate,
                PassportNumber = passenger.PassportNumber,
                PassportExpiryDate = passenger.PassportExpiryDate,
                PassportCountry = passenger.PassportCountry,
                Nationality = passenger.Nationality
            }).ToList();
        }
    }
}