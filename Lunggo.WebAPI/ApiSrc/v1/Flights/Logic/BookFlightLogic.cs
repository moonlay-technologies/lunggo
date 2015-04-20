using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;
using FlightService = Lunggo.ApCommon.Flight.Service.FlightService;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public partial class FlightLogic
    {
        public static FlightBookingApiResponse BookFlight(FlightBookingApiRequest request)
        {
            if (IsValid(request))
            {
                var bookServiceRequest = PreprocessServiceRequest(request);
                var bookServiceResponse = FlightService.GetInstance().BookFlight(bookServiceRequest);
                var apiResponse = AssembleApiResponse(bookServiceResponse, request);
                return apiResponse;
            }
            else
            {
                return new FlightBookingApiResponse
                {
                    Result = "Failed",
                    BookingId = null,
                    TimeLimit = DateTime.MinValue,
                    OriginalRequest = request
                };
            }
        }

        private static BookFlightInput PreprocessServiceRequest(FlightBookingApiRequest request)
        {
            var passengerInfo = request.PassengerData.Select(data => new PassengerFareInfo
            {
                Type = AssignType(data.Type),
                Title = AssignTitle(data.Title),
                Gender = AssignGender(data.Title),
                FirstName = data.FirstName,
                LastName = data.LastName,
                DateOfBirth = data.BirthDate,
                PassportOrIdNumber = data.PassportOrIdNumber,
                PassportExpiryDate = data.PassportExpiryDate,
                PassportCountry = data.Country
            });
            var bookInfo = new FlightBookingInfo
            {
                FareId = request.FareId,
                ContactData = new ContactData
                {
                    Name = request.ContactName,
                    Phone = request.ContactPhone,
                    Email = request.ContactEmail
                },
                PassengerFareInfos = passengerInfo.ToList()
            };
            var bookServiceRequest = new BookFlightInput
            {
                BookingInfo = bookInfo
                //TODO FLIGHT : Booking Payment
            };
            return null;
        }

        private static PassengerType AssignType(string type)
        {
            switch (type)
            {
                case "Adult":
                    return PassengerType.Adult;
                case "Child":
                    return PassengerType.Child;
                case "Infant":
                    return PassengerType.Infant;
                default:
                    return PassengerType.Adult;
            }
        }

        private static Title AssignTitle(string title)
        {
            switch (title)
            {
                case "Mr":
                    return Title.Mister;
                case "Mrs":
                    return Title.Mistress;
                case "Ms":
                    return Title.Miss;
                default:
                    return Title.Mister;
            }
        }

        private static Gender AssignGender(string title)
        {
            switch (title)
            {
                case "Mr":
                    return Gender.Male;
                case "Mrs":
                    return Gender.Female;
                case "Ms":
                    return Gender.Female;
                default:
                    return Gender.Male;
            }
        }

        private static bool IsValid(FlightBookingApiRequest request)
        {
            return
                request.FareId != null &&
                request.PassengerData.TrueForAll(data =>
                    (data.Title == "Mr" || data.Title == "Mrs" || data.Title == "Ms") &&
                    (data.Type == "Adult" || data.Type == "Child" || data.Type == "Infant") &&
                    (
                        (data.Type == "Adult" && GetAge(data.BirthDate) >= 12) ||
                        (data.Type == "Child" && GetAge(data.BirthDate) >= 2 && GetAge(data.BirthDate) < 12) ||
                        (data.Type == "Infant" && data.BirthDate >= DateTime.Now && GetAge(data.BirthDate) < 2)
                    ) &&
                    !string.IsNullOrEmpty(data.FirstName) && data.FirstName.Length < 50 &&
                    !string.IsNullOrEmpty(data.LastName) && data.LastName.Length < 50 &&
                    (
                        !request.PassportReq ||
                        (request.PassportReq && data.PassportExpiryDate.CompareTo(DateTime.Now.AddMonths(6)) > 0)
                    )
                );
        }

        private static int GetAge(DateTime birthDate)
        {
            if (birthDate > DateTime.Now)
                return 0;
            else
                if (DateTime.Now.DayOfYear >= birthDate.DayOfYear)
                    return DateTime.Now.Year - birthDate.Year;
                else
                    return DateTime.Now.Year - birthDate.Year - 1;
        }

        private static FlightBookingApiResponse AssembleApiResponse(BookFlightOutput bookServiceResponse, FlightBookingApiRequest request)
        {
            var apiResponse = new FlightBookingApiResponse
            {
                OriginalRequest = request
            };
            if (bookServiceResponse.IsSuccess)
            {
                apiResponse.Result = "Booked";
                apiResponse.BookingId = bookServiceResponse.BookResult.BookingId;
                apiResponse.TimeLimit = bookServiceResponse.BookResult.TimeLimit;
            }
            else
                apiResponse.Result = "Failed";
            return apiResponse;
        }
    }
}