﻿using System.Text;
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
            clauseBuilder.Append("SELECT act.Id AS ActivityId, act.Name AS Name, ");
            clauseBuilder.Append("ar.TicketCount AS PaxCount, ar.Date AS Date, ");
            clauseBuilder.Append("apo.InsertDate AS RequestTime, ar.SelectedSession AS Session, ");
            clauseBuilder.Append("(SELECT TOP 1 am.MediaSrc AS MediaSrc FROM ActivityMedia AS am WHERE am.ActivityId=act.Id) AS MediaSrc ");
            return clauseBuilder.ToString();
        }

        private static string CreateJoinClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("FROM ((Appointment AS apo ");
            clauseBuilder.Append("INNER JOIN ActivityReservation AS ar ON ar.RsvNo=apo.RsvNo) ");
            clauseBuilder.Append("INNER JOIN Activity AS act ON act.Id=ar.ActivityId) ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE apo.AppointmentStatus = 'Requesting' AND ");
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
