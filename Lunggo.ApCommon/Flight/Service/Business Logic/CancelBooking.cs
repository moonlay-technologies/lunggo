using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public CancelBookingOutput CancelBooking(CancelBookingInput input)
        {
            var output = new CancelBookingOutput();
            var cancelResult = CancelBookingInternal(input.BookingId);
            if (cancelResult.IsSuccess)
            {
                output.IsCancelSuccess = true;
                output.BookingId = cancelResult.BookingId;
            }
            if (input.ReturnBookingId != null)
            {
                cancelResult = CancelBookingInternal(input.ReturnBookingId);
                if (cancelResult.IsSuccess)
                {
                    output.ReturnIsCancelSuccess = true;
                    output.ReturnBookingId = cancelResult.BookingId;
                }
            }
            return output;
        }
    }
}
