using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Payment.Database.Query
{
    internal class GetAccountUserQuery : DbQueryBase<GetAccountUserQuery, AccountUserTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT * FROM AccountUser WHERE UserId = @userId";
        }
    }
}
