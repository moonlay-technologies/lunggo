using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetActivityPackageDbQuery : DbQueryBase<GetActivityPackageDbQuery, ActivityPackage>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "Select ap.PackageName, ap.AdditionalNotes AS Description, ap.MaxCount, ap.MinCount From ActivityPackage AS ap WHERE ap.Id = @PackageId";
        }
    }
}
