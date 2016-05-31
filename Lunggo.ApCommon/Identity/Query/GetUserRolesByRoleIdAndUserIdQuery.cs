using System.Text;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
{
    public class GetUserRolesByRoleIdAndUserIdQuery : QueryBase<GetUserRolesByRoleIdAndUserIdQuery, GetUserRolesByAnyQueryRecord>
    {
        private GetUserRolesByRoleIdAndUserIdQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * " +
                                "FROM [UserRole] " +
                                "WHERE RoleId = @RoleId AND UserId = @UserId");
            return queryBuilder.ToString();
        }
    }
				
}
