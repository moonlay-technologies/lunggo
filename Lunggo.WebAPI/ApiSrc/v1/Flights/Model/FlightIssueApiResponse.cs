namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightIssueApiResponse
    {
        public bool IsSuccess { get; set; }
        public FlightIssueApiRequest OriginalRequest { get; set; }
    }
}