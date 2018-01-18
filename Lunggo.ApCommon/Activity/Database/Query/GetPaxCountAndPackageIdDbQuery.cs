using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetPaxCountAndPackageIdDbQuery : DbQueryBase<GetPaxCountAndPackageIdDbQuery, ActivityPackageReservation>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "Select apr.PackageId AS PackageId, apr.Type AS Type, apr.Count AS Count, (SELECT ap.PackageName From ActivityPackage as ap Where ap.Id = apr.PackageId) AS PackageName From ActivityPackageReservation AS apr WHERE apr.RsvId = (Select ar.Id from ActivityReservation AS ar Where RsvNo = @RsvNo)";
        }
    }
}
