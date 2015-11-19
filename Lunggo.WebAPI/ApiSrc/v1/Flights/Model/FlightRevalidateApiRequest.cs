namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightRevalidateApiRequest
    {
        public string SearchId { get; set; }
        public int ItinIndex { get; set; }
        public string Token { get; set; }
        public string SecureCode { get; set; }
    }
}