using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Redis;
using Gender = Lunggo.ApCommon.Flight.Constant.Gender;

namespace Lunggo.ApCommon.Flight.Wrapper.Mystifly
{
    internal partial class MystiflyWrapper
    {
        internal override BookFlightResult BookFlight(FlightBookingInfo bookInfo)
        {
            RevalidateConditions conditions = new RevalidateConditions
            {
                Itinerary = bookInfo.Itinerary,
                Trips = bookInfo.Itinerary.Trips
            };
            //conditions.Itinerary = bookInfo.Itinerary;
            RevalidateFareResult revalidateResult = RevalidateFare(conditions);
            if (revalidateResult.IsItineraryChanged || revalidateResult.IsPriceChanged || (!revalidateResult.IsValid))
            {
                return new BookFlightResult
                {
                    IsValid = revalidateResult.IsValid,
                    ErrorMessages = revalidateResult.ErrorMessages,
                    Errors = revalidateResult.Errors,
                    IsItineraryChanged = revalidateResult.IsItineraryChanged,
                    IsPriceChanged = revalidateResult.IsPriceChanged,
                    IsSuccess = false,
                    NewItinerary = revalidateResult.NewItinerary,
                    NewPrice = revalidateResult.NewPrice,
                    Status = null
                };
            }
            bookInfo.Itinerary = revalidateResult.NewItinerary;
            if (bookInfo.Itinerary.CanHold)
            {
                var airTravelers = bookInfo.Passengers.Select(MapAirTraveler).ToList();
                var travelerInfo = MapTravelerInfo(bookInfo.Contact, airTravelers);
                var request = new AirBookRQ
                {
                    FareSourceCode = bookInfo.Itinerary.FareId,
                    TravelerInfo = travelerInfo,
                    ClientMarkup = 0,
                    PaymentTransactionID = null,
                    PaymentCardInfo = null,
                    SessionId = Client.SessionId,
                    Target = Client.Target,
                    ExtensionData = null,
                };
                var result = new BookFlightResult();
                var retry = 0;
                var done = false;
                while (!done)
                {
                    var response = Client.BookFlight(request);
                    done = true;
                    if (response.Success && !response.Errors.Any() && response.Status.ToLower() == "confirmed")
                    {
                        result = MapResult(response, true);
                        result.IsSuccess = true;
                        result.Errors = null;
                        result.ErrorMessages = null;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result = MapResult(response, false);
                        if (response.Errors.Any())
                        {
                            result.Errors = new List<FlightError>();
                            result.ErrorMessages = new List<string>();
                            foreach (var error in response.Errors)
                            {
                                if (error.Code == "ERBUK001" || error.Code == "ERBUK002")
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
                            }
                            if (done)
                                MapError(response, result);
                        }
                    }
                }
                return result;
            }
            else
            {
                var bookingId = WebfareBooking(bookInfo);
                var result = new BookFlightResult
                {
                    IsSuccess = true,
                    Status = new BookingStatusInfo
                    {
                        BookingId = bookingId,
                        BookingStatus = BookingStatus.Booked,
                        TimeLimit = DateTime.UtcNow.AddHours(2)
                    }
                };
                return result;
            }
        }

        private static string WebfareBooking(FlightBookingInfo bookInfo)
        {
            var bookingId = FlightBookingIdSequence.GetInstance().GetNext().ToString(CultureInfo.InvariantCulture);

            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisKey = "MystiflyWebfare:" + bookingId;
            var cacheObject = bookInfo.ToCacheObject();
            redisDb.StringSet(redisKey, cacheObject, TimeSpan.FromHours(2));

            return bookingId;
        }

        private static AirTraveler MapAirTraveler(Pax passenger)
        {
            var airTraveler = new AirTraveler
            {
                PassengerName = MapPassengerName(passenger),
                PassengerType = MapPassengerType(passenger),
                Gender = MapGender(passenger),
                Passport = MapPassport(passenger),
                DateOfBirth = passenger.DateOfBirth,
                ExtraServices = null,
                ExtraServices1_1 = null,
                SpecialServiceRequest = null,
                FrequentFlyerNumber = null,
                ResidentFareDocumentType = null,
                ExtensionData = null
            };
            return airTraveler;
        }

        private static ApCommon.Mystifly.OnePointService.Flight.PassengerType MapPassengerType(Pax passenger)
        {
            switch (passenger.Type)
            {
                case PaxType.Adult:
                    return ApCommon.Mystifly.OnePointService.Flight.PassengerType.ADT;
                case PaxType.Child:
                    return ApCommon.Mystifly.OnePointService.Flight.PassengerType.CHD;
                case PaxType.Infant:
                    return ApCommon.Mystifly.OnePointService.Flight.PassengerType.INF;
                default:
                    return ApCommon.Mystifly.OnePointService.Flight.PassengerType.Default;
            }
        }

