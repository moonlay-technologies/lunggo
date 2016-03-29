namespace Lunggo.ApCommon.Flight.Model
{
    public class RevalidateFareResult : ResultBase
    {
        internal bool IsValid { get; set; }
        internal FlightItinerary Itinerary { get; set; }
    }
}
