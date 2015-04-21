using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query
{
    internal class UpdateFlightDetailsQuery : NoReturnQueryBase<UpdateFlightDetailsQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateUpdateClause());
            queryBuilder.Append(CreateSetClause());
            queryBuilder.Append(CreateWhereClause());
            return queryBuilder.ToString();
        }

        private static string CreateUpdateClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"UPDATE FlightSegment AS sg ");
            clauseBuilder.Append(@"INNER JOIN FlightTrip AS t ON sg.TripId = t.TripId ");
            clauseBuilder.Append(@"INNER JOIN FlightItinerary AS i ON t.ItineraryId = i.ItineraryId ");
            return clauseBuilder.ToString();
        }

        private static string CreateSetClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"SET ");
            clauseBuilder.Append(@"i.BookingStatusCd = @BookingStatus, ");
            clauseBuilder.Append(@"sg.Pnr = @Pnr, ");
            clauseBuilder.Append(@"sg.DepartureTerminal = @DepartureTerminal, ");
            clauseBuilder.Append(@"sg.ArrivalTerminal = @ArrivalTerminal, ");
            clauseBuilder.Append(@"sg.Baggage = @Baggage ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"WHERE ");
            clauseBuilder.Append(@"i.BookingId = @BookingId AND ");
            clauseBuilder.Append(@"sg.SegmentId IN @FlightSegmentPrimKeys AND ");
            clauseBuilder.Append(@"sg.DepartureAirportCd = @DepartureAirport AND ");
            clauseBuilder.Append(@"sg.ArrivalAirportCd = @ArrivalAirport AND ");
            clauseBuilder.Append(@"sg.DepartureTime BETWEEN (@DepartureTime AND DATEADD(day, 1, @DepartureTime))");
            return clauseBuilder.ToString();
        }
    }
}
