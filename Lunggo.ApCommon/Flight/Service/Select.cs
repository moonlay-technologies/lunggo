namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public string SelectFlight(string searchId, int itinIndex, string requestId)
        {
            var token = SaveItineraryFromSearchToCache(searchId, itinIndex, requestId);
            return token;
        }
    }
}
