using System.Text;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
{

    public class GetNameByUserIdQuery : DbQueryBase<GetNameByUserIdQuery, string>
    {
        private GetNameByUserIdQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT [User].FirstName FROM [User] WHERE [User].Id = @userId");
            return queryBuilder.ToString();
        }
    }
}