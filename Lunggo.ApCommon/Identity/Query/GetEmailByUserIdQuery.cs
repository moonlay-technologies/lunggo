using System.Text;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
{

    public class GetEmailByUserIdQuery : DbQueryBase<GetEmailByUserIdQuery, string>
    {
        private GetEmailByUserIdQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT [User].Email FROM [User] WHERE [User].Id = @userId");
            return queryBuilder.ToString();
        }
    }
}