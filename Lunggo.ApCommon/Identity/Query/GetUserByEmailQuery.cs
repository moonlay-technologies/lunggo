using System.Text;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
{

    public class GetUserByEmailQuery : DbQueryBase<GetUserByEmailQuery, GetUserByAnyQueryRecord>
    {
        private GetUserByEmailQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM [User] WHERE Email = @Email");
            return queryBuilder.ToString();
        }
    }
				
}
