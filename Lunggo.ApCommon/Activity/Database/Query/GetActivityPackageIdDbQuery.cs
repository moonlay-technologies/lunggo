using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetActivityPackageIdDbQuery : DbQueryBase<GetActivityPackageIdDbQuery, long>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "Select ap.Id From ActivityPackage AS ap WHERE ap.ActivityId = @ActivityId";
        }
    }
}
