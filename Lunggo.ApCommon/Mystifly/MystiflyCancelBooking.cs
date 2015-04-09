using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;

namespace Lunggo.ApCommon.Mystifly
{
    internal partial class MystiflyWrapper
    {
        internal override CancelBookingResult CancelBooking(string bookingId)
        {
            using (var client = new MystiflyClientHandler())
            {
                var request = new AirCancelRQ
                {
                    UniqueID = bookingId,
                    SessionId = client.SessionId,
                    Target = MystiflyClientHandler.Target,
                    ExtensionData = null
                };
                var result = new CancelBookingResult();
                var retry = 0;
                var done = false;
                while (!done)
                {
                    var response = client.CancelBooking(request);
                    done = true;
                    if (!response.Errors.Any())
                    {
                        result = MapResult(response);
                        result.IsSuccess = true;
                    }
                    else
                    {
                        foreach (var error in response.Errors)
                        {
                            if (error.Code == "ERCBK002")
                            {
                                result.Errors.Clear();
                                client.CreateSession();
                                request.SessionId = client.SessionId;
                                retry++;
                                if (retry <= 3) { 
                                    done = false;
                                    break;
                                }
                            }
                            MapError(response, result);
                            result.IsSuccess = false;
                        }
                    }
                }
                return result;
            }
        }

        private static CancelBookingResult MapResult(AirCancelRS response)
        {
            return new CancelBookingResult
            {
                BookingId = response.UniqueID
            };
        }

        private static void MapError(AirCancelRS response, ResultBase result)
        {
            foreach (var error in response.Errors)
            {
                switch (error.Code)
                {
                    case "ERCBK001":
                    case "ERCBK004":
                        goto case "InvalidInputData";
                    case "ERCBK003":
                        goto case "BookingIdNoLongerValid";
                    case "ERCBK005":
                    case "ERCBK006":
                        goto case "ProcessFailed";
                    case "ERCBK007":
                        goto case "AlreadyBooked";
                    case "ERGEN008":
                        result.ErrorMessages.Add("Unexpected error on the other end!");
                        goto case "TechnicalError";
                    case "ERMAI001":
                        result.ErrorMessages.Add("Mystifly is under maintenance!");
                        goto case "TechnicalError";

                    case "InvalidInputData":
                        result.Errors.Add(FlightError.InvalidInputData);
                        break;
                    case "BookingIdNoLongerValid":
                        result.Errors.Add(FlightError.BookingIdNoLongerValid);
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
