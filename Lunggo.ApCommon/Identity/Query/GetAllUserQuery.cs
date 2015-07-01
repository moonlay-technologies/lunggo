using System.Text;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
{

    public class GetAllUserQuery : QueryBase<GetAllUserQuery,GetUserByAnyQueryRecord>
    {
        private GetAllUserQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("select * from Users");
            return queryBuilder.ToString();
        }
    }
				
}
