using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetCustomAvailableHoursWhitelistDbQuery : DbQueryBase<GetCustomAvailableHoursWhitelistDbQuery, AvailableSessionAndPaxSlot>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT DISTINCT acd.AvailableHour AS AvailableHour, acd.PaxSlot AS PaxSlot FROM ActivityCustomDate AS acd WHERE CustomDate = @CustomDate AND DateStatus = 'whitelisted' AND ActivityId = @ActivityId" ;
        }

    }
}
