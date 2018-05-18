using System.Text;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetAppointmentRequestQuery : DbQueryBase<GetAppointmentRequestQuery, AppointmentDetail>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause());
            queryBuilder.Append(CreateJoinClause());
            queryBuilder.Append(CreateWhereClause());
            queryBuilder.Append(CreateRangeClause());
            return queryBuilder.ToString();
        }

        private static string CreateSelectClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("SELECT act.Id AS ActivityId, ar.RsvNo AS RsvNo, act.Name AS Name, ");
            clauseBuilder.Append("ar.TicketCount AS PaxCount, ar.Date AS Date, ");
            clauseBuilder.Append("ar.SelectedSession AS Session, ");
            clauseBuilder.Append("r.RsvTime AS RequestTime, ");
            clauseBuilder.Append("(SELECT TOP 1 am.MediaSrc AS MediaSrc FROM ActivityMedia AS am WHERE am.ActivityId=act.Id) AS MediaSrc ");
            return clauseBuilder.ToString();
        }

        private static string CreateJoinClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("FROM (((ActivityReservation AS ar ");
            clauseBuilder.Append("INNER JOIN Activity AS act ON act.Id=ar.ActivityId) ");
            clauseBuilder.Append("INNER JOIN Reservation AS r ON r.RsvNo=ar.RsvNo) ");
            clauseBuilder.Append("INNER JOIN Payment AS p ON p.RsvNo=ar.RsvNo) ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE ar.BookingStatusCd = 'FORW' AND p.StatusCd LIKE 'SET%' AND ");
            clauseBuilder.Append("(SELECT UserId FROM Operator WHERE ActivityId = act.Id) = @UserId ");
            return clauseBuilder.ToString();
        }

        private static string CreateRangeClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("ORDER BY Name OFFSET @Page-1 ROWS FETCH NEXT @PerPage ROWS ONLY");
            return clauseBuilder.ToString();
        }
    }
}
