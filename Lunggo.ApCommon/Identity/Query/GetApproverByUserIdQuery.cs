using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
{

    public class GetApproverByUserIdQuery : DbQueryBase<GetApproverByUserIdQuery, string>
    {
        private GetApproverByUserIdQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT [Role].ApproverId FROM [Role] WHERE UserId = @UserId");
            return queryBuilder.ToString();
        }
    }

}

