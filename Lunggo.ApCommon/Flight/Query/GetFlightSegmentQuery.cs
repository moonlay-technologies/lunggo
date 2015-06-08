using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Query.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Flight.Query
{
    internal class GetFlightSegmentQuery : QueryBase<GetFlightSegmentQuery, FlightSegmentTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause());
            if (condition != null)
                queryBuilder.Append(CreateWhereClause(condition));
            return queryBuilder.ToString();
        }

        private static string CreateSelectClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"SELECT sg.SegmentId, sg.DepartureAirportCd, sg.ArrivalAirportCd, sg.DepartureTime ");
            clauseBuilder.Append(@"FROM FlightItinerary AS i ");
            clauseBuilder.Append(@"INNER JOIN FlightTrip AS t ON t.ItineraryId = i.ItineraryId ");
            clauseBuilder.Append(@"INNER JOIN FlightSegment AS sg ON sg.TripId = t.TripId ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause(dynamic condition)
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"WHERE ");
            if (condition.BookingId != null)
                clauseBuilder.Append(@"i.BookingId = @BookingId AND ");
            if (condition.DepartureAirport != null && condition.ArrivalAirport != null && condition.DepartureDate != null)
            {
                clauseBuilder.Append(@"sg.DepartureAirportCd = @DepartureAirport AND ");
                clauseBuilder.Append(@"sg.ArrivalAirportCd = @ArrivalAirport AND ");
                clauseBuilder.Append(@"sg.DepartureTime BETWEEN (@DepartureTime AND DATEADD(day, 1, @DepartureTime)) AND ");
            }
            clauseBuilder.Remove(clauseBuilder.Length - 5, 5);
            return clauseBuilder.ToString();
        }
    }
}
