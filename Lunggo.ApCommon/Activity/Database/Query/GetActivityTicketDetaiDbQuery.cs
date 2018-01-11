using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetActivityTicketDetailDbQuery : DbQueryBase<GetActivityTicketDetailDbQuery, ActivityPricePackage>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "Select asp.Type, asp.Description ,asp.Price AS Amount From ActivityPackage AS ap INNER JOIN ActivitySellPrice As asp ON asp.PackageId = ap.Id  WHERE ap.Id = @PackageId";
        }
    }
}
