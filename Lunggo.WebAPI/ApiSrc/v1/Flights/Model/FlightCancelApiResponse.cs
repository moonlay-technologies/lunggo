namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightCancelApiResponse
    {
        public string Result { get; set; }
        public string BookingId { get; set; }
        public FlightCancelApiRequest OriginalRequest { get; set; }
    }
}