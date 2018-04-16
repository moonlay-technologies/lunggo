using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class DeleteDaySessionDbQuery : NoReturnDbQueryBase<DeleteDaySessionDbQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "DELETE FROM ActivityRegularDate WHERE ActivityId = @ActivityId AND AvailableDay = @AvailableDay";
        }
    }
}
