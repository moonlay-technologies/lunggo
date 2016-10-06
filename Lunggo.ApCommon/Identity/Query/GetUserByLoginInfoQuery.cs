using System.Text;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
{

    public class GetUserByLoginInfoQuery : DbQueryBase<GetUserByLoginInfoQuery, GetUserByAnyQueryRecord>
    {
        private GetUserByLoginInfoQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(
                "SELECT u.* " +
                "FROM [User] AS u " +
                "INNER JOIN [UserLogin] AS l ON l.UserId = u.Id " +
                "WHERE l.LoginProvider = @loginProvider AND l.ProviderKey = @providerKey"
            );
            return queryBuilder.ToString();
        }
    }
				

}
