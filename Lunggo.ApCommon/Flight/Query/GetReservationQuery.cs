using System.Text;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Flight.Query
{
    internal class GetReservationQuery : QueryBase<GetReservationQuery, FlightReservation, FlightReservationTableRecord, FlightItineraryTableRecord, FlightTripTableRecord, FlightSegmentTableRecord, FlightPassengerTableRecord, FlightStopTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause());
            queryBuilder.Append(CreateWhereClause(condition));
            return queryBuilder.ToString();
        }

        private static string CreateSelectClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"SELECT r.RsvNo, r.RsvTime, r.RsvStatusCd, r.CancellationTypeCd, r.CancellationTime, ");
            clauseBuilder.Append(@"r.UserId, r.AdultCount, r.ChildCount, r.InfantCount, r.OverallTripTypeCd, ");
            clauseBuilder.Append(@"i.Id, i.BookingId, i.BookingStatusCd, i.TicketTimeLimit, i.TripTypeCd, i.FareTypeCd, ");
            clauseBuilder.Append(@"i.SupplierCd, ");
            clauseBuilder.Append(@"t.Id, t.ItineraryId, t.OriginAirportCd, t.DestinationAirportCd, t.DepartureDate, ");
            clauseBuilder.Append(@"s.Id, s.TripId, s.Pnr, s.OperatingAirlineCd, s.AirlineCd, s.FlightNumber, s.AircraftCd, ");
            clauseBuilder.Append(@"s.DepartureAirportCd, s.DepartureTerminal, s.DepartureTime, ");
            clauseBuilder.Append(@"s.ArrivalAirportCd, s.ArrivalTerminal, s.ArrivalTime, ");
            clauseBuilder.Append(@"s.Duration, s.Rbd, s.CabinClassCd, s.AirlineTypeCd, s.Baggage, s.Meal, ");
            clauseBuilder.Append(@"p.Id, p.RsvNo, p.TitleCd, p.FirstName, p.LastName, ");
            clauseBuilder.Append(@"p.TypeCd, p.BirthDate, ");
            clauseBuilder.Append(@"st.Id, st.SegmentId, st.AirportCd, st.ArrivalTime, st.DepartureTime, ");
            clauseBuilder.Append(@"st.Duration ");
            clauseBuilder.Append(@"FROM FlightReservation AS r ");
            clauseBuilder.Append(@"INNER JOIN FlightItinerary AS i ON i.RsvNo = r.RsvNo ");
            clauseBuilder.Append(@"INNER JOIN FlightTrip AS t ON t.ItineraryId = i.Id ");
            clauseBuilder.Append(@"INNER JOIN FlightSegment AS s ON s.TripId = t.Id ");
            clauseBuilder.Append(@"INNER JOIN FlightPassenger AS p ON p.RsvNo = r.RsvNo ");
            clauseBuilder.Append(@"LEFT OUTER JOIN FlightStop AS st ON st.SegmentId = s.Id ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause(dynamic condition)
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"WHERE r.RsvNo = @RsvNo");
            return clauseBuilder.ToString();
        }
    }
}
