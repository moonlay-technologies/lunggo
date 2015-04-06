using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Interface;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;

namespace Lunggo.ApCommon.Mystifly
{
    internal partial class MystiflyWrapper
    {
        internal override OrderTicketResult OrderTicket(string bookingId)
        {
            using (var client = new MystiflyClientHandler())
            {
                var request = new AirOrderTicketRQ
                {
                    FareSourceCode = null,
                    UniqueID = bookingId,
                    SessionId = client.SessionId,
                    Target = MystiflyClientHandler.Target,
                    ExtensionData = null
                };
                var result = new OrderTicketResult();
                var retry = 0;
                var done = false;
                while (!done)
                {
                    var response = client.TicketOrder(request);
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
                            if (error.Code == "EROTK002")
                            {
                                result.Errors.Clear();
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
                            result.Success = false;
                        }
                    }
                }
                return result;
            }
        }

        private static OrderTicketResult MapResult(AirOrderTicketRS response)
        {
            return new OrderTicketResult
            {
                IsOrderSuccess = response.Success,
                BookingId = response.UniqueID,
            };
        }

        private static void MapError(AirOrderTicketRS response, ResultBase result)
        {
            foreach (var error in response.Errors)
            {
                switch (error.Code)
                {
                    case "EROTK001":
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
                    case "EROTK008":
                        result.ErrorMessages.Add("Insufficient balance!");
                        goto case "TechnicalError";
                    case "ERGEN007":
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
                    case "BookingIdNoLongerValid":
                        result.Errors.Add(FlightError.BookingIdNoLongerValid);
                        break;
                    case "AlreadyBooked":
                        result.Errors.Add(FlightError.AlreadyBooked);
                        break;
                    case "TechnicalError":
                        result.Errors.Add(FlightError.TechnicalError);
                        break;
                }
            }
        }
    }
}
