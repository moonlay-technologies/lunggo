using System.Collections.Generic;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightRevalidateApiRequest
    {
        public string SearchId { get; set; }
        public List<int> ItinIndices { get; set; }
        public string Token { get; set; }
        public string SecureCode { get; set; }
    }
}