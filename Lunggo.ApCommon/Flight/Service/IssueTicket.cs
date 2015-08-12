using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Database.Query;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.Framework.Database;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;
using StackExchange.Redis;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public IssueTicketOutput IssueTicket(IssueTicketInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                if (input.BookingId == null)
                    input.BookingId = GetFlightBookingIdQuery.GetInstance().Execute(conn, new {input.RsvNo}).Single();
                var output = new IssueTicketOutput();
                var orderResult = OrderTicketInternal(input.BookingId);
                if (orderResult.IsSuccess)
                {
                    output.IsSuccess = true;
                    output.BookingStatus = BookingStatus.Ticketing;
                    output.BookingId = orderResult.BookingId;

                    var bookingStatus = orderResult.IsInstantIssuance ? BookingStatus.Ticketed : BookingStatus.Ticketing;
                    var bookingStatusCd = BookingStatusCd.Mnemonic(bookingStatus);
                    UpdateFlightBookingStatusQuery.GetInstance().Execute(conn, new
                    {
                        input.BookingId,
                        NewBookingId = orderResult.BookingId,
                        BookingStatusCd = bookingStatusCd
                    });

                    if (orderResult.IsInstantIssuance)
                    {
                        var detailsInput = new GetDetailsInput {RsvNo = input.RsvNo};
                        GetAndUpdateNewDetails(detailsInput);
                        SendEticketToCustomer(input.RsvNo);
                    }
                }
                else
                {
                    output.IsSuccess = false;
                    output.Errors = orderResult.Errors;
                    output.ErrorMessages = orderResult.ErrorMessages;
                }
                return output;
            }
        }
    }
}
