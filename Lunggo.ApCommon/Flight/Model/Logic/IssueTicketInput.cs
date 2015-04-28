namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class IssueTicketInput
    {
        public string BookingId { get; set; }
        public ReservationDetails ReservationDetails { get; set; }
    }
}
