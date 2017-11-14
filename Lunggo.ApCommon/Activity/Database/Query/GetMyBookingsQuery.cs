using System.Text;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetMyBookingsQuery : DbQueryBase<GetMyBookingsQuery, BookingDetail>
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
            clauseBuilder.Append("SELECT act.Id AS ActivityId, ar.RsvNo AS RsvNo, ");
            clauseBuilder.Append("act.Name AS Name, rsv.RsvStatusCd AS BookingStatus, rsv.RsvTime AS TimeLimit, ");
            clauseBuilder.Append("ar.TicketCount AS PaxCount, ar.Date AS Date, ar.SelectedSession AS SelectedSession, ");
            clauseBuilder.Append("(SELECT TOP 1 asp.Price FROM ActivitySellPrice AS asp WHERE asp.PackageId=(SELECT Id FROM ActivityPackage WHERE Id=act.Id)) AS Price, ");
            clauseBuilder.Append("(SELECT TOP 1 am.MediaSrc AS MediaSrc FROM ActivityMedia AS am WHERE am.ActivityId=act.Id) AS MediaSrc ");
            return clauseBuilder.ToString();
        }

        private static string CreateJoinClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("FROM ((ActivityReservation AS ar ");
            clauseBuilder.Append("INNER JOIN Activity AS act ON act.Id=ar.ActivityId) ");
            clauseBuilder.Append("INNER JOIN Reservation AS rsv ON rsv.RsvNo=ar.RsvNo) ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE ar.RsvNo IN ");
            clauseBuilder.Append("(SELECT RsvNo FROM Reservation WHERE UserId = @UserId)");
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
