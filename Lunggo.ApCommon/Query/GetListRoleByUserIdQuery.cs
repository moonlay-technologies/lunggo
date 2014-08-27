using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Query
{
    public class GetListRoleByUserIdQuery : QueryBase<GetListRoleByUserIdQuery, String>
    {
        private GetListRoleByUserIdQuery()
        {

        }

        protected override string GetQuery()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("select b.Name from UserRoles a left join AspNetRoles b on a.RoleId = b.Id where a.UserId = @UserId");
            return queryBuilder.ToString();
        }
    }
				
}