        private static ApCommon.Mystifly.OnePointService.Flight.Gender MapGender(Pax passenger)
        {
            switch (passenger.Gender)
            {
                case Gender.Male:
                    return ApCommon.Mystifly.OnePointService.Flight.Gender.M;
                case Gender.Female:
                    return ApCommon.Mystifly.OnePointService.Flight.Gender.F;
                default:
                    return ApCommon.Mystifly.OnePointService.Flight.Gender.Default;
            }
        }

        private static PassengerName MapPassengerName(Pax passenger)
        {
            var passengerName = new PassengerName
            {
                PassengerTitle = MapPassengerTitle(passenger),
                PassengerFirstName = passenger.FirstName,
                PassengerLastName = passenger.LastName,
                ExtensionData = null
            };
            return passengerName;
        }

        private static PassengerTitle MapPassengerTitle(Pax passenger)
        {
            switch (passenger.Type)
            {
                case PaxType.Adult:
                    switch (passenger.Title)
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
                case PaxType.Child:
                    switch (passenger.Title)
                    {
                        case Title.Mister:
                            return PassengerTitle.MSTR;
                        case Title.Miss:
                            return PassengerTitle.MISS;
                        default:
                            return PassengerTitle.Default;
                    }
                case PaxType.Infant:
                    switch (passenger.Title)
                    {
                        case Title.Mister:
                        case Title.Miss:
                            return PassengerTitle.INF;
                        default:
                            return PassengerTitle.Default;
                    }
                default:
                    return PassengerTitle.Default;
            }
        }

        private static Passport MapPassport(Pax passenger)
        {
            var passport = new Passport
            {
                PassportNumber = passenger.PassportNumber ?? " ",
                ExpiryDate = passenger.PassportExpiryDate.GetValueOrDefault(DateTime.Now.AddYears(5)),
                Country = passenger.PassportCountry ?? "XX",
                ExtensionData = null
            };
            return passport;
        }

        private static TravelerInfo MapTravelerInfo(Contact contact, List<AirTraveler> airTravelers)
        {
            var travelerInfo = new TravelerInfo
            {
                CountryCode = contact.CountryCallingCode,
                AreaCode = "",
                PhoneNumber = contact.Phone,
                Email = contact.Email,
                AirTravelers = airTravelers.ToArray(),
                ExtensionData = null
            };
            return travelerInfo;
        }

        private static BookFlightResult MapResult(AirBookRS response, bool isSuccess)
        {
            if (isSuccess)
                return new BookFlightResult
                {
                    Status = new BookingStatusInfo
                    {
                        BookingStatus = BookingStatus.Booked,
                        BookingId = response.UniqueID,
                        TimeLimit = response.TktTimeLimit.HasValue ? DateTime.SpecifyKind(response.TktTimeLimit.Value, DateTimeKind.Utc) : DateTime.MaxValue
                    }
                };
            else
                return new BookFlightResult
                {
                    Status = new BookingStatusInfo
                    {
                        BookingStatus = BookingStatus.Failed,
                        BookingId = response.UniqueID,
                    }
                };
        }

        private static void MapError(AirBookRS response, ResultBase result)
        {
            foreach (var error in response.Errors)
            {
                switch (error.Code)
                {
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
                    case "ERBUK081":
                    case "ERBUK085":
                        goto case "FailedOnSupplier";
                    case "ERBUK001":
                    case "ERBUK002":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
                        if (!result.ErrorMessages.Contains("[Mystifly] Invalid account information!"))
                            result.ErrorMessages.Add("[Mystifly] Invalid account information!");
                        goto case "TechnicalError";
                    case "ERBUK078":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
                        if (!result.ErrorMessages.Contains("[Mystifly] Insufficient balance!"))
                            result.ErrorMessages.Add("[Mystifly] Insufficient balance!");
                        goto case "TechnicalError";
                    case "ERGEN003":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
                        if (!result.ErrorMessages.Contains("[Mystifly] Unexpected error on the other end!"))
                            result.ErrorMessages.Add("[Mystifly] Unexpected error on the other end!");
                        goto case "TechnicalError";
                    case "ERMAI001":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
                        if (!result.ErrorMessages.Contains("[Mystifly] Mystifly is under maintenance!"))
                            result.ErrorMessages.Add("[Mystifly] Mystifly is under maintenance!");
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
                    case "FailedOnSupplier":
                        if (!result.Errors.Contains(FlightError.FailedOnSupplier))
                            result.Errors.Add(FlightError.FailedOnSupplier);
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
