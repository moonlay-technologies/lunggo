using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public IssueTicketOutput IssueTicket(IssueTicketInput input)
        {
            var output = new IssueTicketOutput();
            var orderResult = OrderTicketInternal(input.BookingId);
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
