using Lunggo.Framework.Database;
using System.Text;

namespace Lunggo.ApCommon.Campaign.Database.Query
{
    internal class GetMemberRecordQuery : QueryBase<GetMemberRecordQuery, string>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause());
            queryBuilder.Append(CreateWhereClause());
            return queryBuilder.ToString();
        }

        private static string CreateSelectClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("SELECT Email ");
            clauseBuilder.Append("FROM [User] ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE Email = @Email ");
            return clauseBuilder.ToString();
        }
    }
}
