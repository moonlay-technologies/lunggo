using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Payment.Query
{
    internal class GetAccountUserQuery : DbQueryBase<GetAccountUserQuery, AccountUserTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT * FROM AccountUser WHERE UserId = @userId";
        }
    }
}
