using System.Text;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
{

    public class GetRoleByNameQuery : QueryBase<GetRoleByNameQuery, GetRoleByAnyQueryRecord>
    {
        private GetRoleByNameQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM Roles WHERE Upper(Name) = Upper(@Name)");
            return queryBuilder.ToString();
        }
    }
				
}
