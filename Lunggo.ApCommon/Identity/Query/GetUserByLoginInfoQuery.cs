using System.Text;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
{

    public class GetUserByLoginInfoQuery : QueryBase<GetUserByLoginInfoQuery, GetUserByAnyQueryRecord>
    {
        private GetUserByLoginInfoQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(
                "SELECT u.* FROM Users u inner join UserLogins l on l.UserId = u.Id where l.LoginProvider = @loginProvider and l.ProviderKey = @providerKey"
            );
            return queryBuilder.ToString();
        }
    }
				

}
