using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
{
    public class GetListApproverEmailByCompanyIdQuery : DbQueryBase<GetListApproverEmailByCompanyIdQuery, string>
    {
        private GetListApproverEmailByCompanyIdQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT [User].Email " +
                                "FROM [User] " +
                                "LEFT JOIN [UserRole] ON [User].Id = UserRole.UserId " +
                                "LEFT JOIN [Role] ON UserRole.RoleId = [Role].Id " +
                                "WHERE [Role].Name = 'Approver' AND [User].CompanyId = @CompanyId");
            return queryBuilder.ToString();
        }
    }
}
