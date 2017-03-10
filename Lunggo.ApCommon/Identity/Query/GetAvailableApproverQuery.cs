using System.Text;
using Lunggo.ApCommon.Identity.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
{
    public class GetAvailableApproverQuery : DbQueryBase<GetAvailableApproverQuery, ApproverData>
    {
        private GetAvailableApproverQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT a.Id AS UserId, a.FirstName as Name " +
                                "FROM [User] AS a");
            return queryBuilder.ToString();
        }
    }

}

