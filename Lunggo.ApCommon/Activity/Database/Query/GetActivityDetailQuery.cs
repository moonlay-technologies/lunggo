using System.Text;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    public class GetActivityDetailQuery : DbQueryBase<GetActivityDetailQuery, ActivityDetail, ActivityDetail, DurationActivity>
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
            clauseBuilder.Append("act.OperatorName AS OperatorName, act.OperatorEmail AS OperatorEmail, act.OperatorPhone AS OperatorPhone, ");
            clauseBuilder.Append("act.ImportantNotice AS ImportantNotice, act.Warning AS Warning, act.AdditionalNotes AS AdditionalNotes, ");
            clauseBuilder.Append("act.PriceDetail AS PriceDetail, act.Cancellation AS Cancellation, ");
            clauseBuilder.Append("act.IsPassportNeeded AS IsPassportNeeded, act.IsPassportIssueDateNeeded AS IsPassportIssueDateNeeded, ");
            clauseBuilder.Append("act.IsDateOfBirthNeeded AS IsDateOfBirthNeeded, ");
            clauseBuilder.Append("act.AmountDuration AS Amount, act.UnitDuration AS Unit ");
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
