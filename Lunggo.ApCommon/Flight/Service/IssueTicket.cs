using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Query;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public IssueTicketOutput IssueTicket(IssueTicketInput input)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var bookingId = GetFlightBookingIdQuery.GetInstance().Execute(conn, new {input.RsvNo}).Single();
                var output = new IssueTicketOutput();
                var orderResult = OrderTicketInternal(bookingId);
                if (orderResult.IsSuccess)
                {
                    output.IsSuccess = true;
                    output.BookingStatus = BookingStatus.Ticketing;
                    output.BookingId = orderResult.BookingId;
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
