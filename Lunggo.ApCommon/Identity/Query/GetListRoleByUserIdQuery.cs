using System.Text;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
{
    public class GetListRoleByUserIdQuery : DbQueryBase<GetListRoleByUserIdQuery, string>
    {
        private GetListRoleByUserIdQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT b.Name " +
                                "FROM [UserRole] AS a " +
                                "LEFT JOIN [Role] AS b ON a.RoleId = b.Id " +
                                "WHERE a.UserId = @UserId");
            return queryBuilder.ToString();
        }
    }
				
}
