using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
{

    public class GetCompanyIdByUserQuery : DbQueryBase<GetCompanyIdByUserQuery, string>
    {
        private GetCompanyIdByUserQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT [User].CompanyId FROM [User] WHERE Id = @UserId");
            return queryBuilder.ToString();
        }
    }

}