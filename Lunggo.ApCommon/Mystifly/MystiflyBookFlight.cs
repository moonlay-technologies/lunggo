using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Interface;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using Gender = Lunggo.ApCommon.Flight.Constant.Gender;
using PassengerType = Lunggo.ApCommon.Flight.Constant.PassengerType;

namespace Lunggo.ApCommon.Mystifly
{
    public partial class MystiflyWrapper : IBookFlight
    {
        public BookFlightResult BookFlight(FlightBookingInfo bookInfo)
        {
            var airTravelers = bookInfo.PassengerFareInfos.Select(MapAirTraveler).ToList();
            var travelerInfo = MapTravelerInfo(bookInfo, airTravelers);
            using (var client = new MystiflyClientHandler())
            {
                var request = new AirBookRQ
                {
                    FareSourceCode = bookInfo.FareId,
                    TravelerInfo = travelerInfo,
                    ClientMarkup = 0,
                    PaymentTransactionID = null,
                    PaymentCardInfo = null,
                    SessionId = client.SessionId,
                    Target = client.Target,
                    ExtensionData = null,
                };
                var result = new BookFlightResult();
                var retry = 0;
                var done = false;
                while (retry < 3 && !done)
                {
                    var response = client.BookFlight(request);
                    done = true;
                    if (!response.Errors.Any())
                    {
                        result = MapResult(response);
                        result.Success = true;
                    }
                    else
                    {
                        foreach (var error in response.Errors)
                        {
                            if (error.Code == "ERBUK002")
                            {
                                result.Errors.Clear();
                                client.CreateSession();
                                request.SessionId = client.SessionId;
                                retry++;
                                done = false;
                                break;
                            }
                            result.Errors.Add(MapError(error));
                            result.Success = false;
                        }
                    }
                    
                }
                return result;
            }
        }

        private static AirTraveler MapAirTraveler(PassengerFareInfo passengerFareInfo)
        {
            var airTraveler = new AirTraveler
            {
                PassengerName = MapPassengerName(passengerFareInfo),
                PassengerType = MapPassengerType(passengerFareInfo),
                Gender = MapGender(passengerFareInfo),
                Passport = MapPassport(passengerFareInfo),
                DateOfBirth = passengerFareInfo.DateOfBirth,
                ExtraServices = null,
                ExtraServices1_1 = null,
                SpecialServiceRequest = null,
                FrequentFlyerNumber = null,
                ResidentFareDocumentType = null,
                ExtensionData = null
            };
            return airTraveler;
        }

        private static OnePointService.Flight.PassengerType MapPassengerType(PassengerFareInfo passengerFareInfo)
        {
            switch (passengerFareInfo.Type)
            {
                case PassengerType.Adult :
                    return OnePointService.Flight.PassengerType.ADT;
                case PassengerType.Child :
                    return OnePointService.Flight.PassengerType.CHD;
                case PassengerType.Infant :
                    return OnePointService.Flight.PassengerType.INF;
                default :
                    return OnePointService.Flight.PassengerType.Default;
            }
        }

        private static OnePointService.Flight.Gender MapGender(PassengerFareInfo passengerFareInfo)
        {
            switch (passengerFareInfo.Gender)
            {
                case Gender.Male :
                    return OnePointService.Flight.Gender.M;
                case Gender.Female :
                    return OnePointService.Flight.Gender.F;
                default :
                    return OnePointService.Flight.Gender.Default;
            }
        }

        private static PassengerName MapPassengerName(PassengerFareInfo passengerFareInfo)
        {
            var passengerName = new PassengerName
            {
                PassengerTitle = MapPassengerTitle(passengerFareInfo),
                PassengerFirstName = passengerFareInfo.FirstName,
                PassengerLastName = passengerFareInfo.LastName,
                ExtensionData = null
            };
            return passengerName;
        }

        private static PassengerTitle MapPassengerTitle(PassengerFareInfo passengerFareInfo)
        {
            switch (passengerFareInfo.Title)
            {
                case Title.Mister :
                    return PassengerTitle.MR;
                case Title.Mistress :
                    return PassengerTitle.MRS;
                case Title.Miss :
                    return PassengerTitle.MS;
                default :
                    return PassengerTitle.Default;
            }
        }

        private static Passport MapPassport(PassengerFareInfo passengerFareInfo)
        {
            var passport = new Passport
            {
                PassportNumber = passengerFareInfo.PassportOrIdNumber,
                ExpiryDate = passengerFareInfo.PassportExpiryDate.GetValueOrDefault(),
                Country = passengerFareInfo.PassportCountry,
                ExtensionData = null
            };
            return passport;
        }

        private static TravelerInfo MapTravelerInfo(FlightBookingInfo bookInfo, List<AirTraveler> airTravelers)
        {
            var travelerInfo = new TravelerInfo
            {
                CountryCode = "0",
                AreaCode = "0",
                PhoneNumber = bookInfo.ContactPhone,
                Email = bookInfo.ContactEmail,
                AirTravelers = airTravelers.ToArray(),
                ExtensionData = null
            };
            return travelerInfo;
        }

        private static BookFlightResult MapResult(AirBookRS response)
        {
            return new BookFlightResult
            {
                BookingStatus = MapBookingStatus(response),
                IsBookSuccess = response.Success,
                BookingId = response.UniqueID,
                TimeLimit = response.TktTimeLimit
            };
        }

        private static BookingStatus MapBookingStatus(AirBookRS response)
        {
            switch (response.Status)
            {
                case "Confirmed" :
                    return BookingStatus.Confirmed;
                case "Pending" :
                    return BookingStatus.Pending;
                default :
                    return BookingStatus.Pending;
            }
        }
    }
}
