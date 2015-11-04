using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Redis;

namespace Lunggo.ApCommon.Flight.Wrapper.Mystifly
{
    internal partial class MystiflyWrapper
    {
        internal override OrderTicketResult OrderTicket(string bookingId, bool canHold)
        {
            if (canHold)
            {
                var request = new AirOrderTicketRQ
                {
                    FareSourceCode = null,
                    UniqueID = bookingId,
                    SessionId = Client.SessionId,
                    Target = Client.Target,
                    ExtensionData = null
                };
                var result = new OrderTicketResult();
                var retry = 0;
                var done = false;
                while (!done)
                {
                    var response = Client.TicketOrder(request);
                    done = true;
                    if (response.Success && !response.Errors.Any())
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
                                if (error.Code == "EROTK001" || error.Code == "EROTK002")
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
                        result.IsSuccess = false;
                    }
                }
                return result;
            }
            else
            {
                var bookInfo = WebfareBooking(bookingId);
                var airTravelers = bookInfo.Passengers.Select(MapAirTraveler).ToList();
                var travelerInfo = MapTravelerInfo(bookInfo.ContactData, airTravelers);
                var request = new AirBookRQ
                {
                    FareSourceCode = bookInfo.FareId,
                    TravelerInfo = travelerInfo,
                    ClientMarkup = 0,
                    PaymentTransactionID = null,
                    PaymentCardInfo = null,
                    SessionId = Client.SessionId,
                    Target = Client.Target,
                    ExtensionData = null,
                };
                var result = new OrderTicketResult();
                var retry = 0;
                var done = false;
                while (!done)
                {
                    var response = Client.BookFlight(request);
                    done = true;
                    if (response.Success && !response.Errors.Any() && response.Status == "CONFIRMED")
                    {
                        result = MapBookResult(response);
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
        }

        private static FlightBookingInfo WebfareBooking(string bookingId)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisKey = "MystiflyWebfare:" + bookingId;
            var cacheObject = redisDb.StringGet(redisKey);

            if (!cacheObject.IsNullOrEmpty)
            {
                var bookInfo = cacheObject.DeconvertTo<FlightBookingInfo>();
                return bookInfo;
            }
            else
            {
                return null;
            }
        }

        private static OrderTicketResult MapResult(AirOrderTicketRS response)
        {
            return new OrderTicketResult
            {
                BookingId = response.UniqueID,
                IsInstantIssuance = false
            };
        }

        private static OrderTicketResult MapBookResult(AirBookRS response)
        {
            return new OrderTicketResult
            {
                BookingId = response.UniqueID
            };
        }

        private static void MapError(AirOrderTicketRS response, ResultBase result)
        {
            foreach (var error in response.Errors)
            {
                switch (error.Code)
                {
                    case "EROTK003":
                    case "EROTK009":
                        goto case "InvalidInputData";
                    case "EROTK005":
                        goto case "FareIdNoLongerValid";
                    case "EROTK004":
                    case "EROTK006":
                        goto case "BookingIdNoLongerValid";
                    case "EROTK007":
                        goto case "AlreadyBooked";
                    case "EROTK001":
                    case "EROTK002":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
                        if (!result.ErrorMessages.Contains("Invalid account information!"))
                            result.ErrorMessages.Add("Invalid account information!");
                        goto case "TechnicalError";
                    case "EROTK008":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
                        if (!result.ErrorMessages.Contains("Insufficient balance!"))
                            result.ErrorMessages.Add("Insufficient balance!");
                        goto case "TechnicalError";
                    case "ERGEN007":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
                        if (!result.ErrorMessages.Contains("Unexpected error on the other end!"))
                            result.ErrorMessages.Add("Unexpected error on the other end!");
                        goto case "TechnicalError";
                    case "ERMAI001":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
                        if (!result.ErrorMessages.Contains("Mystifly is under maintenance!"))
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
                    case "BookingIdNoLongerValid":
                        if (!result.Errors.Contains(FlightError.InvalidInputData))
                            result.Errors.Add(FlightError.BookingIdNoLongerValid);
                        break;
                    case "AlreadyBooked":
                        if (!result.Errors.Contains(FlightError.AlreadyBooked))
                            result.Errors.Add(FlightError.AlreadyBooked);
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
