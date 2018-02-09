using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetRatingFromDbByRsvNoQuery : DbQueryBase<GetRatingFromDbByRsvNoQuery, string>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT DISTINCT ar.RsvNo FROM ActivityRating AS ar WHERE RsvNo = @RsvNo";
        }
    }
}
