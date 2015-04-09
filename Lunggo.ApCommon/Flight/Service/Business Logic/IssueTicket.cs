using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using StackExchange.Redis;

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
                output.BookingStatus = BookingStatus.Ticketing;
                output.BookingId = orderResult.BookingId;
            }
            else
            {
                output.IsSuccess = false;
                output.Errors = orderResult.Errors;
                output.ErrorMessages = orderResult.ErrorMessages;
            }

            if (input.ReturnBookingId != null)
            {
                orderResult = OrderTicketInternal(input.ReturnBookingId);
                if (orderResult.IsSuccess)
                {
                    output.ReturnBookingStatus = BookingStatus.Ticketing;
                    output.ReturnBookingId = orderResult.BookingId;
                }
                else
                {
                    output.IsSuccess = false;
                    output.Errors = orderResult.Errors;
                    output.ErrorMessages = orderResult.ErrorMessages;
                }
            }
            return output;
        }
    }
}
