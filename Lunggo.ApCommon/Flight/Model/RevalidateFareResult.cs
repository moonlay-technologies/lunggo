namespace Lunggo.ApCommon.Flight.Model
{
    internal class RevalidateFareResult : ResultBase
    {
        internal bool IsValid { get; set; }
        internal FlightItinerary Itinerary { get; set; }
    }
}
