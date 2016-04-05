namespace Lunggo.ApCommon.Flight.Model
{
    internal class BookFlightResult : ResultBase
    {
        internal BookingStatusInfo Status { get; set; }
        internal bool IsValid { get; set; }
        internal bool IsItineraryChanged { get; set; }
        internal bool IsPriceChanged { get; set; }
        internal FlightItinerary NewItinerary { get; set; }
        internal decimal? NewPrice { get; set; }
    }
}
