using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Interface;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;

namespace Lunggo.ApCommon.Mystifly
{
    public partial class MystiflyWrapper : IOrderTicket
    {
        public OrderTicketResult OrderTicket(FlightBooking booking)
        {
            using (var client = new MystiflyClientHandler())
            {
                var request = new AirOrderTicketRQ
                {
                    FareSourceCode = null,
                    UniqueID = booking.BookingId,
                    SessionId = client.SessionId,
                    Target = client.Target,
                    ExtensionData = null
                };
                var result = new OrderTicketResult();
                var retry = 0;
                var done = false;
                while (retry < 3 && !done)
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

        private static OrderTicketResult MapResult(AirOrderTicketRS response)
        {
            return new OrderTicketResult
            {
                IsOrderSuccess = response.Success,
                BookingId = response.UniqueID,
            };
        }

    }
}
