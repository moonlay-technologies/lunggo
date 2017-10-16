using System.Text;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetSearchResultByNameQuery : DbQueryBase<GetSearchResultByNameQuery, SearchResult>
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
            clauseBuilder.Append("SELECT act.Id AS Id, ");
            clauseBuilder.Append("act.Name AS Name, ");
            clauseBuilder.Append("act.Description AS Description, ");
            clauseBuilder.Append("act.City AS City, ");
            clauseBuilder.Append("act.Country AS Country, ");
            clauseBuilder.Append("act.OperationTime AS OperationTime, ");
            clauseBuilder.Append("asp.Price AS Price, ");
            clauseBuilder.Append("acd.Date AS CloseDate, ");
            clauseBuilder.Append("act.ImgSrc AS ImgSrc ");
            return clauseBuilder.ToString();
        }

        private static string CreateJoinClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("FROM (((Activity AS act ");
            clauseBuilder.Append("INNER JOIN ActivityPackage AS ap ON ap.ActivityId=act.Id) ");
            clauseBuilder.Append("INNER JOIN ActivitySellPrice AS asp ON asp.PackageId=ap.Id) ");
            clauseBuilder.Append("INNER JOIN ActivityCloseDate AS acd ON acd.ActivityId=act.Id) ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE Name LIKE '%' + @Name + '%' ");
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
