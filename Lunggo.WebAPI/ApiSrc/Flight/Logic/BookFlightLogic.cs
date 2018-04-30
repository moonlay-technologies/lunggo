using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.Framework.Config;
using Lunggo.Framework.Context;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Log;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Flight.Model;
using Lunggo.ApCommon.Log;

namespace Lunggo.WebAPI.ApiSrc.Flight.Logic
{
    public static partial class FlightLogic
    {
        public static ApiResponseBase BookFlight(FlightBookApiRequest request)
        {
            if (IsValid(request))
            {
                var TableLog = new GlobalLog();
               
                TableLog.PartitionKey = "BOOKING API LOG";
         
                OnlineContext.SetActiveLanguageCode(request.LanguageCode);
                var bookServiceRequest = PreprocessServiceRequest(request);
                var bookServiceResponse = FlightService.GetInstance().BookFlight(bookServiceRequest);
                var apiResponse = AssembleApiResponse(bookServiceResponse, request.Test);
                if (apiResponse.StatusCode != HttpStatusCode.OK)
                {
                    var log = LogService.GetInstance();
                    var env = EnvVariables.Get("general", "environment");
                    TableLog.Log = "```Booking API Log```"
                            + "\n`*Environment :* " + env.ToUpper()
                            + "\n*Platform :* "
                            + Client.GetPlatformType(HttpContext.Current.User.Identity.GetClientId());                            
                        log.Post(TableLog.Log
                            ,
                            env == "production" ? "#logging-prod" : "#logging-dev",
                            new List<LogAttachment>
                            {
                                new LogAttachment("REQUEST", request.Serialize()),
                                new LogAttachment("RESPONSE", apiResponse.Serialize()),
                                new LogAttachment("LOGIC RESPONSE", bookServiceResponse.Serialize()),
                                new LogAttachment("ITINERARY", FlightService.GetInstance().GetItineraryForDisplay(request.Token).Serialize())
                            });
                    TableLog.Log = TableLog.Log + "\n*REQUEST:*" + request.Serialize()
                            + "\n*RESPONSE:*" + apiResponse.Serialize()
                            + "\n*LOGIC RESPONSE:*" + bookServiceResponse.Serialize()
                            + "\n*ITINERARY:*" + FlightService.GetInstance().GetItineraryForDisplay(request.Token).Serialize();
                    TableLog.Logging();
                }
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
            try
            {
                return
                    request != null &&
                    !string.IsNullOrEmpty(request.Token) &&
                    !string.IsNullOrEmpty(request.LanguageCode) &&
                    request.Contact != null &&
                    request.Contact.Title != Title.Undefined &&
                    !string.IsNullOrEmpty(request.Contact.Name) &&
                    !string.IsNullOrEmpty(request.Contact.Phone) &&
                    new MailAddress(request.Contact.Email) != null &&
                    !string.IsNullOrEmpty(request.Contact.CountryCallingCode) &&
                    request.Passengers != null &&
                    request.Passengers.Any() &&
                    request.Passengers.TrueForAll(p => !string.IsNullOrEmpty(p.Name)) &&
                    request.Passengers.TrueForAll(p => p.Title != Title.Undefined) &&
                    request.Passengers.TrueForAll(p => p.Type != PaxType.Undefined);
            }
            catch
            {
                return false;
            }
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
                        TimeLimit = bookServiceResponse.TimeLimit.TruncateMilliseconds(),
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
                //if (bookServiceResponse.Errors[0] == FlightError.PartialSuccess)
                //    bookServiceResponse.Errors.RemoveAt(0);
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
                Passengers = FlightService.GetInstance().ConvertToPax(request.Passengers),
                Test = request.Test
            };
            return bookServiceRequest;
        }
    }
}