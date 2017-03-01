using System.Text;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
{
    public class GetListRolesQuery : DbQueryBase<GetListRolesQuery, string>
    {
        private GetListRolesQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT a.Name " +
                                "FROM [Role] AS a");
            return queryBuilder.ToString();
        }
    }

}
