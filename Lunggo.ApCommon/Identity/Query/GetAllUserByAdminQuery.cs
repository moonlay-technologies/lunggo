using System.Collections.Generic;
using System.Text;
using Lunggo.ApCommon.Identity.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
{
    internal class GetAllUserByAdminQuery : DbQueryBase<GetAllUserByAdminQuery, UserData>
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
            clauseBuilder.Append(@"SELECT a.Email, a.FirstName, a.LastName, c.Name as RoleName ");
            clauseBuilder.Append(@"FROM dbo.[User] AS a ");
            clauseBuilder.Append(@"JOIN dbo.[UserRole] as b ON a.Id = b.UserId ");
            clauseBuilder.Append(@"JOIN dbo.[Role] as c ON c.Id = b.RoleId ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"WHERE a.CompanyId = @CompanyId");
            return clauseBuilder.ToString();
        }
    }
}
