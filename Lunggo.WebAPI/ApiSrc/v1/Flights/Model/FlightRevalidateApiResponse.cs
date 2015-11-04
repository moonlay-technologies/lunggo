namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightRevalidateApiResponse
    {
        public string Token { get; set; }
        public bool IsValid { get; set; }
        public bool? IsOtherFareAvailable { get; set; }
        public decimal? NewFare { get; set; }
        public FlightRevalidateApiRequest OriginalRequest { get; set; }
    }
}