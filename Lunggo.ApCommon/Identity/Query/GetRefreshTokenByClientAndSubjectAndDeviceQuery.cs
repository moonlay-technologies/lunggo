using System.Text;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Identity.Query
{
    public class GetRefreshTokenByClientAndSubjectAndDeviceQuery : DbQueryBase<GetRefreshTokenByClientAndSubjectAndDeviceQuery, RefreshTokenTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM [RefreshToken] WHERE Subject = @Subject AND ClientId = @ClientId AND DeviceID = @DeviceId");
            return queryBuilder.ToString();
        }
    }
				
}
