using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;

namespace Lunggo.Flight.Query
{
    public class GetFlightRsvNoQuery : QueryBase<GetFlightRsvNoQuery, string>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause(condition));
            queryBuilder.Append(CreateWhereClause(condition));
            return queryBuilder.ToString();
        }
        private static string CreateSelectClause(dynamic condition = null)
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(
                @"SELECT DISTINCT r.RsvNo ");
            clauseBuilder.Append(
                @"FROM FlightReservations AS r " +
                @"INNER JOIN FlightTrip AS t ON t.RsvNo = r.RsvNo " +
                @"INNER JOIN FlightTripDetail AS td ON td.TripId = t.TripId " +
                @"INNER JOIN FlightPassenger AS p ON p.TripId = t.TripId ");
            return clauseBuilder.ToString();
        }
        private static string CreateWhereClause(dynamic condition = null)
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(
                @"WHERE ");
            if (condition.OriginAirport != null)
                clauseBuilder.Append(
                    @"(td.DepartureAirportCd = @OriginAirport AND td.SequenceNo = 1) AND ");
            if (condition.DestinationAirport != null)
                clauseBuilder.Append(
                    @"");
            if (condition.RsvDateStart != null)
                clauseBuilder.Append(
                    condition.RsvDateEnd != null
                        ? @"(r.RsvTime BETWEEN @RsvDateStart AND @RsvDateEnd) AND "
                        : @"r.RsvTime = @RsvDateStart AND ");
            if (condition.IsReturn != null)
                clauseBuilder.Append(
                    @"");
            if (condition.DepartureAirline != null)
                clauseBuilder.Append(
                    @"(td.CarrierCd = '@DepartureAirline' AND td.SequenceNo = 1) AND ");
            if (condition.ReturnAirline != null)
                clauseBuilder.Append(
                    @"");
            if (condition.DepartureDateStart != null)
                clauseBuilder.Append(
                    condition.DepartureDateEnd != null
                        ? @""
                        : @"");
            if (condition.ReturnDateStart != null)
                clauseBuilder.Append(
                    condition.ReturnDateEnd != null
                        ? @""
                        : @"");
            if (condition.PassengerName != null)
            {
                clauseBuilder.Append(
                    @"(p.FirstName LIKE '%' + @PassengerName + '%' OR p.LastName LIKE '%' + @PassengerName + '%') AND ");
            }
            if (condition.ContactName != null)
                clauseBuilder.Append(
                    @"r.ContactName LIKE '%' + @ContactName + '%' AND ");
            clauseBuilder.Remove(clauseBuilder.Length - 5, 5);
            return clauseBuilder.ToString();
        }
    }
}
