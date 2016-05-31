using System.Text;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
{
    


    public class GetLoginInfoByUserIdQuery : QueryBase<GetLoginInfoByUserIdQuery, GetUserLoginInfoByAnyQueryRecord>
    {
        private GetLoginInfoByUserIdQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT LoginProvider, ProviderKey " +
                                "FROM [UserLogin] " +
                                "WHERE UserId = @Id");
            return queryBuilder.ToString();
        }
    }
				

}
