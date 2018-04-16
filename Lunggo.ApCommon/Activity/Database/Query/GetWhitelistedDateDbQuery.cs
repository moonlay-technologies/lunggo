using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetWhitelistedDateDbQuery : DbQueryBase<GetWhitelistedDateDbQuery, DateTime>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT DISTINCT acd.CustomDate FROM ActivityCustomDate AS acd WHERE ActivityId = @ActivityId AND DateStatus = 'whitelisted'";
        }
    }
}
