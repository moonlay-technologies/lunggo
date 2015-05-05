namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class CancelBookingOutput : OutputBase
    {
        public bool IsCancelSuccess { get; set; }
        public string BookingId { get; set; }
    }
}
