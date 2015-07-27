using System.Text;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Database.Query
{
    internal class UpdateFlightDetailsQuery : NoReturnQueryBase<UpdateFlightDetailsQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateUpdateClause());
            queryBuilder.Append(CreateFromClause());
            queryBuilder.Append(CreateWhereClause());
            return queryBuilder.ToString();
        }

        private static string CreateUpdateClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"UPDATE sg ");
            clauseBuilder.Append(@"SET ");
            clauseBuilder.Append(@"sg.Pnr = @Pnr, ");
            clauseBuilder.Append(@"sg.DepartureTerminal = @DepartureTerminal, ");
            clauseBuilder.Append(@"sg.ArrivalTerminal = @ArrivalTerminal, ");
            clauseBuilder.Append(@"sg.Baggage = @Baggage ");
            return clauseBuilder.ToString();
        }

        private static string CreateFromClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"FROM FlightSegment AS sg ");
            clauseBuilder.Append(@"INNER JOIN FlightTrip AS t ON sg.TripId = t.TripId ");
            clauseBuilder.Append(@"INNER JOIN FlightItinerary AS i ON t.ItineraryId = i.ItineraryId ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"WHERE ");
            clauseBuilder.Append(@"i.BookingId = @BookingId AND ");
            clauseBuilder.Append(@"sg.DepartureAirportCd = @DepartureAirport AND ");
            clauseBuilder.Append(@"sg.ArrivalAirportCd = @ArrivalAirport AND ");
            clauseBuilder.Append(@"sg.DepartureTime = @DepartureTime");
            return clauseBuilder.ToString();
        }
    }
}
