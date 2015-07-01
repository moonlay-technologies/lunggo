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
            queryBuilder.Append("select * from UserRoles where RoleId = @RoleId and UserId = @UserId");
            return queryBuilder.ToString();
        }
    }
				
}
