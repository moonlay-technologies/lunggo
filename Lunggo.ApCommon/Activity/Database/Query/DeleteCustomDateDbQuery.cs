using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class DeleteCustomDateDbQuery : NoReturnDbQueryBase<DeleteCustomDateDbQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "DELETE FROM ActivityCustomDate WHERE ActivityId = @ActivityId AND CustomDate = @CustomDate AND AvailableHour = @AvailableHour";
        }
    }
}
