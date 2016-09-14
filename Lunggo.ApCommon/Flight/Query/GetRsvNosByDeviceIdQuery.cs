using System.Linq;
using System.Text;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query
{
    public class GetRsvNosByDeviceIdQuery : QueryBase<GetRsvNosByDeviceIdQuery, string>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause());
            queryBuilder.Append(CreateWhereClause());
            queryBuilder.Append(CreateConditionClause(condition));
            return queryBuilder.ToString();
        }

        private static string CreateSelectClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("SELECT r.RsvNo ");
            clauseBuilder.Append("FROM Reservation AS r ");
            clauseBuilder.Append("INNER JOIN ReservationState AS st ON r.RsvNo = st.RsvNo ");
            clauseBuilder.Append("INNER JOIN Payment AS p ON r.RsvNo = p.RsvNo ");
            clauseBuilder.Append("INNER JOIN FlightItinerary AS i ON r.RsvNo = i.RsvNo ");
            clauseBuilder.Append("INNER JOIN FlightTrip AS t ON i.Id = ");
            clauseBuilder.Append("(SELECT TOP 1 t.ItineraryId FROM FlightTrip WHERE t.ItineraryId = i.Id) ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE st.DeviceId = @DeviceId");
            return clauseBuilder.ToString();
        }

        private static string CreateConditionClause(dynamic condition)
        {
            string[] filters = condition.Filters;
            int? page = condition.Page;
            int itemsPerPage = condition.ItemsPerPage ?? 10;
            var clauseBuilder = new StringBuilder();
            if (filters != null)
            {
                if (filters.Contains("active"))
                    clauseBuilder.Append(" AND (r.RsvTime >= DATEADD(day,-1,GETUTCDATE()) OR r.RsvStatusCd = 'PROC' OR (r.RsvStatusCd = 'COMP' AND t.DepartureDate >= DATEADD(day,-1,GETUTCDATE())))");
                if (filters.Contains("inactive"))
                    clauseBuilder.Append(" AND (r.RsvTime < DATEADD(day,-1,GETUTCDATE()) AND r.RsvStatusCd != 'PROC' AND (r.RsvStatusCd != 'COMP' OR t.DepartureDate < DATEADD(day,-1,GETUTCDATE())))");
                if (filters.Contains("issued"))
                    clauseBuilder.Append(" AND i.BookingStatusCd = 'TKTD'");
            }
            if (page != null)
            {
                clauseBuilder.Append(" ORDER BY r.RsvTime OFFSET " + page * itemsPerPage + " ROWS FETCH NEXT " + itemsPerPage + " ROWS ONLY");
            }
            return clauseBuilder.ToString();
        }
    }
}
