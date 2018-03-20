using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class UpdateViewCountDbQuery : NoReturnDbQueryBase<UpdateViewCountDbQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "UPDATE Activity SET ViewCount = @ViewCount WHERE Id = @ActivityId";
        }
    }
}

