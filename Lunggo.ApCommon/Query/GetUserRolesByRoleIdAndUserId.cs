using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Query
{
    public class GetUserRolesByRoleIdAndUserId : QueryBase<GetUserRolesByRoleIdAndUserId, GetUserRolesByAnyQueryRecord>
    {
        private GetUserRolesByRoleIdAndUserId()
        {

        }

        protected override string GetQuery()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("select * from UserRoles where RoleId = @RoleId and UserId = @UserId");
            return queryBuilder.ToString();
        }
    }
				
}
