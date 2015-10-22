using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsQuery.ExtensionMethods;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;

namespace Lunggo.ApCommon.Flight.Wrapper.AirAsia
{
    internal partial class AirAsiaWrapper
    {
        internal override GetTripDetailsResult GetTripDetails(TripDetailsConditions conditions)
        {
            var rsvNo = FlightService.GetDb.RsvNoByBookingId(new List<string> {conditions.BookingId}).Single();
            var reservation = FlightService.GetDb.Reservation(rsvNo);
            var itinerary = reservation.Itineraries.Single(itin => itin.BookingId == conditions.BookingId);
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
