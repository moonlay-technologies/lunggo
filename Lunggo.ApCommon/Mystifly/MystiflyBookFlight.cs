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
            var airTravelers = bookInfo.PassengerInfoFares.Select(MapAirTraveler).ToList();
            var travelerInfo = MapTravelerInfo(bookInfo, airTravelers);
            var request = new AirBookRQ
            {
                FareSourceCode = bookInfo.FareId,
                TravelerInfo = travelerInfo,
                ClientMarkup = 0,
                PaymentTransactionID = null,
                PaymentCardInfo = null,
                SessionId = Client.SessionId,
                Target = MystiflyClientHandler.Target,
                ExtensionData = null,
            };
            var result = new BookFlightResult();
            var retry = 0;
            var done = false;
            while (!done)
            {
                var response = Client.BookFlight(request);
                done = true;
                if (response.Success && !response.Errors.Any() && response.Status == "CONFIRMED")
                {
                    result = MapResult(response);
                    result.IsSuccess = true;
                    result.Errors = null;
                    result.ErrorMessages = null;
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
                                Client.CreateSession();
                                request.SessionId = Client.SessionId;
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

        private static AirTraveler MapAirTraveler(PassengerInfoFare passengerInfoFare)
        {
            var airTraveler = new AirTraveler
            {
                PassengerName = MapPassengerName(passengerInfoFare),
                PassengerType = MapPassengerType(passengerInfoFare),
                Gender = MapGender(passengerInfoFare),
                Passport = MapPassport(passengerInfoFare),
                DateOfBirth = passengerInfoFare.DateOfBirth,
                ExtraServices = null,
                ExtraServices1_1 = null,
                SpecialServiceRequest = null,
                FrequentFlyerNumber = null,
                ResidentFareDocumentType = null,
                ExtensionData = null
            };
            return airTraveler;
        }

        private static OnePointService.Flight.PassengerType MapPassengerType(PassengerInfoFare passengerInfoFare)
        {
            switch (passengerInfoFare.Type)
            {
                case PassengerType.Adult:
                    return OnePointService.Flight.PassengerType.ADT;
                case PassengerType.Child:
                    return OnePointService.Flight.PassengerType.CHD;
                case PassengerType.Infant:
                    return OnePointService.Flight.PassengerType.INF;
                default:
                    return OnePointService.Flight.PassengerType.Default;
            }
        }

        private static OnePointService.Flight.Gender MapGender(PassengerInfoFare passengerInfoFare)
        {
            switch (passengerInfoFare.Gender)
            {
                case Gender.Male:
                    return OnePointService.Flight.Gender.M;
                case Gender.Female:
                    return OnePointService.Flight.Gender.F;
                default:
                    return OnePointService.Flight.Gender.Default;
            }
        }

        private static PassengerName MapPassengerName(PassengerInfoFare passengerInfoFare)
        {
            var passengerName = new PassengerName
            {
                PassengerTitle = MapPassengerTitle(passengerInfoFare),
                PassengerFirstName = passengerInfoFare.FirstName,
                PassengerLastName = passengerInfoFare.LastName,
                ExtensionData = null
            };
            return passengerName;
        }

        private static PassengerTitle MapPassengerTitle(PassengerInfoFare passengerInfoFare)
        {
            if (passengerInfoFare.Type == PassengerType.Adult)
                switch (passengerInfoFare.Title)
                {
                    case Title.Mister:
                        return PassengerTitle.MR;
                    case Title.Mistress:
                        return PassengerTitle.MRS;
                    case Title.Miss:
                        return PassengerTitle.MS;
                    default:
                        return PassengerTitle.Default;
                }
            else
                switch (passengerInfoFare.Title)
                {
                    case Title.Mister:
                        return PassengerTitle.MSTR;
                    case Title.Miss:
                        return PassengerTitle.MISS;
                    default:
                        return PassengerTitle.Default;
                }
        }

        private static Passport MapPassport(PassengerInfoFare passengerInfoFare)
        {
            var passport = new Passport
            {
                PassportNumber = passengerInfoFare.IdNumber,
                ExpiryDate = passengerInfoFare.PassportExpiryDate.GetValueOrDefault(),
                Country = passengerInfoFare.PassportCountry,
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
                    case "ERBUK002":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
                        result.ErrorMessages.Add("Invalid account information!");
                        goto case "TechnicalError";
                    case "ERBUK078":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
                        result.ErrorMessages.Add("Insufficient balance!");
                        goto case "TechnicalError";
                    case "ERBUK081":
                    case "ERBUK085":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
                        result.ErrorMessages.Add("Host not responding!");
                        goto case "TechnicalError";
                    case "ERGEN003":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
                        result.ErrorMessages.Add("Unexpected error on the other end!");
                        goto case "TechnicalError";
                    case "ERMAI001":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
                        result.ErrorMessages.Add("Mystifly is under maintenance!");
                        goto case "TechnicalError";

                    case "InvalidInputData":
                        if (!result.Errors.Contains(FlightError.InvalidInputData))
                            result.Errors.Add(FlightError.InvalidInputData);
                        break;
                    case "FareIdNoLongerValid":
                        if (!result.Errors.Contains(FlightError.FareIdNoLongerValid))
                            result.Errors.Add(FlightError.FareIdNoLongerValid);
                        break;
                    case "AlreadyBooked":
                        if (!result.Errors.Contains(FlightError.AlreadyBooked))
                            result.Errors.Add(FlightError.AlreadyBooked);
                        break;
                    case "ProcessFailed":
                        if (!result.Errors.Contains(FlightError.ProcessFailed))
                            result.Errors.Add(FlightError.ProcessFailed);
                        break;
                    case "TechnicalError":
                        if (!result.Errors.Contains(FlightError.TechnicalError))
                            result.Errors.Add(FlightError.TechnicalError);
                        break;
                }
            }
        }
    }
}
