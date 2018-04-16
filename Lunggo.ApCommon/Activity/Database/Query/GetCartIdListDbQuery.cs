using System.Text;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetCartIdListDbQuery : DbQueryBase<GetCartIdListDbQuery, string>
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
            clauseBuilder.Append("SELECT DISTINCT c.CartId AS CartId ");
            return clauseBuilder.ToString();
        }

        private static string CreateJoinClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("FROM Carts AS c ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE c.UserId = @UserId ");
            return clauseBuilder.ToString();
        }

        private static string CreateRangeClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("ORDER BY CartId OFFSET @Page-1 ROWS FETCH NEXT @PerPage ROWS ONLY");
            return clauseBuilder.ToString();
        }
    }
}
