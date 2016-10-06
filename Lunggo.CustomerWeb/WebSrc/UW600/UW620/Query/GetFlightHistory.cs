using Lunggo.Framework.Database;

namespace Lunggo.CustomerWeb.WebSrc.UW600.UW620.Query
{
    public class GetFlightHistory : DbQueryBase<GetFlightHistory, GetFlightHistoryRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT	a.RsvNo, a.PaymentMethodCd, a.MemberCd, a.OverallTripTypeCd,  b.ItineraryId, c.OriginAirportCd , c.DestinationAirportCd , c.DepartureDate, d.TripId, d.AirLineCd  FROM FlightReservation a INNER JOIN FlightItinerary b ON a.RsvNo = b.RsvNo INNER JOIN FlightTrip c ON b.ItineraryId = c.ItineraryId INNER JOIN FlightSegment d ON c.TripId = d.TripId WHERE MemberCd = @idMember ORDER BY  c.DepartureDate";
        }

    }
}