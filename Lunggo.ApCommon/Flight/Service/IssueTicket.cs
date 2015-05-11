using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public IssueTicketOutput IssueTicket(IssueTicketInput input)
        {
            var output = new IssueTicketOutput();
            var orderInfo = new FlightOrderInfo
            {
                BookingId = input.BookingId.Substring(7),
                FareType = FareTypeCd.Mnemonic(input.BookingId.Substring(4,3)),
                Supplier = FlightSupplierCd.Mnemonic(input.BookingId.Substring(0,4))
            };
            var orderResult = OrderTicketInternal(orderInfo);
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
