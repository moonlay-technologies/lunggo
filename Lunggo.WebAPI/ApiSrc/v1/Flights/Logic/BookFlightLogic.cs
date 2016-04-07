using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.Framework.Context;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public partial class FlightLogic
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
                    IsSuccess = false,
                    OriginalRequest = request
                };
            }
        }

        private static bool IsValid(FlightBookApiRequest request)
        {
            return
                request != null &&
                request.Token != null &&
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
                    IsSuccess = true,
                    RsvNo = bookServiceResponse.RsvNo,
                    OriginalRequest = request
                };
            else
            {
                if (bookServiceResponse.IsPriceChanged)
                {
                    return new FlightBookApiResponse
                    {
                        IsSuccess = false,
                        NewPrice = bookServiceResponse.NewPrice,
                        OriginalRequest = request
                    };
                }
                else
                {
                    return new FlightBookApiResponse
                    {
                        IsSuccess = false,
                        Error = bookServiceResponse.Errors[0],
                        OriginalRequest = request
                    };
                }
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
                DiscountCode = request.DiscountCode,
                TransferToken = request.TransferToken
            };
            return bookServiceRequest;
        }

        private static List<FlightPassenger> MapPassengers(IEnumerable<PassengerData> passengers)
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