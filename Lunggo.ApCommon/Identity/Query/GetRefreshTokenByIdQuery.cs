using System.Text;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Identity.Query
{
    public class GetRefreshTokenByIdQuery : QueryBase<GetRefreshTokenByIdQuery, RefreshTokenTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM [RefreshToken] WHERE ClientId = @ClientId");
            return queryBuilder.ToString();
        }
    }
				
}
