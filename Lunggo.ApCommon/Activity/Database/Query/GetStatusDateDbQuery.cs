using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetStatusDateDbQuery : DbQueryBase<GetStatusDateDbQuery, string>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "Select DISTINCT acd.DateStatus From ActivityCustomDate AS acd WHERE acd.ActivityId = @ActivityId AND acd.CustomDate = @CustomDate AND acd.AvailableHour = @CustomHour";
        }
    }
}
