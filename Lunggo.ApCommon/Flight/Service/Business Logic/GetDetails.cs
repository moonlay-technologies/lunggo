using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public GetDetailsOutput GetDetails(GetDetailsInput input)
        {
            var output = new GetDetailsOutput();
            var details = GetTripDetailsInternal(input.BookingId);
            output.FlightDetails = MapDetails(details);
            if (input.ReturnBookingId != null)
            {
                details = GetTripDetailsInternal(input.ReturnBookingId);
                output.ReturnFlightDetails = MapDetails(details);
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
                PSCFare = details.PSCFare,
                Currency = details.Currency
            };
        }
    }
}
