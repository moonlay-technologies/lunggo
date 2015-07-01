using System.Text;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
{

    public class GetUserByNameQuery : QueryBase<GetUserByNameQuery, GetUserByAnyQueryRecord>
    {
        private GetUserByNameQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM Users WHERE LOWER(UserName) = LOWER(@userName)");
            return queryBuilder.ToString();
        }
    }
}			

