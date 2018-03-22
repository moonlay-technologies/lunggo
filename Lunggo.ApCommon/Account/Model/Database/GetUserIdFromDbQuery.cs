using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Account.Model.Database
{
    public class GetUserIdFromDbQuery : DbQueryBase<GetUserIdFromDbQuery, string>
    {
        private GetUserIdFromDbQuery()
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
            long phone;
            bool IsNumeric = Int64.TryParse(condition.Contact, out phone);
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("SELECT Id FROM [User] ");
            if (IsNumeric)
            {
                clauseBuilder.Append("WHERE PhoneNumber = @PhoneNumber AND CountryCallCd = @CountryCallCd");
            }
            else if (condition.Contact.Contains("@"))
            {
                clauseBuilder.Append("WHERE Email = @Contact");
            }
            else
            {
                clauseBuilder.Append("WHERE UserName = @Contact");
            }

            return clauseBuilder.ToString();
        }
    }
}
