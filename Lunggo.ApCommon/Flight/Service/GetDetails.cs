using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public GetDetailsOutput GetDetails(GetDetailsInput input)
        {
            var output = new GetDetailsOutput();
            var details = GetTripDetailsInternal(input.BookingId);
            output.FlightDetails = MapDetails(details);
            return output;
        }

        private static FlightDetails MapDetails(GetTripDetailsResult details)
        {
            return new FlightDetails
            {
                BookingId = details.BookingId,
                BookingNotes = details.BookingNotes,
                FlightSegmentCount = details.FlightSegmentCount,
                FlightItineraryDetails = details.FlightItineraryDetails,
                TotalFare = details.TotalFare,
                AdultTotalFare = details.AdultTotalFare,
                ChildTotalFare = details.ChildTotalFare,
                InfantTotalFare = details.InfantTotalFare,
                PscFare = details.PSCFare,
                Currency = details.Currency
            };
        }
    }
}
