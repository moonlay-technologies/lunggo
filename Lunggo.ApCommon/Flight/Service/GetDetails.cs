using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public GetDetailsOutput GetDetails(GetDetailsInput input)
        {
            var output = new GetDetailsOutput();
            var request = new TripDetailsConditions
            {
                BookingId = input.BookingId,
                TripInfos = input.TripInfos
            };
            var details = GetTripDetailsInternal(request);
            if (details.IsSuccess)
            {
                output.IsSuccess = true;
                output.FlightDetails = MapDetails(details);
            }
            else
            {
                output.IsSuccess = false;
                output.Errors = details.Errors;
                output.ErrorMessages = details.ErrorMessages;
            }
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
