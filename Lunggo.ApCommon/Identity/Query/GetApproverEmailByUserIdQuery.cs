using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
{
    public class GetApproverEmailByUserIdQuery : DbQueryBase<GetApproverEmailByUserIdQuery, string>
    {
        private GetApproverEmailByUserIdQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT [User].Email " +
                                "FROM [User] " +
                                "LEFT JOIN [UserApprover] ON [User].Id = UserApprover.ApproverId " +
                                "WHERE [UserApprover].UserId = @userId");
            return queryBuilder.ToString();
        }
    }
}