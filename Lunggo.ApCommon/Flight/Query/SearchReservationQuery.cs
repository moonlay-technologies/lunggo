﻿using System.Text;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query
{
    public class SearchReservationQuery : DbQueryBase<SearchReservationQuery, string>
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
            clauseBuilder.Append("SELECT DISTINCT r.RsvNo ");
            clauseBuilder.Append("FROM Reservation AS r ");
            clauseBuilder.Append("INNER JOIN FlightItinerary AS i ON r.RsvNo = i.RsvNo ");
            clauseBuilder.Append("INNER JOIN FlightTrip AS t ON i.Id = t.ItineraryId ");
            clauseBuilder.Append("INNER JOIN FlightSegment AS s ON t.Id = s.TripId ");
            clauseBuilder.Append("INNER JOIN Pax AS p ON r.RsvNo = p.RsvNo ");
            clauseBuilder.Append("INNER JOIN Contact AS c ON r.RsvNo = c.RsvNo ");
            clauseBuilder.Append("INNER JOIN Payment AS b ON r.RsvNo = b.RsvNo ");
            clauseBuilder.Append("LEFT OUTER JOIN ReservationState AS v ON r.RsvNo = v.RsvNo ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause(dynamic condition)
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE ");
            if (condition.RsvNo != null)
            {
                clauseBuilder.Append("r.RsvNo = @RsvNo");
            }
            else
            {
                if (condition.ContactName != null)
                    clauseBuilder.Append("c.Name LIKE ('%' + @ContactName + '%') AND ");
                if (condition.ContactEmail != null)
                    clauseBuilder.Append("c.Email LIKE ('%' + @ContactEmail + '%') AND ");
                if (condition.ContactPhone != null)
                    clauseBuilder.Append("c.Phone LIKE ('%' + @ContactPhone + '%') AND ");
                if (condition.PassengerName != null)
                    clauseBuilder.Append("p.FirstName + p.LastName = '%' + @PassengerName + '%' AND ");
                if (condition.Airline != null)
                    clauseBuilder.Append("s.AirlineCd = @Airline AND ");
                if (condition.OriginAirport != null)
                    clauseBuilder.Append("s.OriginAirportCd = @OriginAirport AND ");
                if (condition.DestinationAirport != null)
                    clauseBuilder.Append("s.DestinationAirportCd = @DestinationAirport AND ");
                if (condition.RsvDateSelection != null)
                {
                    switch ((FlightReservationSearch.DateSelectionType)condition.RsvDateSelection)
                    {
                        case FlightReservationSearch.DateSelectionType.Span:
                            if (condition.RsvDateStart != null)
                                clauseBuilder.Append("CONVERT(DATE, r.RsvTime) >= @RsvDateStart AND ");
                            if (condition.RsvDateEnd != null)
                                clauseBuilder.Append("CONVERT(DATE, r.RsvTime) <= @RsvDateEnd AND ");
                            break;
                        case FlightReservationSearch.DateSelectionType.Specific:
                            if (condition.RsvDate != null)
                                clauseBuilder.Append("CONVERT(DATE, r.RsvTime) = @RsvDate AND ");
                            break;
                        case FlightReservationSearch.DateSelectionType.MonthYear:
                            if (condition.RsvDateMonth != null)
                                clauseBuilder.Append("MONTH(r.RsvTime) = @RsvDateMonth AND ");
                            if (condition.RsvDateYear != null)
                                clauseBuilder.Append("YEAR(r.RsvTime) = @RsvDateYear AND ");
                            break;
                    }
                }
                if (condition.DepartureDateSelection != null)
                {
                    switch ((FlightReservationSearch.DateSelectionType)condition.DepartureDateSelection)
                    {
                        case FlightReservationSearch.DateSelectionType.Span:
                            if (condition.DepartureDateStart != null)
                                clauseBuilder.Append("CONVERT(DATE, t.DepartureDate) >= @DepartureDateStart AND ");
                            if (condition.DepartureDateEnd != null)
                                clauseBuilder.Append("CONVERT(DATE, t.DepartureDate) <= @DepartureDateEnd AND ");
                            break;
                        case FlightReservationSearch.DateSelectionType.Specific:
                            if (condition.DepartureDate != null)
                                clauseBuilder.Append("CONVERT(DATE, t.DepartureDate) = @DepartureDate AND ");
                            break;
                        case FlightReservationSearch.DateSelectionType.MonthYear:
                            if (condition.DepartureDateMonth != null)
                                clauseBuilder.Append("MONTH(t.DepartureDate) = @DepartureDateMonth AND ");
                            if (condition.DepartureDateYear != null)
                                clauseBuilder.Append("YEAR(t.DepartureDate) = @DepartureDateYear AND ");
                            break;
                    }
                }
                if (condition.ReturnDateSelection != null)
                {
                    switch ((FlightReservationSearch.DateSelectionType)condition.ReturnDateSelection)
                    {
                        case FlightReservationSearch.DateSelectionType.Span:
                            clauseBuilder.Append("r.OverallTripTypeCd = 'RET AND ");
                            if (condition.ReturnDateStart != null)
                                clauseBuilder.Append("CONVERT(DATE, t.DepartureDate) >= @ReturnDateStart AND ");
                            if (condition.ReturnDateEnd != null)
                                clauseBuilder.Append("CONVERT(DATE, t.DepartureDate) <= @ReturnDateEnd AND ");
                            break;
                        case FlightReservationSearch.DateSelectionType.Specific:
                            if (condition.ReturnDate != null)
                            {
                                clauseBuilder.Append("r.OverallTripTypeCd = 'RET AND ");
                                clauseBuilder.Append("CONVERT(DATE, t.DepartureDate) = @ReturnDate AND ");
                            }
                            break;
                        case FlightReservationSearch.DateSelectionType.MonthYear:
                            if (condition.ReturnDateMonth != null)
                            {
                                clauseBuilder.Append("r.OverallTripTypeCd = 'RET AND ");
                                clauseBuilder.Append("MONTH(t.DepartureDate) = @ReturnDateMonth AND ");
                            }
                            if (condition.ReturnDateYear != null)
                            {
                                clauseBuilder.Append("r.OverallTripTypeCd = 'RET AND ");
                                clauseBuilder.Append("YEAR(t.DepartureDate) = @ReturnDateYear AND ");
                            }
                            break;
                    }
                }
                if (clauseBuilder.Length == 6)
                    clauseBuilder.Clear();
                else
                    clauseBuilder.Remove(clauseBuilder.Length - 5, 5);
            }
            return clauseBuilder.ToString();
        }
    }
}
