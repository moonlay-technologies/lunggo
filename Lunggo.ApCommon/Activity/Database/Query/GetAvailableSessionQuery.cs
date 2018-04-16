using System.Text;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetAvailableSessionQuery : DbQueryBase<GetAvailableSessionQuery, string>
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
            clauseBuilder.Append("SELECT AvailableHour ");
            return clauseBuilder.ToString();
        }

        private static string CreateJoinClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("FROM ActivityRegularDate ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE ActivityRegularDate.ActivityId = @ActivityId AND ActivityRegularDate.AvailableDay = @AvailableDay");
            return clauseBuilder.ToString();
        }
    }
}
