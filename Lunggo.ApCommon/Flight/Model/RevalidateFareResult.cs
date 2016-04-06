namespace Lunggo.ApCommon.Flight.Model
{
    internal class RevalidateFareResult : ResultBase
    {
        internal bool IsValid { get; set; }
        internal bool IsItineraryChanged { get; set; }
        internal bool IsPriceChanged { get; set; }
        internal FlightItinerary NewItinerary { get; set; }
        internal decimal? NewPrice { get; set; }
    }
}
