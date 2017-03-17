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
            queryBuilder.Append("SELECT a.Email " +
                                "FROM [User] as a " +
                                "LEFT JOIN [User] as b ON a.Id = b.ApproverId " +
                                "WHERE b.Id = @userId");
            return queryBuilder.ToString();
        }
    }
}