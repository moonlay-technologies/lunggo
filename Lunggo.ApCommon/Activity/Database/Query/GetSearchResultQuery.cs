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
            queryBuilder.Append(CreateWhereClause(condition));
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
            clauseBuilder.Append("(SELECT TOP 1 am.MediaSrc AS MediaSrc FROM ActivityMedia AS am WHERE am.ActivityId=act.Id) AS MediaSrc, ");
            clauseBuilder.Append("(SELECT CASE WHEN (count(*) > 0) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END from wishlist where UserId=@userId and ActivityId=act.id) AS Wishlisted, ");
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

        private static string CreateWhereClause(dynamic condition)
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE ");
            if (condition.Name != null)
                clauseBuilder.Append("act.Name LIKE '%' + @Name + '%' AND ");
            if (condition.Id != null)
                clauseBuilder.Append("act.Id IN @Id AND ");
            if (condition.Id == null)
                clauseBuilder.Append("ad.Date BETWEEN @StartDate AND @EndDate AND asp.Flag = 1 AND ");
            clauseBuilder.Remove(clauseBuilder.Length - 4, 4);
            return clauseBuilder.ToString();
        }

        private static string CreateRangeClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("ORDER BY act.Name OFFSET @Page-1 ROWS FETCH NEXT @PerPage ROWS ONLY");
            return clauseBuilder.ToString();
        }
    }
}
