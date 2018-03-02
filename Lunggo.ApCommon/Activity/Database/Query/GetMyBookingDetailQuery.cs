using System.Text;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetMyBookingDetailQuery : DbQueryBase<GetMyBookingDetailQuery, BookingDetail>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause());
            queryBuilder.Append(CreateJoinClause());
            queryBuilder.Append(CreateWhereClause());
            return queryBuilder.ToString();
        }

        private static string CreateSelectClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("SELECT rsv.TicketNumber AS TicketNumber, act.Id AS ActivityId, act.Name AS Name, act.Address AS Address, ");
            clauseBuilder.Append("ar.BookingStatusCd AS BookingStatus, rsv.RsvTime AS TimeLimit, ");
            clauseBuilder.Append("ar.Date AS Date, ar.SelectedSession AS SelectedSession, ");
            clauseBuilder.Append("act.City AS City, act.Latitude AS Latitude, act.Longitude AS Longitude, ");
            clauseBuilder.Append("act.HasPDFVoucher AS HasPdfVoucher, ar.IsPdfUploaded AS IsPdfUploaded, ");
            clauseBuilder.Append("(SELECT TOP 1 am.MediaSrc AS MediaSrc FROM ActivityMedia AS am WHERE am.ActivityId=act.Id) AS MediaSrc, ");
            clauseBuilder.Append("act.OperatorName AS OperatorName, act.OperatorEmail AS OperatorEmail, act.OperatorPhone AS OperatorPhone ");
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
            clauseBuilder.Append("WHERE ar.RsvNo = @RsvNo");
            return clauseBuilder.ToString();
        }
        
    }
}
