using System.Text;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    public class GetAppointmentDetailQuery : DbQueryBase<GetAppointmentDetailQuery, AppointmentDetail>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause());
            queryBuilder.Append(CreateJoinClause());
            queryBuilder.Append(CreateWhereClause(condition));
            return queryBuilder.ToString();
        }

        private static string CreateSelectClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("SELECT act.Id AS ActivityId, apo.RsvNo AS RsvNo, ");
            clauseBuilder.Append("act.Name AS Name, ar.Date AS Date, ar.SelectedSession AS Session, ");
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

        private static string CreateWhereClause(dynamic condition)
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE ar.ActivityId = @ActivityId AND ");
            clauseBuilder.Append("ar.Date = @Date ");
            if(!string.IsNullOrEmpty(condition.Session))
                clauseBuilder.Append("AND ar.SelectedSession = @Session ");
            return clauseBuilder.ToString();
        }
    }
}
