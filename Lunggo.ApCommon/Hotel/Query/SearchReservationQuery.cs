using System.Text;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Hotel.Query
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
            clauseBuilder.Append("INNER JOIN HotelReservationDetails AS d ON r.RsvNo = d.RsvNo ");
            clauseBuilder.Append("INNER JOIN HotelRoom AS ro ON d.Id = ro.DetailsId ");
            clauseBuilder.Append("INNER JOIN HotelRate AS ra ON ro.Id = ra.RoomId ");
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
                if (condition.PaxName != null)
                    clauseBuilder.Append("p.FirstName + p.LastName = '%' + @PaxName + '%' AND ");
                if (condition.RsvDateSelection != null)
                {
                    switch ((HotelReservationSearch.DateSelectionType)condition.RsvDateSelection)
                    {
                        case HotelReservationSearch.DateSelectionType.Span:
                            if (condition.RsvDateStart != null)
                                clauseBuilder.Append("CONVERT(DATE, r.RsvTime) >= @RsvDateStart AND ");
                            if (condition.RsvDateEnd != null)
                                clauseBuilder.Append("CONVERT(DATE, r.RsvTime) <= @RsvDateEnd AND ");
                            break;
                        case HotelReservationSearch.DateSelectionType.Specific:
                            if (condition.RsvDate != null)
                                clauseBuilder.Append("CONVERT(DATE, r.RsvTime) = @RsvDate AND ");
                            break;
                        case HotelReservationSearch.DateSelectionType.MonthYear:
                            if (condition.RsvDateMonth != null)
                                clauseBuilder.Append("MONTH(r.RsvTime) = @RsvDateMonth AND ");
                            if (condition.RsvDateYear != null)
                                clauseBuilder.Append("YEAR(r.RsvTime) = @RsvDateYear AND ");
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
