using System.Text;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetAppointmentListQuery : DbQueryBase<GetAppointmentListQuery, AppointmentDetail>
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
            clauseBuilder.Append("SELECT DISTINCT act.Id AS ActivityId, act.Name AS Name, ar.RsvNo AS RsvNo, ar.BookingStatusCd AS RsvStatus, ");
            clauseBuilder.Append("SUM(ar.TicketCount) OVER(PARTITION BY act.Id) AS PaxCount, ");
            clauseBuilder.Append("ar.Date AS Date, ar.SelectedSession AS Session, ");
            clauseBuilder.Append("(SELECT TOP 1 am.MediaSrc AS MediaSrc FROM ActivityMedia AS am WHERE am.ActivityId=act.Id) AS MediaSrc ");
            return clauseBuilder.ToString();
        }

        private static string CreateJoinClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("FROM ActivityReservation AS ar ");
            clauseBuilder.Append("INNER JOIN Activity AS act ON act.Id=ar.ActivityId ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE (ar.BookingStatusCd = 'CONF' OR ar.BookingStatusCd = 'CACU' OR ar.BookingStatusCd = 'CAAD' OR ar.BookingStatusCd = 'CAOP') AND ");
            clauseBuilder.Append("(SELECT UserId FROM Operator WHERE ActivityId = act.Id) = @UserId AND ar.Date BETWEEN @StartDate AND @EndDate ");
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
