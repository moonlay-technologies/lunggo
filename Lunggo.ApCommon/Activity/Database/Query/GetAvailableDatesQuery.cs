using System.Text;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetAvailableDatesQuery : DbQueryBase<GetAvailableDatesQuery, string>
    {
        /*protected override string GetQuery(dynamic condition = null)
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
            clauseBuilder.Append("SELECT DISTINCT Date ");
            return clauseBuilder.ToString();
        }

        private static string CreateJoinClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("FROM ActivityDate ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE ActivityDate.ActivityId = @ActivityId ORDER BY Date");
            return clauseBuilder.ToString();
        }*/

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
            clauseBuilder.Append("SELECT DISTINCT AvailableDay ");
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
            clauseBuilder.Append("WHERE ActivityRegularDate.ActivityId = @ActivityId");
            return clauseBuilder.ToString();
        }
    }
}
