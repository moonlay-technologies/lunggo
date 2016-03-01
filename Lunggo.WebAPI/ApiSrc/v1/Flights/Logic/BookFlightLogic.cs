using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.Framework.Context;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public static partial class FlightLogic
    {
        public static FlightBookApiResponse BookFlight(FlightBookApiRequest request)
        {
            if (IsValid(request))
            {
                OnlineContext.SetActiveLanguageCode(request.Language);
                var bookServiceRequest = PreprocessServiceRequest(request);
                var bookServiceResponse = FlightService.GetInstance().BookFlight(bookServiceRequest);
                var apiResponse = AssembleApiResponse(bookServiceResponse, request);
                return apiResponse;
            }
            else
            {
                return new FlightBookApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "There is an error on your submitted data.",
                    OriginalRequest = request
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
                request.Contact.CountryCode != null &&
                request.Passengers != null &&
                request.Passengers.TrueForAll(p => p.FirstName != null) &&
                request.Passengers.TrueForAll(p => p.LastName != null) &&
                request.Passengers.TrueForAll(p => p.Title != Title.Undefined) &&
                request.Passengers.TrueForAll(p => p.Type != PassengerType.Undefined) &&
                request.Payment != null &&
                request.Payment.Method != PaymentMethod.Undefined;
        }

        private static FlightBookApiResponse AssembleApiResponse(BookFlightOutput bookServiceResponse, FlightBookApiRequest request)
        {
            if (bookServiceResponse.IsSuccess)
                return new FlightBookApiResponse
                {
                    RsvNo = bookServiceResponse.RsvNo,
                    StatusCode = HttpStatusCode.OK,
                    StatusMessage = "Success, all fares booked.",
                    OriginalRequest = request
                };
            else
                switch (bookServiceResponse.Errors[0])
                {
                    case FlightError.AlreadyBooked:
                        return new FlightBookApiResponse
                        {
                            StatusCode = HttpStatusCode.Accepted,
                            StatusMessage = "This reservation is already booked, please make another reservation.",
                                OriginalRequest = request
                        };
                    case FlightError.BookingIdNoLongerValid:
                    case FlightError.FareIdNoLongerValid:
                        return new FlightBookApiResponse
                        {
                            StatusCode = HttpStatusCode.Accepted,
                            StatusMessage = "This reservation is already expired, please make another reservation.",
                            OriginalRequest = request
                        };
                    case FlightError.FailedOnSupplier:
                        return new FlightBookApiResponse
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            StatusMessage = "There is an error when processing this flight to the supplier.",
                            OriginalRequest = request
                        };
                    case FlightError.InvalidInputData:
                        return new FlightBookApiResponse
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            StatusMessage = "There is an error on your submitted data.",
                            OriginalRequest = request
                        };
                    case FlightError.PartialSuccess:
                        return new FlightBookApiResponse
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            StatusMessage = "Only some of the fares are success.",
                            OriginalRequest = request
                        };
                    case FlightError.TechnicalError:
                        return new FlightBookApiResponse
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            StatusMessage = "There is an error occured, please try again later.",
                            OriginalRequest = request
                        };
                    default:
                        return new FlightBookApiResponse
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            StatusMessage = "There is an error occured, please try again later.",
                            OriginalRequest = request
                        };
                }
        }

        private static BookFlightInput PreprocessServiceRequest(FlightBookApiRequest request)
        {
            request.Payment.Medium = request.Payment.Method == PaymentMethod.BankTransfer
                ? PaymentMedium.Direct
                : PaymentMedium.Veritrans;

            var bookServiceRequest = new BookFlightInput
            {
                ItinCacheId = request.Token,
                Contact = request.Contact,
                Passengers = MapPassengers(request.Passengers),
                Payment = request.Payment,
                DiscountCode = request.DiscountCode
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
                PassportCountry = passenger.Country
            }).ToList();
        }
    }
}