using System.Text;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    public class GetSearchResultQuery : DbQueryBase<GetSearchResultQuery, SearchResult, SearchResult, DurationActivity>
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
            clauseBuilder.Append("SELECT DISTINCT act.Id AS Id, ");
            clauseBuilder.Append("act.Name AS Name, ");
            clauseBuilder.Append("act.Category AS Category, ");
            clauseBuilder.Append("act.Description AS ShortDesc, ");
            clauseBuilder.Append("act.Address AS Address, ");
            clauseBuilder.Append("act.City AS City, ");
            clauseBuilder.Append("act.Country AS Country, ");
            clauseBuilder.Append("asp.Price AS Price, ");
            clauseBuilder.Append("act.PriceDetail AS PriceDetail, ");
            clauseBuilder.Append("act.AmountDuration AS Amount, ");
            clauseBuilder.Append("act.UnitDuration AS Unit ");
            return clauseBuilder.ToString();
        }

        private static string CreateJoinClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("FROM (((Activity AS act ");
            clauseBuilder.Append("INNER JOIN ActivityPackage AS ap ON ap.ActivityId=act.Id) ");
            clauseBuilder.Append("INNER JOIN ActivitySellPrice AS asp ON asp.PackageId=ap.Id) ");
            clauseBuilder.Append("INNER JOIN ActivityDate AS ad ON ad.ActivityId=act.Id) ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE Name LIKE '%' + @Name + '%' ");
            clauseBuilder.Append("AND Date BETWEEN @StartDate AND @EndDate ");
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
