using System.Text;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.Framework.Database;
using System;

namespace Lunggo.ApCommon.Identity.Query
{

    public class GetUserByNameQuery : DbQueryBase<GetUserByNameQuery, GetUserByAnyQueryRecord>
    {
        private GetUserByNameQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateConditionClause(condition));
            return queryBuilder.ToString();
        }

        private static string CreateConditionClause(dynamic condition)
        {
            bool IsNumeric = Int64.TryParse(condition.userName, out long phone);
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("SELECT * FROM [User] WHERE LOWER(UserName) LIKE");
            if (IsNumeric)
            {
                clauseBuilder.Append(" '%' + @userName + ':%' ");
            }
            else
            {
                clauseBuilder.Append(" LOWER('%:' + @userName + '%')");
            }
            
            return clauseBuilder.ToString();
        }
    }
}			

