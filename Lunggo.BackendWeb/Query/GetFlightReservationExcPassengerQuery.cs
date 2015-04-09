using System.Text;
using Lunggo.BackendWeb.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.BackendWeb.Query
{
    public class GetFlightReservationExcPassengerQuery : QueryBase<GetFlightReservationExcPassengerQuery, FlightReservationIntegrated, FlightReservationsTableRecord, FlightTripTableRecord, FlightTripDetailTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause(condition));
            queryBuilder.Append(CreateWhereClause(condition));
            if (condition.QueryType == QueryType.Complete)
                queryBuilder.Append(" ORDER BY td.SequenceNo");
            return queryBuilder.ToString();
        }

        private static string CreateSelectClause(dynamic condition = null)
        {
            var clauseBuilder = new StringBuilder();
            if (condition.QueryType == QueryType.PrimKeys)
                clauseBuilder.Append(
                    @"SELECT DISTINCT r.RsvNo, t.TripId, td.TripDetailId ");
            else if (condition.QueryType == QueryType.Overview)
                clauseBuilder.Append(
                    @"SELECT DISTINCT r.RsvNo, r.ContactName, r.RsvTime, " +
                    "r.PaymentMethodCd, r.PaymentStatusCd, r.FinalPrice, " +
                    "t.TripId, t.TripTypeCd, t.OriginAirportCd, t.DestinationAirportCd, t.DepartureTime, t.ReturnTime, " +
                    "td.TripDetailId, td.CarrierCd ");
            else
                clauseBuilder.Append(
                    @"SELECT DISTINCT r.*, t.*, td.* ");
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
            if (condition.Param.RsvNo != null)
                clauseBuilder.Append(
                    @"r.RsvNo = @RsvNo");
            else
            {
                if (condition.Param.OriginAirport != null)
                    clauseBuilder.Append(
                        @"t.OriginAirportCd = @OriginAirport AND ");
                if (condition.Param.DestinationAirport != null)
                    clauseBuilder.Append(
                        @"t.DestinationAirportCd = @DestinationAirport AND ");
                if (condition.Param.RsvDateSelection != null)
                {
                    switch ((int) condition.Param.RsvDateSelection)
                    {
                        case (int) FlightReservationSearch.DateSelection.Specific:
                            clauseBuilder.Append(
                                @"(r.RsvTime BETWEEN @RsvDate AND @RsvDate + ' 23:59:59') AND ");
                            break;
                        case (int) FlightReservationSearch.DateSelection.Range:
                            clauseBuilder.Append(
                                @"(r.RsvTime BETWEEN @RsvDateStart AND @RsvDateEnd + ' 23:59:59') AND ");
                            break;
                        case (int) FlightReservationSearch.DateSelection.MonthYear:
                            clauseBuilder.Append(
                                @"(MONTH(r.RsvTime) = @RsvMonth AND YEAR(r.RsvTime) = @RsvYear) AND ");
                            break;
                    }
                }
                if (condition.Param.DepartureAirline != null)
                    clauseBuilder.Append(
                        @"(td.CarrierCd = @DepartureAirline AND (td.DepartureAirportCd = t.OriginAirportCd OR td.ArrivalAirportCd = t.DestinationAirportCd)) AND ");
                if (condition.Param.ReturnAirline != null)
                    clauseBuilder.Append(
                        @"(td.CarrierCd = @ReturnAirline AND (td.DepartureAirportCd = t.DestinationAirportCd OR td.ArrivalAirportCd = t.OriginAirportCd)) AND ");
                if (condition.Param.DepartureDateSelection != null)
                {
                    switch ((int) condition.Param.DepartureDateSelection)
                    {
                        case (int) FlightReservationSearch.DateSelection.Specific:
                            clauseBuilder.Append(
                                @"(t.DepartureTime BETWEEN @DepartureDate AND @DepartureDate + ' 23:59:59') AND ");
                            break;
                        case (int) FlightReservationSearch.DateSelection.Range:
                            clauseBuilder.Append(
                                @"(t.DepartureTime BETWEEN @DepartureDateStart AND @DepartureDateEnd + ' 23:59:59') AND ");
                            break;
                        case (int) FlightReservationSearch.DateSelection.MonthYear:
                            clauseBuilder.Append(
                                @"(MONTH(t.DepartureTime) = @DepartureMonth AND YEAR(t.DepartureTime) = @DepartureYear) AND ");
                            break;
                    }
                }
                if (condition.Param.ReturnDateSelection != null)
                {
                    switch ((int) condition.Param.ReturnDateSelection)
                    {
                        case (int) FlightReservationSearch.DateSelection.Specific:
                            clauseBuilder.Append(
                                @"(t.ReturnTime BETWEEN @ReturnDate AND @ReturnDate + ' 23:59:59') AND ");
                            break;
                        case (int) FlightReservationSearch.DateSelection.Range:
                            clauseBuilder.Append(
                                @"(t.ReturnTime BETWEEN @ReturnDateStart AND @ReturnDateEnd + ' 23:59:59') AND ");
                            break;
                        case (int) FlightReservationSearch.DateSelection.MonthYear:
                            clauseBuilder.Append(
                                @"(MONTH(t.ReturnTime) = @ReturnMonth AND YEAR(t.ReturnTime) = @ReturnYear) AND ");
                            break;
                    }
                }
                if (condition.Param.PassengerName != null)
                {
                    clauseBuilder.Append(
                        @"(p.FirstName LIKE '%' + @PassengerName + '%' OR p.LastName LIKE '%' + @PassengerName + '%' OR (p.FirstName + ' ' + p.LastName) LIKE '%' + @PassengerName + '%') AND ");
                }
                if (condition.Param.ContactName != null)
                    clauseBuilder.Append(
                        @"r.ContactName LIKE '%' + @ContactName + '%' AND ");
                if (clauseBuilder.Length == 6)
                    clauseBuilder.Clear();
                else
                    clauseBuilder.Remove(clauseBuilder.Length - 5, 5);
            }
            return clauseBuilder.ToString();
        }

    }
}
