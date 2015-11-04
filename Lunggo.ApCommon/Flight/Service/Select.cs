namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public string SelectFlight(string searchId, int itinIndex)
        {
            var token = SaveItineraryFromSearchToCache(searchId, itinIndex);
            return token;
        }
    }
}
