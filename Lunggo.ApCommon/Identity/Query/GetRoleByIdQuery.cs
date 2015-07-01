using System.Text;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
{

    public class GetRoleByIdQuery : QueryBase<GetRoleByIdQuery, GetRoleByAnyQueryRecord>
    {
        private GetRoleByIdQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM Roles WHERE Id = @Id");
            return queryBuilder.ToString();
        }
    }
				
}
