using System.Text;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    public class GetActivityDetailQuery : DbQueryBase<GetActivityDetailQuery, ActivityDetail, ActivityDetail, DurationActivity, Content>
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
            clauseBuilder.Append("SELECT act.Id AS ActivityId, act.Name AS Name, act.Category AS Category, ");
            clauseBuilder.Append("act.Description AS ShortDesc, act.Address AS Address, ");
            clauseBuilder.Append("act.City AS City, act.Country AS Country, ");
            clauseBuilder.Append("act.Latitude AS Latitude, act.Longitude AS Longitude, ");
            clauseBuilder.Append("act.OperationTime AS OperationTime, asp.Price AS Price, ");
            clauseBuilder.Append("act.PriceDetail AS PriceDetail, act.Cancellation AS Cancellation, ");
            clauseBuilder.Append("act.IsPassportNumberNeeded AS IsPassportNumberNeeded, act.IsPassportIssuedDateNeeded AS IsPassportIssuedDateNeeded, ");
            clauseBuilder.Append("act.IsPaxDoBNeeded AS IsPaxDoBNeeded, ");
            clauseBuilder.Append("act.AmountDuration AS Amount, act.UnitDuration AS Unit, ");
            clauseBuilder.Append("act.ImportantNotice AS Content1, act.Warning AS Content2, act.AdditionalNotes AS Content3 ");
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
