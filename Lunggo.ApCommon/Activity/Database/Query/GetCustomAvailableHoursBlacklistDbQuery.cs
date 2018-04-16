using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetCustomAvailableHoursBlacklistDbQuery : DbQueryBase<GetCustomAvailableHoursBlacklistDbQuery, string>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT DISTINCT acd.AvailableHour FROM ActivityCustomDate AS acd WHERE CustomDate = @CustomDate AND DateStatus = 'blacklisted' AND ActivityId = @ActivityId";
        }

    }
}
