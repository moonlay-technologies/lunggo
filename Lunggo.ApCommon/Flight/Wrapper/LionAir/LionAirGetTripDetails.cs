using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;

namespace Lunggo.ApCommon.Flight.Wrapper.LionAir
{
    internal partial class LionAirWrapper
    {
        internal override GetTripDetailsResult GetTripDetails(TripDetailsConditions conditions)
        {
            var bookingId = "LIONPUB" + conditions.BookingId;
            var rsvNo = FlightService.GetRsvNoByBookingIdFromDb(new List<string> { bookingId }).Single();
            var reservation = FlightService.GetInstance().GetReservationFromDb(rsvNo);
            var itinerary = reservation.Itineraries.Single(itin => itin.BookingId == bookingId);
            var segments = itinerary.Trips.SelectMany(trip => trip.Segments).ToList();
            segments.ForEach(segment => segment.Pnr = conditions.BookingId);
            return new GetTripDetailsResult
            {
                IsSuccess = true,
                BookingId = conditions.BookingId,
                Itinerary = itinerary,
                FlightSegmentCount = segments.Count
            };
        }
    }
}
