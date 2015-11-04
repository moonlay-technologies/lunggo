using Lunggo.ApCommon.Flight.Model.Logic;

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
            return output;
        }
    }
}
