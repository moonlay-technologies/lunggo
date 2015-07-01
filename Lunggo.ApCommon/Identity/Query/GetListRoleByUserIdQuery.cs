using System.Text;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
{
    public class GetListRoleByUserIdQuery : QueryBase<GetListRoleByUserIdQuery, string>
    {
        private GetListRoleByUserIdQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("select b.Name from UserRoles a left join Roles b on a.RoleId = b.Id where a.UserId = @UserId");
            return queryBuilder.ToString();
        }
    }
				
}
