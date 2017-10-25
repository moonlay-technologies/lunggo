using System.Text;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetActivityDetailQuery : DbQueryBase<GetActivityDetailQuery, ActivityDetail>
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
            clauseBuilder.Append("SELECT act.Id AS ActivityId, act.Name AS Name, ");
            clauseBuilder.Append("act.Description AS ShortDesc, act.Address AS Address, ");
            clauseBuilder.Append("act.City AS City, act.Country AS Country, ");
            clauseBuilder.Append("act.Latitude AS Latitude, act.Longitude AS Longitude, ");
            clauseBuilder.Append("act.ImportantNotice AS ImportantNotice, act.Warning AS Warning, ");
            clauseBuilder.Append("act.OperationTime AS OperationTime, asp.Price AS Price, ");
            clauseBuilder.Append("act.Cancellation AS Cancellation ");
            return clauseBuilder.ToString();
        }

        private static string CreateJoinClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("FROM ((Activity AS act ");
            clauseBuilder.Append("INNER JOIN ActivityPackage AS ap ON ap.ActivityId=act.Id) ");
            clauseBuilder.Append("INNER JOIN ActivitySellPrice AS asp ON asp.PackageId=ap.Id) ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE act.Id = @ActivityId");
            return clauseBuilder.ToString();
        }
    }
}
