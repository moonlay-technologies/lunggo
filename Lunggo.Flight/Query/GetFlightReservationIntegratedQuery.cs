using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Flight.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.Flight.Query
{
    public class GetFlightReservationIntegratedQuery : QueryBase<GetFlightReservationIntegratedQuery, FlightReservationIntegrated, FlightReservationsTableRecord, FlightTripTableRecord, FlightTripDetailTableRecord, FlightPassengerTableRecord>
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
            if (condition.QueryType == QueryType.Overview)
                clauseBuilder.Append(
                    @"SELECT r.RsvNo, r.RsvTime, r.ContactName, r.RsvStatusCd," +
                    @"t.TripId, t.BookingNumber, " +
                    @"td.TripDetailId, td.SequenceNo, td.CarrierCd, td.FlightNumber, td.DepartureAirportCd, " +
                    @"td.ArrivalAirportCd, td.DepartureTime, td.ArrivalTime, td.Duration, " +
                    @"p.PassengerId, p.FirstName, p.LastName ");
            else
                clauseBuilder.Append(
                    @"SELECT * ");
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
            if (condition.Search.RsvNo != null)
            {
                clauseBuilder.Append(
                    @"r.RsvNo = @RsvNo");
            }
            else
            {
                if (condition.Search.OriginAirport != null)
                    clauseBuilder.Append(
                        @"(td.DepartureAirportCd = @OriginAirport AND td.SequenceNo = 1) AND ");
                if (condition.Search.DestinationAirport != null)
                    clauseBuilder.Append(
                        @"");
                if (condition.Search.RsvDateStart != null)
                    clauseBuilder.Append(
                        condition.Search.RsvDateEnd != null
                            ? @"(r.RsvTime BETWEEN @RsvDateStart AND @RsvDateEnd) AND "
                            : @"r.RsvTime = @RsvDateStart AND ");
                if (condition.Search.IsReturn != null)
                    clauseBuilder.Append(
                        @"t.TripTypeCd = @IsReturn AND ");
                if (condition.Search.DepartureAirline != null)
                    clauseBuilder.Append(
                        @"(td.CarrierCd = @DepartureAirline AND td.SequenceNo = 1) AND ");
                if (condition.Search.ReturnAirline != null)
                    clauseBuilder.Append(
                        @"");
                if (condition.Search.DepartureDateStart != null)
                    clauseBuilder.Append(
                        condition.Search.DepartureDateEnd != null
                            ? @""
                            : @"");
                if (condition.Search.ReturnDateStart != null)
                    clauseBuilder.Append(
                        condition.Search.ReturnDateEnd != null
                            ? @""
                            : @"");
                if (condition.Search.PassengerName != null)
                    clauseBuilder.Append(
                        @"(p.FirstName LIKE '%@PassengerName%' OR p.LastName LIKE '%@PassengerName%') AND ");
                if (condition.Search.ContactName != null)
                    clauseBuilder.Append(
                        @"r.ContactName LIKE '%@ContactName%' AND ");
                clauseBuilder.Remove(clauseBuilder.Length - 5, 5);
            }
            return clauseBuilder.ToString();
        }

        public enum QueryType
        {
            Overview = 0,
            Complete
        }

    }
}
