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
    internal partial class MystiflyWrapper
    {
        internal override BookFlightResult BookFlight(FlightBookingInfo bookInfo)
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
                    Target = MystiflyClientHandler.Target,
                    ExtensionData = null,
                };
                var result = new BookFlightResult();
                var retry = 0;
                var done = false;
                while (!done)
                {
                    var response = client.BookFlight(request);
                    done = true;
                    if (!response.Errors.Any() && response.Success && response.Status == "CONFIRMED")
                    {
                        result = MapResult(response);
                        result.IsSuccess = true;
                    }
                    else
                    {
                        if (response.Errors.Any())
                        {
                            result.Errors = new List<FlightError>();
                            result.ErrorMessages = new List<string>();
                            foreach (var error in response.Errors)
                            {
                                if (error.Code == "ERBUK002")
                                {
                                    result.Errors = null;
                                    result.ErrorMessages = null;
                                    client.CreateSession();
                                    request.SessionId = client.SessionId;
                                    retry++;
                                    if (retry <= 3)
                                    {
                                        done = false;
                                        break;
                                    }
                                }
                                MapError(response, result);
                            }
                        }
                        result.IsSuccess = false;
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
                PassportNumber = passengerFareInfo.IdNumber,
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
                CountryCode = "",
                AreaCode = "",
                PhoneNumber = bookInfo.ContactData.Phone,
                Email = bookInfo.ContactData.Email,
                AirTravelers = airTravelers.ToArray(),
                ExtensionData = null
            };
            return travelerInfo;
        }

        private static BookFlightResult MapResult(AirBookRS response)
        {
            return new BookFlightResult
            {
                Status = new BookingStatusInfo
                {
                    BookingStatus = BookingStatus.Booked,
                    BookingId = response.UniqueID,
                    TimeLimit = response.TktTimeLimit
                }
            };
        }

        private static void MapError(AirBookRS response, ResultBase result)
        {
            foreach (var error in response.Errors)
            {
                switch (error.Code)
                {
                    case "ERBUK001":
                    case "ERBUK002":
                    case "ERBUK003":
                    case "ERBUK004":
                    case "ERBUK005":
                    case "ERBUK006":
                    case "ERBUK007":
                    case "ERBUK008":
                    case "ERBUK009":
                    case "ERBUK010":
                    case "ERBUK011":
                    case "ERBUK012":
                    case "ERBUK013":
                    case "ERBUK014":
                    case "ERBUK015":
                    case "ERBUK016":
                    case "ERBUK017":
                    case "ERBUK019":
                    case "ERBUK020":
                    case "ERBUK021":
                    case "ERBUK022":
                    case "ERBUK023":
                    case "ERBUK024":
                    case "ERBUK025":
                    case "ERBUK026":
                    case "ERBUK027":
                    case "ERBUK028":
                    case "ERBUK029":
                    case "ERBUK030":
                    case "ERBUK031":
                    case "ERBUK032":
                    case "ERBUK033":
                    case "ERBUK034":
                    case "ERBUK035":
                    case "ERBUK036":
                    case "ERBUK037":
                    case "ERBUK038":
                    case "ERBUK039":
                    case "ERBUK040":
                    case "ERBUK041":
                    case "ERBUK042":
                    case "ERBUK043":
                    case "ERBUK044":
                    case "ERBUK045":
                    case "ERBUK047":
                    case "ERBUK048":
                    case "ERBUK049":
                    case "ERBUK050":
                    case "ERBUK051":
                    case "ERBUK052":
                    case "ERBUK053":
                    case "ERBUK054":
                    case "ERBUK055":
                    case "ERBUK056":
                    case "ERBUK057":
                    case "ERBUK058":
                    case "ERBUK059":
                    case "ERBUK060":
                    case "ERBUK061":
                    case "ERBUK062":
                    case "ERBUK063":
                    case "ERBUK064":
                    case "ERBUK065":
                    case "ERBUK066":
                    case "ERBUK067":
                    case "ERBUK068":
                    case "ERBUK069":
                    case "ERBUK070":
                    case "ERBUK071":
                    case "ERBUK072":
                    case "ERBUK075":
                    case "ERBUK077":
                    case "ERBUK086":
                    case "ERBUK088":
                    case "ERBUK089":
                    case "ERBUK090":
                    case "ERBUK091":
                    case "ERBUK092":
                    case "ERBUK093":
                    case "ERBUK094":
                    case "ERBUK095":
                    case "ERBUK096":
                    case "ERBUK097":
                    case "ERBUK098":
                    case "ERBUK099":
                    case "ERBUK100":
                    case "ERBUK102":
                        goto case "InvalidInputData";
                    case "ERBUK018":
                    case "ERBUK046":
                        goto case "FareIdNoLongerValid";
                    case "ERBUK074":
                    case "ERBUK076":
                    case "ERBUK084":
                    case "ERBUK087":
                    case "ERBUK101":
                        goto case "AlreadyBooked";
                    case "ERBUK073":
                    case "ERBUK079":
                    case "ERBUK080":
                    case "ERBUK082":
                    case "ERBUK083":
                        goto case "ProcessFailed";
                    case "ERBUK078":
                        result.ErrorMessages.Add("Insufficient balance!");
                        goto case "TechnicalError";
                    case "ERBUK081":
                        result.ErrorMessages.Add("Host not responding!");
                        goto case "TechnicalError";
                    case "ERBUK085":
                        result.ErrorMessages.Add("Host not responding!");
                        goto case "TechnicalError";
                    case "ERGEN003":
                        result.ErrorMessages.Add("Unexpected error on the other end!");
                        goto case "TechnicalError";
                    case "ERMAI001":
                        result.ErrorMessages.Add("Mystifly is under maintenance!");
                        goto case "TechnicalError";

                    case "InvalidInputData":
                        result.Errors.Add(FlightError.InvalidInputData);
                        break;
                    case "FareIdNoLongerValid":
                        result.Errors.Add(FlightError.FareIdNoLongerValid);
                        break;
                    case "AlreadyBooked":
                        result.Errors.Add(FlightError.AlreadyBooked);
                        break;
                    case "ProcessFailed":
                        result.Errors.Add(FlightError.ProcessFailed);
                        break;
                    case "TechnicalError":
                        result.Errors.Add(FlightError.TechnicalError);
                        break;
                }
            }
        }
    }
}
