using System.Text;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Identity.Query
{
    public class GetClientByIdQuery : DbQueryBase<GetClientByIdQuery, ClientTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM [Client] WHERE Id = @Id");
            return queryBuilder.ToString();
        }
    }
				
}
